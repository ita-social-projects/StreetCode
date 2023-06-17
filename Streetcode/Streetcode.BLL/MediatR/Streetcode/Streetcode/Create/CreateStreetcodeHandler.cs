using AutoMapper;
using FluentResults;
using MailKit.Net.Imap;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.Factories.Streetcode;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
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
                AddTimelineItems(streetcode, request.Streetcode.TimelineItems);
                AddAudio(streetcode, request.Streetcode.AudioId);
                AddArts(streetcode, request.Streetcode.StreetcodeArts);
                await AddTags(streetcode, request.Streetcode.Tags.ToList());
                await AddRelatedFigures(streetcode, request.Streetcode.RelatedFigures);
                await AddPartners(streetcode, request.Streetcode.Partners);
                /*await AddToponyms(streetcode, request.Streetcode.Toponyms);*/
                AddImages(streetcode, request.Streetcode.Images);
                AddStatisticRecords(streetcode, request.Streetcode.StatisticRecords);
                AddTransactionLink(streetcode, request.Streetcode.ARBlockURL);

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

    private void AddTransactionLink(StreetcodeContent streetcode, string? url)
    {
        if(url != null)
        {
            streetcode.TransactionLink = new DAL.Entities.Transactions.TransactionLink()
            {
                QrCodeUrl = url,
                QrCodeUrlTitle = url,
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

    private void AddImages(StreetcodeContent streetcode, IEnumerable<ImageDTO> images)
    {
        streetcode.Images.AddRange(_mapper.Map<IEnumerable<Image>>(images));
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

    private void AddTimelineItems(StreetcodeContent streetcode, IEnumerable<TimelineItemUpdateDTO> timelineItems)
    {
        streetcode.TimelineItems.AddRange(_mapper.Map<IEnumerable<TimelineItem>>(timelineItems));
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

    private async Task AddToponyms(StreetcodeContent streetcode, IEnumerable<string> toponymsName)
    {
        streetcode.Toponyms.AddRange(await _repositoryWrapper.ToponymRepository.GetAllAsync(x => toponymsName.Contains(x.StreetName)));
    }
}
