using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Factories.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
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
    public const int StreetcodeIndexMaxValue = 9999;
    public const int StreetcodeIndexMinValue = 1;
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailedToCreate;
    private readonly IStringLocalizer<AnErrorOccurredSharedResource> _stringLocalizerAnErrorOccurred;

    public CreateStreetcodeHandler(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper,
        ILoggerService logger,
        IStringLocalizer<AnErrorOccurredSharedResource> stringLocalizerAnErrorOccurred,
        IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailedToCreate)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerAnErrorOccurred = stringLocalizerAnErrorOccurred;
        _stringLocalizerFailedToCreate = stringLocalizerFailedToCreate;
    }

    public async Task<Result<int>> Handle(CreateStreetcodeCommand request, CancellationToken cancellationToken)
    {
        using (var transactionScope = _repositoryWrapper.BeginTransaction())
        {
            try
            {
                var streetcode = StreetcodeFactory.CreateStreetcode(request.Streetcode.StreetcodeType);
                _mapper.Map(request.Streetcode, streetcode);
                if (streetcode.Index < StreetcodeIndexMinValue || streetcode.Index > StreetcodeIndexMaxValue)
                {
                    string errorMessage = $"The 'index' must be in range from {StreetcodeIndexMinValue} to {StreetcodeIndexMaxValue}.";
                    _logger.LogError(request, errorMessage);
                    return Result.Fail(new Error(errorMessage));
                }

                streetcode.CreatedAt = streetcode.UpdatedAt = DateTime.UtcNow;
                _repositoryWrapper.StreetcodeRepository.Create(streetcode);
                var isResultSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
                await AddTimelineItems(streetcode, request.Streetcode.TimelineItems);
                await AddImagesAsync(streetcode, request.Streetcode.ImagesIds);
                AddAudio(streetcode, request.Streetcode.AudioId);
                await AddArtGallery(streetcode, request.Streetcode.StreetcodeArtSlides.ToList(), request.Streetcode.Arts);
                await AddTags(streetcode, request.Streetcode.Tags.ToList());
                await AddRelatedFigures(streetcode, request.Streetcode.RelatedFigures);
                await AddPartners(streetcode, request.Streetcode.Partners);
                await AddToponyms(streetcode, request.Streetcode.Toponyms);
                AddStatisticRecords(streetcode, request.Streetcode.StatisticRecords);
                AddTransactionLink(streetcode, request.Streetcode.ARBlockURL);

                if (string.IsNullOrWhiteSpace(request.Streetcode.Text?.Title) && !string.IsNullOrWhiteSpace(request.Streetcode.Text?.TextContent))
                {
                    string errorMsg = "The 'title' key for the text is empty or missing.";
                    _logger.LogError(request, errorMsg);
                    return Result.Fail(new Error(errorMsg));
                }

                if (string.IsNullOrWhiteSpace(request.Streetcode.Text?.Title)
                     && request.Streetcode.Videos != null
                     && !string.IsNullOrWhiteSpace(request.Streetcode.Videos.FirstOrDefault()?.Url))
                {
                    string errorMsg = "The 'title' key for the video is empty or missing.";
                    _logger.LogError(request, errorMsg);
                    return Result.Fail(new Error(errorMsg));
                }

                await _repositoryWrapper.SaveChangesAsync();
                await AddFactImageDescription(request.Streetcode.Facts);
                await AddImagesDetails(request.Streetcode.ImagesDetails);
                await _repositoryWrapper.SaveChangesAsync();

                if (isResultSuccess)
                {
                    transactionScope.Complete();
                    return Result.Ok(streetcode.Id);
                }
                else
                {
                    string errorMsg = _stringLocalizerFailedToCreate["FailedToCreateStreetcode"].Value;
                    _logger.LogError(request, errorMsg);
                    return Result.Fail(new Error(errorMsg));
                }
            }
            catch (Exception ex)
            {
                string errorMsg = _stringLocalizerAnErrorOccurred["AnErrorOccurredWhileCreating", ex.Message].Value;
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }

    public static List<StreetcodeArt> CreateStreetcodeArtsForUnusedArts(int streetcodeId, List<Art> arts, List<StreetcodeArt> streetcodeArts)
    {
        foreach (var art in arts)
        {
            var newStreetcodeArt = new StreetcodeArt()
            {
                Index = 0,
                StreetcodeId = streetcodeId,
                ArtId = art.Id,
                StreetcodeArtSlideId = null,
            };

            if (streetcodeArts.Count == 0)
            {
                streetcodeArts.Add(newStreetcodeArt);
            }
            else
            {
                for (int streetcodeArtsIndex = 0; streetcodeArtsIndex < streetcodeArts.Count; streetcodeArtsIndex++)
                {
                    if (art.Id == streetcodeArts[streetcodeArtsIndex].ArtId)
                    {
                        break;
                    }

                    if (streetcodeArtsIndex == streetcodeArts.Count - 1)
                    {
                        streetcodeArts.Add(newStreetcodeArt);
                    }
                }
            }
        }

        return streetcodeArts;
    }

    public async Task AddImagesDetails(IEnumerable<ImageDetailsDto> imageDetails)
    {
        if (imageDetails.IsNullOrEmpty())
        {
            throw new HttpRequestException("There is no valid imagesDetails value", null, System.Net.HttpStatusCode.BadRequest);
        }

        foreach (var detail in imageDetails)
        {
            if (string.IsNullOrEmpty(detail.Alt))
            {
                throw new HttpRequestException("There is no valid imagesDetails value", null, System.Net.HttpStatusCode.BadRequest);
            }
        }

        await _repositoryWrapper.ImageDetailsRepository.CreateRangeAsync(_mapper.Map<IEnumerable<ImageDetails>>(imageDetails));
    }

    public async Task AddImagesAsync(StreetcodeContent streetcode, IEnumerable<int> imagesIds)
    {
        if (imagesIds.IsNullOrEmpty())
        {
            throw new HttpRequestException("There is no valid imagesIds value", null, System.Net.HttpStatusCode.BadRequest);
        }

        await _repositoryWrapper.StreetcodeImageRepository.CreateRangeAsync(imagesIds.Select(imageId => new StreetcodeImage()
        {
            ImageId = imageId,
            StreetcodeId = streetcode.Id,
        }));
    }

    private Task AddFactImageDescription(IEnumerable<FactUpdateCreateDto> facts)
    {
        foreach (FactUpdateCreateDto fact in facts)
        {
            if (fact.ImageDescription != null)
            {
                _repositoryWrapper.ImageDetailsRepository.Create(new ImageDetails()
                {
                    Alt = fact.ImageDescription,
                    ImageId = fact.ImageId
                });
            }
        }

        return Task.CompletedTask;
    }

    private void AddTransactionLink(StreetcodeContent streetcode, string? url)
    {
        if (url != null)
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

        foreach (var statisticRecord in statisticRecords)
        {
            var newStatistic = _mapper.Map<StatisticRecord>(statisticRecord);
            var streetcodeCoordinate = streetcode.Coordinates.FirstOrDefault(x =>
                x.Latitude == newStatistic.StreetcodeCoordinate.Latitude
                && x.Longtitude == newStatistic.StreetcodeCoordinate.Longtitude);

            newStatistic.StreetcodeCoordinate = streetcodeCoordinate ?? throw new InvalidOperationException();
            statisticRecordsToCreate.Add(newStatistic);
        }

        streetcode.StatisticRecords.AddRange(statisticRecordsToCreate);
    }

    private void AddAudio(StreetcodeContent streetcode, int? audioId)
    {
        streetcode.AudioId = audioId;
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

    private async Task AddArtGallery(StreetcodeContent streetcode, List<StreetcodeArtSlideCreateUpdateDTO> artSlides, IEnumerable<ArtCreateUpdateDTO> arts)
    {
        var usedArtIds = new HashSet<int>(artSlides.SelectMany(slide => slide.StreetcodeArts).Select(streetcodeArt => streetcodeArt.ArtId));

        var filteredArts = arts.Where(art => usedArtIds.Contains(art.Id)).ToList();

        var newArtSlides = _mapper.Map<List<StreetcodeArtSlide>>(artSlides);
        newArtSlides.ForEach(artSlide => artSlide.StreetcodeId = streetcode.Id);

        await _repositoryWrapper.StreetcodeArtSlideRepository.CreateRangeAsync(newArtSlides);
        await _repositoryWrapper.SaveChangesAsync();

        var newArts = _mapper.Map<List<Art>>(filteredArts);
        newArts.ForEach(art => art.StreetcodeId = streetcode.Id);

        await _repositoryWrapper.ArtRepository.CreateRangeAsync(newArts);
        await _repositoryWrapper.SaveChangesAsync();

        var artIdMap = filteredArts.Zip(newArts, (placeholderArt, newArt) => new { PlaceholderId = placeholderArt.Id, RealId = newArt.Id })
                                   .ToDictionary(x => x.PlaceholderId, x => x.RealId);

        var newStreetcodeArts = new List<StreetcodeArt>();
        foreach (var artSlide in artSlides)
        {
            var slideId = newArtSlides[artSlides.IndexOf(artSlide)].Id;
            foreach (var streetcodeArt in artSlide.StreetcodeArts)
            {
                var newStreetcodeArt = _mapper.Map<StreetcodeArt>(streetcodeArt);
                newStreetcodeArt.StreetcodeId = streetcode.Id;
                if (artIdMap.TryGetValue(streetcodeArt.ArtId, out var newArtId))
                {
                    newStreetcodeArt.ArtId = newArtId;
                }
                else
                {
                    throw new KeyNotFoundException($"Art ID '{streetcodeArt.ArtId}' not found in the mapped arts.");
                }

                newStreetcodeArt.StreetcodeArtSlideId = slideId;
                newStreetcodeArts.Add(newStreetcodeArt);
            }
        }

        await _repositoryWrapper.StreetcodeArtRepository.CreateRangeAsync(newStreetcodeArts);
        await _repositoryWrapper.SaveChangesAsync();
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
                       ? newContextsDb.First(h => h.Title!.Equals(x.Title)).Id
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
