using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Factories.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Util.Helpers;
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
    private readonly ILoggerService _logger;
    private readonly IStringLocalizer<FailedToCreateSharedResource> _stringLocalizerFailedToCreate;
    private readonly IStringLocalizer<AnErrorOccurredSharedResource> _stringLocalizerAnErrorOccurred;
    private readonly IStringLocalizer<FailedToValidateSharedResource> _stringLocalizerFailedToValidate;
    private readonly IStringLocalizer<FieldNamesSharedResource> _stringLocalizerFieldNames;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateStreetcodeHandler(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper,
        ILoggerService logger,
        IStringLocalizer<AnErrorOccurredSharedResource> stringLocalizerAnErrorOccurred,
        IStringLocalizer<FailedToCreateSharedResource> stringLocalizerFailedToCreate,
        IStringLocalizer<FailedToValidateSharedResource> stringLocalizerFailedToValidate,
        IStringLocalizer<FieldNamesSharedResource> stringLocalizerFieldNames,
        IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _logger = logger;
        _stringLocalizerAnErrorOccurred = stringLocalizerAnErrorOccurred;
        _stringLocalizerFailedToCreate = stringLocalizerFailedToCreate;
        _stringLocalizerFailedToValidate = stringLocalizerFailedToValidate;
        _stringLocalizerFieldNames = stringLocalizerFieldNames;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<int>> Handle(CreateStreetcodeCommand request, CancellationToken cancellationToken)
    {
        using (var transactionScope = _repositoryWrapper.BeginTransaction())
        {
            try
            {
                var streetcode = StreetcodeFactory.CreateStreetcode(request.Streetcode.StreetcodeType);
                _mapper.Map(request.Streetcode, streetcode);
                streetcode.CreatedAt = streetcode.UpdatedAt = DateTime.UtcNow;
                streetcode.UserId = HttpContextHelper.GetCurrentUserId(_httpContextAccessor) !;
                await _repositoryWrapper.StreetcodeRepository.CreateAsync(streetcode);
                var isResultSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
                await AddTimelineItems(streetcode, request.Streetcode.TimelineItems);
                await AddImagesAsync(streetcode, request.Streetcode.ImagesIds);
                AddAudio(streetcode, request.Streetcode.AudioId);
                await AddArtGallery(streetcode, request.Streetcode.StreetcodeArtSlides, request.Streetcode.Arts);
                await AddTags(streetcode, request.Streetcode.Tags);
                await AddRelatedFigures(streetcode, request.Streetcode.RelatedFigures);
                await AddPartners(streetcode, request.Streetcode.Partners);
                await AddToponyms(streetcode, request.Streetcode.Toponyms);
                AddStatisticRecords(streetcode, request.Streetcode.StatisticRecords);
                AddTransactionLink(streetcode, request.Streetcode.ArBlockUrl);

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

    private async Task AddImagesDetails(IEnumerable<ImageDetailsDto> imagesDetailsDtos)
    {
        if (imagesDetailsDtos.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(imagesDetailsDtos), _stringLocalizerFailedToValidate["CannotBeEmpty", _stringLocalizerFieldNames["ImagesDetails"]]);
        }

        var imageDetails = _mapper.Map<IEnumerable<ImageDetails>>(imagesDetailsDtos);
        await _repositoryWrapper.ImageDetailsRepository.CreateRangeAsync(imageDetails);
    }

    private async Task AddImagesAsync(StreetcodeContent streetcode, IEnumerable<int> imagesIds)
    {
        if (imagesIds == null)
        {
            throw new ArgumentNullException(nameof(imagesIds), _stringLocalizerFailedToValidate["CannotBeEmpty", _stringLocalizerFieldNames["Images"]]);
        }

        var imagesList = imagesIds.ToList();

        if (imagesList.Count == 0)
        {
            throw new ArgumentException(_stringLocalizerFailedToValidate["CannotBeEmpty", _stringLocalizerFieldNames["Images"]], nameof(imagesIds));
        }

        var streetcodeImages = imagesList
            .Select(imageId => new StreetcodeImage()
            {
                ImageId = imageId,
                StreetcodeId = streetcode.Id,
            })
            .ToList();

        await _repositoryWrapper.StreetcodeImageRepository.CreateRangeAsync(streetcodeImages);
    }

    private Task AddFactImageDescription(IEnumerable<FactUpdateCreateDto>? facts)
    {
        var factsList = facts?.ToList();

        if (factsList is null or { Count: 0 })
        {
            return Task.CompletedTask;
        }

        foreach (var fact in factsList.Where(fact => !string.IsNullOrEmpty(fact.ImageDescription)))
        {
            _repositoryWrapper.ImageDetailsRepository.Create(new ImageDetails()
            {
                Alt = fact.ImageDescription,
                ImageId = fact.ImageId
            });
        }

        return Task.CompletedTask;
    }

    private static void AddTransactionLink(StreetcodeContent streetcode, string? url)
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

    private void AddStatisticRecords(StreetcodeContent streetcode, IEnumerable<StatisticRecordDTO>? statisticRecords)
    {
        var statisticRecordsList = statisticRecords?.ToList();

        if (statisticRecordsList is null or { Count: 0 })
        {
            return;
        }

        var statisticRecordsToCreate = new List<StatisticRecord>();

        foreach (var statisticRecord in statisticRecordsList)
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

    private static void AddAudio(StreetcodeContent streetcode, int? audioId)
    {
        streetcode.AudioId = audioId;
    }

    private async Task AddTags(StreetcodeContent streetcode, IEnumerable<StreetcodeTagDTO>? tags)
    {
        var tagsList = tags?.ToList();

        if (tagsList is null or { Count: 0 })
        {
            return;
        }

        var indexedTags = new List<StreetcodeTagIndex>();

        for (int i = 0; i < tagsList.Count; i++)
        {
            var newTagIndex = new StreetcodeTagIndex
            {
                StreetcodeId = streetcode.Id,
                TagId = tagsList[i].Id,
                IsVisible = tagsList[i].IsVisible,
                Index = i,
            };

            if (tagsList[i].Id <= 0)
            {
                var exists = await _repositoryWrapper.TagRepository.GetFirstOrDefaultAsync(t => tagsList[i].Title == t.Title);
                if (exists is not null)
                {
                    throw new HttpRequestException(_stringLocalizerFailedToValidate["MustBeUnique", _stringLocalizerFieldNames["Tag"]], null, System.Net.HttpStatusCode.BadRequest);
                }

                var newTag = _mapper.Map<Tag>(tagsList[i]);
                newTag.Id = 0;
                newTagIndex.Tag = newTag;
            }

            indexedTags.Add(newTagIndex);
        }

        await _repositoryWrapper.StreetcodeTagIndexRepository.CreateRangeAsync(indexedTags);
    }

    private async Task AddRelatedFigures(StreetcodeContent streetcode, IEnumerable<RelatedFigureShortDTO>? relatedFigures)
    {
        var relatedFiguresList = relatedFigures?.ToList();

        if (relatedFiguresList is null or { Count: 0 })
        {
            return;
        }

        var relatedFiguresToCreate = relatedFiguresList
            .Select(relatedFigure => new DAL.Entities.Streetcode.RelatedFigure
            {
                ObserverId = streetcode.Id,
                TargetId = relatedFigure.Id,
            })
            .ToList();

        await _repositoryWrapper.RelatedFigureRepository.CreateRangeAsync(relatedFiguresToCreate);
    }

    private async Task AddArtGallery(StreetcodeContent streetcode, IEnumerable<StreetcodeArtSlideCreateUpdateDTO>? artSlides, IEnumerable<ArtCreateUpdateDTO>? arts)
    {
        var artSlidesList = artSlides?.ToList() ?? new List<StreetcodeArtSlideCreateUpdateDTO>();
        var artsList = arts?.ToList() ?? new List<ArtCreateUpdateDTO>();

        if (artSlidesList.Count == 0 && artsList.Count == 0)
        {
            return;
        }

        var usedArtIds = new HashSet<int>(artSlidesList
            .SelectMany(slide => slide.StreetcodeArts)
            .Select(streetcodeArt => streetcodeArt.ArtId));

        var filteredArts = artsList
            .Where(art => usedArtIds.Contains(art.Id))
            .ToList();

        var newArtSlides = _mapper.Map<List<StreetcodeArtSlide>>(artSlides);
        newArtSlides.ForEach(artSlide => artSlide.StreetcodeId = streetcode.Id);

        await _repositoryWrapper.StreetcodeArtSlideRepository.CreateRangeAsync(newArtSlides);
        await _repositoryWrapper.SaveChangesAsync();

        var newArts = _mapper.Map<List<Art>>(filteredArts);
        newArts.ForEach(art => art.StreetcodeId = streetcode.Id);

        await _repositoryWrapper.ArtRepository.CreateRangeAsync(newArts);
        await _repositoryWrapper.SaveChangesAsync();

        var artIdMap = filteredArts
            .Zip(newArts, (placeholderArt, newArt) => new { PlaceholderId = placeholderArt.Id, RealId = newArt.Id })
            .ToDictionary(x => x.PlaceholderId, x => x.RealId);

        var newStreetcodeArts = new List<StreetcodeArt>();

        foreach (var artSlide in artSlidesList)
        {
            var slideId = newArtSlides[artSlidesList.IndexOf(artSlide)].Id;
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

    private async Task AddTimelineItems(StreetcodeContent streetcode, IEnumerable<TimelineItemCreateUpdateDTO>? timelineItems)
    {
        var timelineItemCreateUpdateDtos = timelineItems?.ToList();

        if (timelineItemCreateUpdateDtos is null or { Count: 0 })
        {
            return;
        }

        var newContexts = timelineItemCreateUpdateDtos.SelectMany(x => x.HistoricalContexts).Where(c => c.Id == 0).DistinctBy(x => x.Title).ToList();
        var newContextsDb = _mapper.Map<List<HistoricalContext>>(newContexts);
        await _repositoryWrapper.HistoricalContextRepository.CreateRangeAsync(newContextsDb);
        await _repositoryWrapper.SaveChangesAsync();
        List<TimelineItem> newTimelines = new List<TimelineItem>();

        foreach (var timelineItem in timelineItemCreateUpdateDtos)
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

    private async Task AddPartners(StreetcodeContent streetcode, IEnumerable<int>? partners)
    {
        var partnersList = partners?.ToList();

        if (partnersList is null or { Count: 0 })
        {
            return;
        }

        var partnersToCreate = partnersList.Select(partnerId => new StreetcodePartner
        {
            StreetcodeId = streetcode.Id,
            PartnerId = partnerId
        })
          .ToList();
        await _repositoryWrapper.PartnerStreetcodeRepository.CreateRangeAsync(partnersToCreate);
    }

    private async Task AddToponyms(StreetcodeContent streetcode, IEnumerable<StreetcodeToponymCreateUpdateDTO>? toponyms)
    {
        var toponymList = toponyms?.ToList();

        if (toponymList is null or { Count: 0 })
        {
            return;
        }

        var toponymsName = toponymList.Select(x => x.StreetName);
        streetcode.Toponyms.AddRange(await _repositoryWrapper.ToponymRepository.GetAllAsync(x => toponymsName.Contains(x.StreetName)));
    }
}
