using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.BLL.Enums;
using Streetcode.BLL.Factories.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Enums;
using Streetcode.BLL.Interfaces.Cache;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.DAL.Entities.Transactions;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Update
{
    public class UpdateStreetcodeHandler : IRequestHandler<UpdateStreetcodeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ILoggerService _logger;
        private readonly IStringLocalizer<FailedToUpdateSharedResource> _stringLocalizerFailedToUpdate;
        private readonly IStringLocalizer<AnErrorOccurredSharedResource> _stringLocalizerAnErrorOccurred;
        private readonly ICacheService _cacheService;
        public UpdateStreetcodeHandler(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper,
            ILoggerService logger,
            IStringLocalizer<AnErrorOccurredSharedResource> stringLocalizerAnErrorOccurred,
            IStringLocalizer<FailedToUpdateSharedResource> stringLocalizerFailedToUpdate,
            ICacheService cacheService)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _logger = logger;
            _stringLocalizerAnErrorOccurred = stringLocalizerAnErrorOccurred;
            _cacheService = cacheService;
        }

        public async Task<Result<int>> Handle(UpdateStreetcodeCommand request, CancellationToken cancellationToken)
        {
            using (var transactionScope = _repositoryWrapper.BeginTransaction())
            {
                try
                {
                    var streetcodeToUpdate = StreetcodeFactory.CreateStreetcode(request.Streetcode.StreetcodeType);
                    _mapper.Map(request.Streetcode, streetcodeToUpdate);

                    await UpdateEntitiesAsync(request.Streetcode.StatisticRecords, _repositoryWrapper.StreetcodeCoordinateRepository);
                    await UpdateEntitiesAsync(request.Streetcode.StreetcodeCategoryContents, _repositoryWrapper.StreetcodeCategoryContentRepository);
                    await UpdateEntitiesAsync(request.Streetcode.RelatedFigures, _repositoryWrapper.RelatedFigureRepository);
                    await UpdateEntitiesAsync(request.Streetcode.Partners, _repositoryWrapper.PartnerStreetcodeRepository);
                    await UpdateEntitiesAsync(request.Streetcode.Facts, _repositoryWrapper.FactRepository);
                    await UpdateEntitiesAsync(request.Streetcode.Tags, _repositoryWrapper.StreetcodeTagIndexRepository);
                    await UpdateStreetcodeToponymAsync(streetcodeToUpdate, request.Streetcode.Toponyms);
                    await UpdateTimelineItemsAsync(streetcodeToUpdate, request.Streetcode.TimelineItems);
                    UpdateAudio(request.Streetcode.Audios, streetcodeToUpdate);
                    await UpdateArtGallery(streetcodeToUpdate, request.Streetcode.StreetcodeArtSlides, request.Streetcode.Arts);
                    await UpdateImagesAsync(request.Streetcode.Images);

                    await UpdateFactsDescription(request.Streetcode.ImagesDetails);
                    var deleteTransactionLinks = _repositoryWrapper.TransactLinksRepository.FindAll(t => t.StreetcodeId == streetcodeToUpdate.Id);
                    _repositoryWrapper.TransactLinksRepository.DeleteRange(deleteTransactionLinks);
                    if (request.Streetcode.Text != null)
                    {
                        await UpdateEntitiesAsync(new List<TextUpdateDTO> { request.Streetcode.Text }, _repositoryWrapper.TextRepository);
                    }

                    streetcodeToUpdate.Arts = new List<Art>();
                    streetcodeToUpdate.StreetcodeArtSlides = new List<StreetcodeArtSlide>();
                    _repositoryWrapper.StreetcodeRepository.Update(streetcodeToUpdate);

                    _repositoryWrapper.StreetcodeRepository.Entry(streetcodeToUpdate).Property(x => x.CreatedAt).IsModified = false;
                    var discriminatorProperty = _repositoryWrapper.StreetcodeRepository.Entry(streetcodeToUpdate).Property<string>(StreetcodeTypeDiscriminators.DiscriminatorName);
                    discriminatorProperty.CurrentValue = StreetcodeTypeDiscriminators.GetStreetcodeType(request.Streetcode.StreetcodeType);
                    discriminatorProperty.IsModified = true;
                    streetcodeToUpdate.UpdatedAt = DateTime.UtcNow;
                    var isResultSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

                    if (isResultSuccess)
                    {
                        transactionScope.Complete();
                        _cacheService.RemoveStreetcodeCaches(streetcodeToUpdate.Id);
                        return Result.Ok(streetcodeToUpdate.Id);
                    }
                    else
                    {
                        string errorMsg = _stringLocalizerFailedToUpdate["FailedToUpdateStreetcode"].Value;
                        _logger.LogError(request, errorMsg);
                        return Result.Fail(new Error(errorMsg));
                    }
                }
                catch (Exception)
                {
                    string errorMsg = _stringLocalizerAnErrorOccurred["AnErrorOccurredWhileUpdatin"].Value;
                    _logger.LogError(request, errorMsg);
                    return Result.Fail(new Error(errorMsg));
                }
            }
        }

        private async Task UpdateFactsDescription(IEnumerable<ImageDetailsDto>? imageDetails)
        {
            if (imageDetails == null)
            {
                return;
            }

            _repositoryWrapper.ImageDetailsRepository
                .DeleteRange(_mapper.Map<IEnumerable<ImageDetails>>(imageDetails.Where(f => f.Alt == "" && f.Id != 0)));

            _repositoryWrapper.ImageDetailsRepository
                .UpdateRange(_mapper.Map<IEnumerable<ImageDetails>>(imageDetails.Where(f => f.Alt != "")));

            await _repositoryWrapper.SaveChangesAsync();
        }

        private async Task UpdateTimelineItemsAsync(StreetcodeContent streetcode, IEnumerable<TimelineItemCreateUpdateDTO> timelineItems)
        {
            var contextToCreate = timelineItems.SelectMany(x => x.HistoricalContexts).Where(c => c.Id == 0).DistinctBy(x => x.Title);
            var createdContext = _mapper.Map<IEnumerable<HistoricalContext>>(contextToCreate);
            await _repositoryWrapper.HistoricalContextRepository.CreateRangeAsync(createdContext);
            await _repositoryWrapper.SaveChangesAsync();

            var (toUpdate, toCreate, toDelete) = CategorizeItems<TimelineItemCreateUpdateDTO>(timelineItems);

            var timelineItemsUpdated = new List<TimelineItem>();
            foreach (var timelineItem in toUpdate)
            {
                timelineItemsUpdated.Add(_mapper.Map<TimelineItem>(timelineItem));
                var (historicalContextToUpdate, historicalContextToCreate, historicalContextToDelete) = CategorizeItems<HistoricalContextCreateUpdateDTO>(timelineItem.HistoricalContexts);

                var deletedItems = historicalContextToDelete.Select(x => new HistoricalContextTimeline
                {
                    TimelineId = timelineItem.Id,
                    HistoricalContextId = x.Id,
                })
                .ToList();

                var createdItems = historicalContextToCreate.Select(x => new HistoricalContextTimeline
                {
                    TimelineId = timelineItem.Id,
                    HistoricalContextId = x.Id == 0
                        ? createdContext.FirstOrDefault(h => h.Title.Equals(x.Title)).Id
                        : x.Id
                })
                .ToList();

                _repositoryWrapper.HistoricalContextTimelineRepository.DeleteRange(deletedItems);
                await _repositoryWrapper.HistoricalContextTimelineRepository.CreateRangeAsync(createdItems);
            }

            streetcode.TimelineItems.AddRange(timelineItemsUpdated);

            var timelineItemsCreated = new List<TimelineItem>();
            foreach (var timelineItem in toCreate)
            {
                var timelineItemCreate = _mapper.Map<TimelineItem>(timelineItem);
                timelineItemCreate.HistoricalContextTimelines = timelineItem.HistoricalContexts
                  .Select(x => new HistoricalContextTimeline
                  {
                      HistoricalContextId = x.Id == 0
                          ? createdContext.FirstOrDefault(h => h.Title.Equals(x.Title)).Id
                          : x.Id
                  })
                 .ToList();

                timelineItemsCreated.Add(timelineItemCreate);
            }

            streetcode.TimelineItems.AddRange(timelineItemsCreated);

            _repositoryWrapper.TimelineRepository.DeleteRange(_mapper.Map<List<TimelineItem>>(toDelete));
        }

        private async Task UpdateStreetcodeToponymAsync(StreetcodeContent streetcodeContent, IEnumerable<StreetcodeToponymUpdateDTO> toponyms)
        {
            var (_, toCreate, toDelete) = CategorizeItems(toponyms);

            if (toDelete.Any())
            {
                var toponymsNameToDelete = toDelete.Select(x => x.StreetName);
                await _repositoryWrapper.ToponymRepository.ExecuteSqlRaw(GetToponymDeleteQuery(streetcodeContent.Id, toponymsNameToDelete));
            }

            var toponymsNameToCreate = toCreate.Select(x => x.StreetName);
            var toponymsToAdd = await _repositoryWrapper.ToponymRepository
                    .GetAllAsync(predicate: t => toponymsNameToCreate.Contains(t.StreetName));

            streetcodeContent.Toponyms.AddRange(toponymsToAdd);
        }

        private string GetToponymDeleteQuery(int streetcodeId, IEnumerable<string> toponymsName)
        {
            string query = "DELETE st FROM streetcode.streetcode_toponym AS st " +
                           "LEFT JOIN toponyms.toponyms AS t ON st.ToponymId = t.Id " +
                           $"WHERE st.StreetcodeId = {streetcodeId} AND (";

            string condition = string.Join(" OR ", toponymsName.Select(name => $"t.StreetName LIKE '%{name}%'"));
            query += condition + ")";

            return query;
        }

        private async Task UpdateImagesAsync(IEnumerable<ImageUpdateDTO> images)
        {
            var (_, toCreate, toDelete) = CategorizeItems(images);

            _repositoryWrapper.ImageRepository.DeleteRange(_mapper.Map<IEnumerable<Image>>(toDelete));
            await _repositoryWrapper.StreetcodeImageRepository.CreateRangeAsync(_mapper.Map<IEnumerable<StreetcodeImage>>(toCreate));
        }

        private void UpdateAudio(IEnumerable<AudioUpdateDTO> audios, StreetcodeContent streetcode)
        {
            var (toUpdate, toCreate, _) = CategorizeItems(audios);

            if (toCreate?.Any() == true)
            {
                streetcode.AudioId = toCreate.First().Id;
            }

            if (toUpdate?.Any() == true)
            {
                streetcode.AudioId = toUpdate.First().Id;
            }
        }

        private async Task UpdateArtGallery(StreetcodeContent streetcode, IEnumerable<StreetcodeArtSlideCreateUpdateDTO> artSlides, IEnumerable<ArtCreateUpdateDTO> arts)
        {
            _repositoryWrapper.StreetcodeArtRepository.DeleteRange(await _repositoryWrapper.StreetcodeArtRepository.GetAllAsync(a => a.StreetcodeId == streetcode.Id));

            var toCreateArts = new List<Art>();
            var toDeleteArts = new List<Art>();
            var toUpdateArts = new List<Art>();
            var oldArtIds = new List<int>();
            foreach (var art in arts)
            {
                var newArt = _mapper.Map<Art>(art);
                newArt.StreetcodeId = streetcode.Id;

                if (art.ModelState == ModelState.Created)
                {
                    oldArtIds.Add(art.Id);
                    toCreateArts.Add(newArt);
                }
                else
                {
                    newArt.Id = art.Id;
                    if (art.ModelState == ModelState.Deleted)
                    {
                        toDeleteArts.Add(newArt);
                    }
                    else
                    {
                        toUpdateArts.Add(newArt);
                    }
                }
            }

            await _repositoryWrapper.ArtRepository.CreateRangeAsync(toCreateArts);
            _repositoryWrapper.ArtRepository.DeleteRange(toDeleteArts);
            _repositoryWrapper.ArtRepository.UpdateRange(toUpdateArts);
            await _repositoryWrapper.SaveChangesAsync();

            var toCreateSlides = new List<StreetcodeArtSlide>();
            var toDeleteSlides = new List<StreetcodeArtSlide>();
            var toUpdateSlides = new List<StreetcodeArtSlide>();

            var toCreateStreetcodeArts = new List<StreetcodeArt>();
            foreach (var artSlide in artSlides)
            {
                var newArtSlide = _mapper.Map<StreetcodeArtSlide>(artSlide);
                newArtSlide.StreetcodeId = streetcode.Id;
                newArtSlide.StreetcodeArts = null;

                var newStreetcodeArts =
                    GetStreetcodeArtsWithNewArtsId(streetcode.Id, oldArtIds, artSlide, toCreateArts);

                DistributeArtSlide(artSlide, newArtSlide, newStreetcodeArts, ref toCreateSlides, ref toUpdateSlides, ref toDeleteSlides, ref toCreateStreetcodeArts);
            }

            toCreateStreetcodeArts = CreateStreetcodeHandler.CreateStreetcodeArtsForUnusedArts(streetcode.Id, toCreateArts.Concat(toUpdateArts).ToList(), toCreateStreetcodeArts);

            await _repositoryWrapper.StreetcodeArtSlideRepository.CreateRangeAsync(toCreateSlides);
            _repositoryWrapper.StreetcodeArtSlideRepository.DeleteRange(toDeleteSlides);
            _repositoryWrapper.StreetcodeArtSlideRepository.UpdateRange(toUpdateSlides);
            await _repositoryWrapper.StreetcodeArtRepository.CreateRangeAsync(toCreateStreetcodeArts);
            await _repositoryWrapper.SaveChangesAsync();
        }

        private async Task UpdateEntitiesAsync<T, U>(IEnumerable<U> updates, IRepositoryBase<T> repository)
            where T : class
            where U : IModelState
        {
            var (toUpdate, toCreate, toDelete) = CategorizeItems<U>(updates);

            await repository.CreateRangeAsync(_mapper.Map<IEnumerable<T>>(toCreate));
            repository.DeleteRange(_mapper.Map<IEnumerable<T>>(toDelete));
            repository.UpdateRange(_mapper.Map<IEnumerable<T>>(toUpdate));
        }

        private (IEnumerable<T> toUpdate, IEnumerable<T> toCreate, IEnumerable<T> toDelete) CategorizeItems<T>(IEnumerable<T> items)
              where T : IModelState
        {
            var toUpdate = new List<T>();
            var toCreate = new List<T>();
            var toDelete = new List<T>();

            foreach (var item in items)
            {
                switch (item.ModelState)
                {
                    case Enums.ModelState.Updated:
                        toUpdate.Add(item);
                        break;
                    case Enums.ModelState.Created:
                        toCreate.Add(item);
                        break;
                    default:
                        toDelete.Add(item);
                        break;
                }
            }

            return (toUpdate, toCreate, toDelete);
        }

        private List<StreetcodeArt> GetStreetcodeArtsWithNewArtsId(int streetcodeId, List<int> oldArtIds, StreetcodeArtSlideCreateUpdateDTO streetcodeArtSlide, List<Art> toCreateArts)
        {
            var newStreetcodeArts = new List<StreetcodeArt>();
            foreach (var art in streetcodeArtSlide.StreetcodeArts)
            {
                var newArt = _mapper.Map<StreetcodeArt>(art);
                newArt.StreetcodeId = streetcodeId;

                if (oldArtIds.Contains(art.ArtId))
                {
                    newArt.ArtId = toCreateArts[oldArtIds.IndexOf(art.ArtId)].Id;
                }
                else
                {
                    newArt.ArtId = art.ArtId;
                }

                newStreetcodeArts.Add(newArt);
            }

            return newStreetcodeArts;
        }

        private void DistributeArtSlide(StreetcodeArtSlideCreateUpdateDTO artSlideDto, StreetcodeArtSlide artSlide, List<StreetcodeArt> newStreetcodeArts, ref List<StreetcodeArtSlide> toCreateSlides, ref List<StreetcodeArtSlide> toUpdateSlides, ref List<StreetcodeArtSlide> toDeleteSlides, ref List<StreetcodeArt> toCreateStreetcodeArts)
        {
            if (artSlideDto.ModelState == ModelState.Created)
            {
                toCreateSlides.Add(artSlide);
            }
            else
            {
                artSlide.Id = artSlideDto.SlideId;
                if (artSlideDto.ModelState == ModelState.Deleted)
                {
                    toDeleteSlides.Add(artSlide);
                }
                else if (artSlideDto.ModelState == ModelState.Updated)
                {
                    toUpdateSlides.Add(artSlide);
                    foreach (var streetcodeArt in newStreetcodeArts)
                    {
                        streetcodeArt.StreetcodeArtSlideId = artSlideDto.SlideId;
                        toCreateStreetcodeArts.Add(streetcodeArt);
                    }
                }
            }
        }
    }
}
