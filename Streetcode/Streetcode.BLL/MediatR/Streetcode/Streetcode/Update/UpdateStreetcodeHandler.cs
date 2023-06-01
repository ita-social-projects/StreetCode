using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.DTO.Streetcode.Update.TextContent;
using Streetcode.BLL.DTO.Streetcode.Update.Toponyms;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Repositories.Interfaces.Base;
using RelatedFigureModel = Streetcode.DAL.Entities.Streetcode.RelatedFigure;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Update
{
    internal class UpdateStreetcodeHandler : IRequestHandler<UpdateStreetcodeCommand, Result<StreetcodeUpdateDTO>>
	{
		private readonly IMapper _mapper;
		private readonly IRepositoryWrapper _repositoryWrapper;

		public UpdateStreetcodeHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
		{
			_mapper = mapper;
			_repositoryWrapper = repositoryWrapper;
		}

		public async Task<Result<StreetcodeUpdateDTO>> Handle(UpdateStreetcodeCommand request, CancellationToken cancellationToken)
		{
            var streetcodeToUpdate = _mapper.Map<StreetcodeContent>(request.Streetcode);

            await UpdateTimelineItemsAsync(streetcodeToUpdate, request.Streetcode.TimelineItems);

            _repositoryWrapper.StreetcodeRepository.Update(streetcodeToUpdate);
            UpdateStreetcodeToponym(request.Streetcode.StreetcodeToponym);
            UpdateRelatedFiguresRelation(request.Streetcode.RelatedFigures);
            UpdatePartnersRelation(request.Streetcode.Partners);
            _repositoryWrapper.SaveChanges();

            return await GetOld(streetcodeToUpdate.Id);
		}

		public async Task UpdateTimelineItemsAsync(StreetcodeContent streetcode, IEnumerable<TimelineItemUpdateDTO> timelineItems)
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

		private void UpdateStreetcodeToponym(IEnumerable<StreetcodeToponymUpdateDTO> streetcodeToponymsDTO)
		{
            var toDelete = streetcodeToponymsDTO.Where(_ => _.ModelState == Enums.ModelState.Deleted);
            var toCreate = streetcodeToponymsDTO.Where(_ => _.ModelState == Enums.ModelState.Created);

            foreach (var streetcodeToponymToDelete in toDelete)
			{
                var streetcodeToponym = _mapper.Map<StreetcodeToponym>(streetcodeToponymToDelete);
                _repositoryWrapper.StreetcodeToponymRepository.Delete(streetcodeToponym);
			}

            foreach (var streetcodeToponymToCreate in toCreate)
			{
                var streetcodeToponym = _mapper.Map<StreetcodeToponym>(streetcodeToponymToCreate);
                _repositoryWrapper.StreetcodeToponymRepository.Create(streetcodeToponym);
			}
		}

		private void UpdateRelatedFiguresRelation(IEnumerable<RelatedFigureUpdateDTO> relatedFigureUpdates)
		{
            var relationsToCreate = relatedFigureUpdates.Where(_ => _.ModelState == Enums.ModelState.Created);
            var relationsToDelete = relatedFigureUpdates.Where(_ => _.ModelState == Enums.ModelState.Deleted);

            foreach (var relationToCreate in relationsToCreate)
            {
                var relation = _mapper.Map<RelatedFigureModel>(relationToCreate);
                _repositoryWrapper.RelatedFigureRepository.Create(relation);
            }

            foreach(var relationToDelete in relationsToDelete)
            {
                var relation = _mapper.Map<RelatedFigureModel>(relationToDelete);
                _repositoryWrapper.RelatedFigureRepository.Delete(relation);
            }
        }

		private void UpdatePartnersRelation(IEnumerable<PartnersUpdateDTO> partnersUpdateDTOs)
        {
            var relationsToCreate = partnersUpdateDTOs.Where(_ => _.ModelState == Enums.ModelState.Created);
            var relationsToDelete = partnersUpdateDTOs.Where(_ => _.ModelState == Enums.ModelState.Deleted);

            foreach (var relationToCreate in relationsToCreate)
            {
                var relation = _mapper.Map<StreetcodePartner>(relationToCreate);
                _repositoryWrapper.PartnerStreetcodeRepository.Create(relation);
            }

            foreach (var relationToDelete in relationsToDelete)
            {
                var relation = _mapper.Map<StreetcodePartner>(relationToDelete);
                _repositoryWrapper.PartnerStreetcodeRepository.Delete(relation);
            }
        }

		private async Task<StreetcodeUpdateDTO> GetOld(int id)
        {
            var updatedStreetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Id == id, include:
                x => x.Include(s => s.Text)
                .Include(s => s.Subtitles)
                .Include(s => s.TransactionLink)
                .Include(s => s.Toponyms));

            var updatedDTO = _mapper.Map<StreetcodeUpdateDTO>(updatedStreetcode);
            return updatedDTO;
		}

		private (IEnumerable<T> toUpdate, IEnumerable<T> toCreate, IEnumerable<T> toDelete) CategorizeItems<T>(IEnumerable<T> items)
              where T : IModelState
        {
            var toUpdate = new List<T>();
            var toCreate = new List<T>();
            var toDelete = new List<T>();

            foreach (var item in items)
            {
                if (item.ModelState == Enums.ModelState.Updated)
                {
                    toUpdate.Add(item);
                }
                else if (item.ModelState == Enums.ModelState.Created)
                {
                    toCreate.Add(item);
                }
                else
                {
                    toDelete.Add(item);
                }
            }

            return (toUpdate, toCreate, toDelete);
        }
    }
}
