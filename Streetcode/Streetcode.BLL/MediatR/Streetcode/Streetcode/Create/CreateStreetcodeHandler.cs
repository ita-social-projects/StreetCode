using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Factories.Streetcode;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;

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
                AddAudio(streetcode, request.Streetcode.AudioId);
                await AddArts(streetcode, request.Streetcode.StreetcodeArts);
                await AddTagsToStreetcode(streetcode, request.Streetcode.Tags.ToList());
                await AddRelatedFigures(streetcode, request.Streetcode.RelatedFigures);
                await AddPartnersToStreetcode(streetcode, request.Streetcode.Partners);
                await AddToponyms(streetcode, request.Streetcode.Toponyms);
                await AddImages(streetcode, request.Streetcode.ImagesId);
                AddStatisticRecords(streetcode, request.Streetcode.StatisticRecords);
                await _repositoryWrapper.SaveChangesAsync();

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
                return Result.Fail(new Error("An error occurred while creating a streetcode"));
            }
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

    private async Task AddImages(StreetcodeContent streetcode, IEnumerable<int> imagesId)
    {
        streetcode.Images.AddRange(await _repositoryWrapper.ImageRepository.GetAllAsync(x => imagesId.Contains(x.Id)));
    }

    private async Task AddTagsToStreetcode(StreetcodeContent streetcode, List<StreetcodeTagDTO> tags)
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

    private async Task AddRelatedFigures(StreetcodeContent streetcode, IEnumerable<StreetcodeDTO> relatedFigures)
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

    private async Task AddArts(StreetcodeContent streetcode, IEnumerable<ArtCreateDTO> arts)
    {
        var artsToCreate = new List<StreetcodeArt>();

        foreach(var art in arts)
        {
            var newArt = _mapper.Map<StreetcodeArt>(art);
            newArt.Art.Image = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(x => x.Id == art.ImageId);
            newArt.Art.Image.Alt = art.Title;
            artsToCreate.Add(newArt);
        }

        streetcode.StreetcodeArts.AddRange(artsToCreate);
    }

    private async Task AddTimelineItems(StreetcodeContent streetcode, IEnumerable<TimelineItemDTO> timelineItems)
    {
        var newContexts = timelineItems.SelectMany(x => x.HistoricalContexts).Where(c => c.Id == 0).DistinctBy(x => x.Title);
        var newContextsDb = _mapper.Map<IEnumerable<HistoricalContext>>(newContexts);
        await _repositoryWrapper.HistoricalContextRepository.CreateRangeAsync(newContextsDb);
        await _repositoryWrapper.SaveChangesAsync();
        List<TimelineItem> newTimelines = new List<TimelineItem>();
        TimelineItem current;
        HistoricalContext currentHistoricalContext;
        foreach (TimelineItemDTO timelineItem in timelineItems)
        {
           current = _mapper.Map<TimelineItem>(timelineItem);
           current.HistoricalContexts.Clear();
           newTimelines.Add(current);
           foreach (HistoricalContextDTO historicalContext in timelineItem.HistoricalContexts)
           {
                if (historicalContext.Id == 0)
                {
                    currentHistoricalContext = newContextsDb.FirstOrDefault(x => x.Title.Equals(historicalContext.Title));
                    if(currentHistoricalContext != null)
                    {
                        current.HistoricalContexts.Add(currentHistoricalContext);
                    }
                }
                else
                {
                    current.HistoricalContexts.Add(_mapper.Map<HistoricalContext>(historicalContext));
                }
           }
        }

        streetcode.TimelineItems.AddRange(newTimelines);
    }

    private async Task AddPartnersToStreetcode(StreetcodeContent streetcode, IEnumerable<PartnerShortDTO> partners)
    {
        var partnerIds = partners.Select(p => p.Id);
        streetcode.Partners.AddRange(await _repositoryWrapper.PartnersRepository
            .GetAllAsync(p => partnerIds.Contains(p.Id)));
    }

    private async Task AddToponyms(StreetcodeContent streetcode, IEnumerable<string> toponymsName)
    {
        streetcode.Toponyms.AddRange(await _repositoryWrapper.ToponymRepository.GetAllAsync(x => toponymsName.Contains(x.StreetName)));
    }
}
