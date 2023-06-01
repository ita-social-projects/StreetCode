using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Realizations.Base;

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

		private async Task<StreetcodeUpdateDTO> GetOld(int id)
		{
			var updatedStreetcode = await _repositoryWrapper.StreetcodeRepository.GetFirstOrDefaultAsync(s => s.Id == id, include:
				x => x.Include(s => s.Text)
				.Include(s => s.Subtitles)
				.Include(s => s.TransactionLink));

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
