using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics.Update;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Partners.Update;
using Streetcode.BLL.DTO.Sources.Update;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Factories.Streetcode;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;
using RelatedFigureModel = Streetcode.DAL.Entities.Streetcode.RelatedFigure;

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

                    await UpdateTimelineItemsAsync(streetcodeToUpdate, request.Streetcode.TimelineItems);
                    await UpdateStreetcodeArtsAsync(streetcodeToUpdate, request.Streetcode.StreetcodeArts);
                    await UpdateRelatedFiguresRelationAsync(request.Streetcode.RelatedFigures);
                    await UpdatePartnersRelationAsync(request.Streetcode.Partners);
                    await UpdateStreetcodeTagsAsync(request.Streetcode.StreetcodeTags);
                    await UpdateStatisticRecordsAsync(streetcodeToUpdate, request.Streetcode.StatisticRecords);
                    await UpdateCategoryContentsAsync(request.Streetcode.StreetcodeCategoryContents);

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
                catch (Exception ex)
                {
                    // Logger
                    Console.WriteLine(ex.Message);
                    return Result.Fail(new Error("An error occurred while updating streetcode"));
                }
            }
		}

		private async Task UpdateStreetcodeArtsAsync(StreetcodeContent streetcode, IEnumerable<StreetcodeArtUpdateDTO> arts)
        {
            var (toUpdate, toCreate, toDelete) = CategorizeItems<StreetcodeArtUpdateDTO>(arts);

            var artsToCreate = new List<StreetcodeArt>();

            foreach(var art in toCreate)
            {
                var newArt = _mapper.Map<StreetcodeArt>(art);
                newArt.Art.Image = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(x => x.Id == art.Art.ImageId);
                newArt.Art.Image.Alt = art.Art.Title;
                artsToCreate.Add(newArt);
            }

            streetcode.StreetcodeArts.AddRange(artsToCreate);
            streetcode.StreetcodeArts.AddRange(_mapper.Map<List<StreetcodeArt>>(toUpdate));

            _repositoryWrapper.StreetcodeArtRepository.DeleteRange(_mapper.Map<List<StreetcodeArt>>(toDelete));
        }

		private async Task UpdateTimelineItemsAsync(StreetcodeContent streetcode, IEnumerable<TimelineItemUpdateDTO> timelineItems)
        {
            var newContexts = timelineItems.SelectMany(x => x.HistoricalContexts).Where(c => c.Id == 0).DistinctBy(x => x.Title);
            var newContextsDb = _mapper.Map<IEnumerable<HistoricalContext>>(newContexts);
            await _repositoryWrapper.HistoricalContextRepository.CreateRangeAsync(newContextsDb);
            await _repositoryWrapper.SaveChangesAsync();

            var (toUpdate, toCreate, toDelete) = CategorizeItems<TimelineItemUpdateDTO>(timelineItems);

            var timelineItemsUpdated = new List<TimelineItem>();
            foreach(var timelineItem in toUpdate)
            {
                timelineItemsUpdated.Add(_mapper.Map<TimelineItem>(timelineItem));
                var (historicalContextToUpdate, historicalContextToCreate, historicalContextToDelete) = CategorizeItems<HistoricalContextUpdateDTO>(timelineItem.HistoricalContexts);

                // Mapper?
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
                        ? newContextsDb.FirstOrDefault(x => x.Title.Equals(x.Title)).Id
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

                // and here
                timelineItemCreate.HistoricalContextTimelines = timelineItem.HistoricalContexts
                  .Select(x => new HistoricalContextTimeline
                  {
                      HistoricalContextId = x.Id == 0
                          ? newContextsDb.FirstOrDefault(x => x.Title.Equals(x.Title)).Id
                          : x.Id
                  })
                 .ToList();

                timelineItemsCreated.Add(timelineItemCreate);
            }

            streetcode.TimelineItems.AddRange(timelineItemsCreated);

            _repositoryWrapper.TimelineRepository.DeleteRange(_mapper.Map<List<TimelineItem>>(toDelete));
        }

		private async Task UpdateStreetcodeToponymAsync(IEnumerable<StreetcodeToponymUpdateDTO> toponyms)
        {
            await UpdateEntitiesAsync(toponyms, _repositoryWrapper.StreetcodeToponymRepository);
        }

		private async Task UpdateStatisticRecordsAsync(StreetcodeContent streetcode, IEnumerable<StatisticRecordUpdateDTO> records)
        {
           var (toUpdate, toCreate, toDelete) = CategorizeItems<StatisticRecordUpdateDTO>(records);

           foreach (var recordToCreate in toCreate)
           {
                var coordinate = _repositoryWrapper.StreetcodeCoordinateRepository
                    .Create(_mapper.Map<StreetcodeCoordinate>(recordToCreate.StreetcodeCoordinate));

                var record = _mapper.Map<StatisticRecord>(recordToCreate);

                record.StreetcodeId = streetcode.Id;
                record.StreetcodeCoordinateId = coordinate.Id;

                await _repositoryWrapper.StatisticRecordRepository.CreateAsync(record);
           }

           foreach (var recordToDelete in toDelete)
           {
                var record = _mapper.Map<StatisticRecord>(recordToDelete);
                _repositoryWrapper.StreetcodeCoordinateRepository.Delete(record.StreetcodeCoordinate);
                _repositoryWrapper.StatisticRecordRepository.Delete(record);
           }
        }

		private async Task UpdateCategoryContentsAsync(IEnumerable<StreetcodeCategoryContentUpdateDTO> categoryContents)
        {
            await UpdateEntitiesAsync(categoryContents, _repositoryWrapper.StreetcodeCategoryContentRepository);
        }

		private async Task UpdateRelatedFiguresRelationAsync(IEnumerable<RelatedFigureUpdateDTO> relatedFigures)
        {
            await UpdateEntitiesAsync(relatedFigures, _repositoryWrapper.RelatedFigureRepository);
        }

		private async Task UpdatePartnersRelationAsync(IEnumerable<PartnersUpdateDTO> partners)
        {
            await UpdateEntitiesAsync(partners, _repositoryWrapper.PartnerStreetcodeRepository);
        }

		private async Task UpdateEntitiesAsync<T, U>(IEnumerable<U> updates, IRepositoryBase<T> repository)
            where T : class
            where U : IModelState
        {
            var (toUpdate, toCreate, toDelete) = CategorizeItems<U>(updates);

            await repository.CreateRangeAsync(_mapper.Map<IEnumerable<T>>(toCreate));
            repository.DeleteRange(_mapper.Map<IEnumerable<T>>(toDelete));
        }

		private async Task UpdateStreetcodeTagsAsync(IEnumerable<StreetcodeTagUpdateDTO> streetcodeTagsUpdateDTOs)
        {
            var (toUpdate, toCreate, toDelete) = CategorizeItems<StreetcodeTagUpdateDTO>(streetcodeTagsUpdateDTOs);
            var delete = _mapper.Map<IEnumerable<StreetcodeTagIndex>>(toDelete);
            var update = _mapper.Map<IEnumerable<StreetcodeTagIndex>>(toUpdate);
            var create = _mapper.Map<IEnumerable<StreetcodeTagIndex>>(toCreate);

            _repositoryWrapper.StreetcodeTagIndexRepository.DeleteRange(delete);

            foreach (var streetcodeTag in update)
            {
                 _repositoryWrapper.StreetcodeTagIndexRepository.Update(streetcodeTag);
            }

            await _repositoryWrapper.StreetcodeTagIndexRepository.CreateRangeAsync(create);
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
