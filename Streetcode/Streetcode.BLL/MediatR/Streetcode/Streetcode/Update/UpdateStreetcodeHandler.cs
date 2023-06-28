using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Factories.Streetcode;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Update
{
    public class UpdateStreetcodeHandler : IRequestHandler<UpdateStreetcodeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public UpdateStreetcodeHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Result<int>> Handle(UpdateStreetcodeCommand request, CancellationToken cancellationToken)
        {
            using(var transactionScope = _repositoryWrapper.BeginTransaction())
            {
                try
                {
                    var streetcodeToUpdate = StreetcodeFactory.CreateStreetcode(request.Streetcode.StreetcodeType);
                    _mapper.Map(request.Streetcode, streetcodeToUpdate);

                    await UpdateEntitiesAsync(request.Streetcode.StreetcodeArts, _repositoryWrapper.StreetcodeArtRepository);
                    await UpdateEntitiesAsync(request.Streetcode.StatisticRecords, _repositoryWrapper.StreetcodeCoordinateRepository);
                    await UpdateEntitiesAsync(request.Streetcode.StreetcodeCategoryContents, _repositoryWrapper.StreetcodeCategoryContentRepository);
                    await UpdateEntitiesAsync(request.Streetcode.RelatedFigures, _repositoryWrapper.RelatedFigureRepository);
                    await UpdateEntitiesAsync(request.Streetcode.Partners, _repositoryWrapper.PartnerStreetcodeRepository);
                    await UpdateEntitiesAsync(request.Streetcode.Facts, _repositoryWrapper.FactRepository);
                    await UpdateEntitiesAsync(request.Streetcode.Tags, _repositoryWrapper.StreetcodeTagIndexRepository);
                    await UpdateStreetcodeToponymAsync(streetcodeToUpdate, request.Streetcode.Toponyms);
                    await UpdateTimelineItemsAsync(streetcodeToUpdate, request.Streetcode.TimelineItems);
                    UpdateAudio(request.Streetcode.Audios, streetcodeToUpdate);
                    await UpdateImagesAsync(request.Streetcode.Images);

                    _repositoryWrapper.StreetcodeRepository.Update(streetcodeToUpdate);
                    var isResultSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

                    if (isResultSuccess)
                    {
                        transactionScope.Complete();
                        return Result.Ok(streetcodeToUpdate.Id);
                    }
                    else
                    {
                        return Result.Fail(new Error("Failed to update a streetcode"));
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Result.Fail(new Error("An error occurred while updating a streetcode"));
                }
            }
        }

        private async Task UpdateTimelineItemsAsync(StreetcodeContent streetcode, IEnumerable<TimelineItemCreateUpdateDTO> timelineItems)
        {
            var contextToCreate = timelineItems.SelectMany(x => x.HistoricalContexts).Where(c => c.Id == 0).DistinctBy(x => x.Title);
            var createdContext = _mapper.Map<IEnumerable<HistoricalContext>>(contextToCreate);
            await _repositoryWrapper.HistoricalContextRepository.CreateRangeAsync(createdContext);
            await _repositoryWrapper.SaveChangesAsync();

            var (toUpdate, toCreate, toDelete) = CategorizeItems<TimelineItemCreateUpdateDTO>(timelineItems);

            var timelineItemsUpdated = new List<TimelineItem>();
            foreach(var timelineItem in toUpdate)
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
            foreach(var timelineItem in toCreate)
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
            var (_, toCreate, toDelete) = CategorizeItems(audios);

            if (toDelete?.Any() == true)
            {
                streetcode.AudioId = null;
                _repositoryWrapper.AudioRepository.DeleteRange(_mapper.Map<IEnumerable<Audio>>(toDelete));
            }

            if (toCreate?.Any() == true)
            {
                streetcode.AudioId = toCreate.First().Id;
            }
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
    }
}
