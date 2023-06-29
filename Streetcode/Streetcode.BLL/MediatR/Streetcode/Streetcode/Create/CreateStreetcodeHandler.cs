using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Factories.Streetcode;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;

public class CreateStreetcodeHandler : IRequestHandler<CreateStreetcodeCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CreateStreetcodeHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<int>> Handle(CreateStreetcodeCommand request, CancellationToken cancellationToken)
    {
        using(var transactionScope = _repositoryWrapper.BeginTransaction())
        {
            try
            {
                var streetcode = StreetcodeFactory.CreateStreetcode(request.Streetcode.StreetcodeType);
                _mapper.Map(request.Streetcode, streetcode);

                _repositoryWrapper.StreetcodeRepository.Create(streetcode);
                var isResultSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
                await AddTimelineItems(streetcode, request.Streetcode.TimelineItems);
                await AddImagesAsync(streetcode, request.Streetcode.ImagesIds);
                AddAudio(streetcode, request.Streetcode.AudioId);
                AddArts(streetcode, request.Streetcode.StreetcodeArts);

                await AddTags(streetcode, request.Streetcode.Tags.ToList());

                await AddRelatedFigures(streetcode, request.Streetcode.RelatedFigures);
                await AddPartners(streetcode, request.Streetcode.Partners);
                await AddToponyms(streetcode, request.Streetcode.Toponyms);
                AddStatisticRecords(streetcode, request.Streetcode.StatisticRecords);
                AddTransactionLink(streetcode, request.Streetcode.ARBlockURL);
                await _repositoryWrapper.SaveChangesAsync();
                await AddFactImageDescription(request.Streetcode.Facts);

                if (isResultSuccess)
                {
                    transactionScope.Complete();
                    return Result.Ok(streetcode.Id);
                }
                else
                {
                    return Result.Fail(new Error("Failed to create a streetcode"));
                }
            }
            catch(Exception ex)
            {
                return Result.Fail(new Error($"An error occurred while creating a streetcode. Message: {ex.Message}"));
            }
        }
    }

    private async Task AddFactImageDescription(IEnumerable<FactUpdateCreateDto> facts)
    {
        foreach(FactUpdateCreateDto fact in facts)
        {
            if(fact.ImageDescription != null)
            {
                _repositoryWrapper.ImageDetailsRepository.Create(new ImageDetails()
                {
                    Alt = fact.ImageDescription, ImageId = fact.ImageId
                });
            }
        }

        await _repositoryWrapper.SaveChangesAsync();
    }

    private void AddTransactionLink(StreetcodeContent streetcode, string? url)
    {
        if(url != null)
        {
            streetcode.TransactionLink = new DAL.Entities.Transactions.TransactionLink()
            {
                Url = url,
                UrlTitle = url,
            };
        }
    }

    private void AddStatisticRecords(StreetcodeContent streetcode, IEnumerable<StatisticRecordDTO> statisticRecords)
    {
        var statisticRecordsToCreate = new List<StatisticRecord>();

        foreach(var statisticRecord in statisticRecords)
        {
            var newStatistic = _mapper.Map<StatisticRecord>(statisticRecord);
            newStatistic.StreetcodeCoordinate = streetcode.Coordinates.FirstOrDefault(
              x => x.Latitude == newStatistic.StreetcodeCoordinate.Latitude && x.Longtitude == newStatistic.StreetcodeCoordinate.Longtitude);
            statisticRecordsToCreate.Add(newStatistic);
        }

        streetcode.StatisticRecords.AddRange(statisticRecordsToCreate);
    }

    private void AddAudio(StreetcodeContent streetcode, int? audioId)
    {
        streetcode.AudioId = audioId;
    }

    private async Task AddImagesAsync(StreetcodeContent streetcode, IEnumerable<int> imagesIds)
    {
        await _repositoryWrapper.StreetcodeImageRepository.CreateRangeAsync(imagesIds.Select(imageId => new StreetcodeImage()
        {
            ImageId = imageId,
            StreetcodeId = streetcode.Id,
        }));
    }

    private async Task AddTags(StreetcodeContent streetcode, List<StreetcodeTagDTO> tags)
    {
        var indexedTags = new List<StreetcodeTagIndex>();

        for (int i = 0; i < tags.Count; i++)
        {
            var newTagIndex = new StreetcodeTagIndex
            {
                StreetcodeId = streetcode.Id,
                TagId = tags[i].Id,
                IsVisible = tags[i].IsVisible,
                Index = i,
            };

            if (tags[i].Id <= 0)
            {
                var newTag = _mapper.Map<Tag>(tags[i]);
                newTag.Id = 0;
                newTagIndex.Tag = newTag;
            }

            indexedTags.Add(newTagIndex);
        }

        await _repositoryWrapper.StreetcodeTagIndexRepository.CreateRangeAsync(indexedTags);
    }

    private async Task AddRelatedFigures(StreetcodeContent streetcode, IEnumerable<RelatedFigureShortDTO> relatedFigures)
    {
        var relatedFiguresToCreate = relatedFigures
            .Select(relatedFigure => new DAL.Entities.Streetcode.RelatedFigure
            {
                ObserverId = streetcode.Id,
                TargetId = relatedFigure.Id,
            })
            .ToList();

        await _repositoryWrapper.RelatedFigureRepository.CreateRangeAsync(relatedFiguresToCreate);
    }

    private void AddArts(StreetcodeContent streetcode, IEnumerable<StreetcodeArtCreateUpdateDTO> arts)
    {
        streetcode.StreetcodeArts.AddRange(_mapper.Map<IEnumerable<StreetcodeArt>>(arts));
    }

    private async Task AddTimelineItems(StreetcodeContent streetcode, IEnumerable<TimelineItemCreateUpdateDTO> timelineItems)
    {
        var newContexts = timelineItems.SelectMany(x => x.HistoricalContexts).Where(c => c.Id == 0).DistinctBy(x => x.Title);
        var newContextsDb = _mapper.Map<IEnumerable<HistoricalContext>>(newContexts);
        await _repositoryWrapper.HistoricalContextRepository.CreateRangeAsync(newContextsDb);
        await _repositoryWrapper.SaveChangesAsync();
        List<TimelineItem> newTimelines = new List<TimelineItem>();

        foreach (var timelineItem in timelineItems)
        {
           var newTimeline = _mapper.Map<TimelineItem>(timelineItem);
           newTimeline.HistoricalContextTimelines = timelineItem.HistoricalContexts
              .Select(x => new HistoricalContextTimeline
              {
                  HistoricalContextId = x.Id == 0
                      ? newContextsDb.FirstOrDefault(h => h.Title.Equals(x.Title)).Id
                      : x.Id
              })
              .ToList();

           newTimelines.Add(newTimeline);
        }

        streetcode.TimelineItems.AddRange(newTimelines);
    }

    private async Task AddPartners(StreetcodeContent streetcode, IEnumerable<PartnerShortDTO> partners)
    {
        var partnersToCreate = partners.Select(partner => new StreetcodePartner
            {
                StreetcodeId = streetcode.Id,
                PartnerId = partner.Id
            })
          .ToList();
        await _repositoryWrapper.PartnerStreetcodeRepository.CreateRangeAsync(partnersToCreate);
    }

    private async Task AddToponyms(StreetcodeContent streetcode, IEnumerable<StreetcodeToponymUpdateDTO> toponyms)
    {
        var toponymsName = toponyms.Select(x => x.StreetName);
        streetcode.Toponyms.AddRange(await _repositoryWrapper.ToponymRepository.GetAllAsync(x => toponymsName.Contains(x.StreetName)));
    }
}
