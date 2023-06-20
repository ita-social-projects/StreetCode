using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics.Update;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Partners.Update;
using Streetcode.BLL.DTO.Sources.Update;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
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
            var streetcodeToUpdate = StreetcodeFactory.CreateStreetcode(request.Streetcode.StreetcodeType);
            _mapper.Map(request.Streetcode, streetcodeToUpdate);

            await UpdateStreetcodeArtsAsync(request.Streetcode.StreetcodeArts);
            await UpdateStreetcodeToponymAsync(streetcodeToUpdate, request.Streetcode.Toponyms);
            await UpdateStatisticRecordsAsync(request.Streetcode.StatisticRecords);
            await UpdateCategoryContentsAsync(request.Streetcode.StreetcodeCategoryContents);
            await UpdateTimelineItemsAsync(streetcodeToUpdate, request.Streetcode.TimelineItems);
            UpdateAudio(request.Streetcode.Audios, streetcodeToUpdate);
            await UpdateFactsAsync(request.Streetcode.Facts);
            await UpdateRelatedFiguresRelationAsync(request.Streetcode.RelatedFigures);
            await UpdatePartnersRelationAsync(request.Streetcode.Partners);
            await UpdateImagesAsync(request.Streetcode.Images);
            await UpdateStreetcodeTagsAsync(request.Streetcode.Tags);
            _repositoryWrapper.StreetcodeRepository.Update(streetcodeToUpdate);

            await _repositoryWrapper.SaveChangesAsync();

            return 1;
        }

        private async Task UpdateStreetcodeArtsAsync(IEnumerable<StreetcodeArtCreateUpdateDTO> arts)
        {
            await UpdateEntitiesAsync(arts, _repositoryWrapper.StreetcodeArtRepository);
        }

        private async Task UpdateTimelineItemsAsync(StreetcodeContent streetcode, IEnumerable<TimelineItemCreateUpdateDTO> timelineItems)
        {
            var (toUpdate, toCreate, toDelete) = CategorizeItems<TimelineItemCreateUpdateDTO>(timelineItems);

            var timelineItemsUpdated = new List<TimelineItem>();
            foreach(var timelineItem in toUpdate)
            {
                var timelineItemToUpdate = _mapper.Map<TimelineItem>(timelineItem);
                timelineItemToUpdate.HistoricalContextTimelines = null;
                timelineItemsUpdated.Add(timelineItemToUpdate);
                await UpdateEntitiesAsync(timelineItem.HistoricalContexts, _repositoryWrapper.HistoricalContextTimelineRepository);
            }

            streetcode.TimelineItems.AddRange(timelineItemsUpdated);
            streetcode.TimelineItems.AddRange(_mapper.Map<IEnumerable<TimelineItem>>(toCreate));
            _repositoryWrapper.TimelineRepository.DeleteRange(_mapper.Map<IEnumerable<TimelineItem>>(toDelete));
        }

        private async Task UpdateStreetcodeToponymAsync(StreetcodeContent streetcodeContent, IEnumerable<StreetcodeToponymCreateUpdateDTO> toponymsUpdateDTOs)
        {
            var (_, toCreate, toDelete) = CategorizeItems(toponymsUpdateDTOs);
            var toponymsNameToDelete = toDelete.Select(x => x.StreetName);

            var toponymsToDelete = (await _repositoryWrapper.StreetcodeToponymRepository
                .GetAllAsync(
                predicate: x => x.StreetcodeId == streetcodeContent.Id,
                include: i => i.Include(x => x.Toponym)))
                .Where(x => toponymsNameToDelete.Contains(x.Toponym.StreetName));

            _repositoryWrapper.StreetcodeToponymRepository.DeleteRange(toponymsToDelete);

            var toponymsName = toCreate.Select(x => x.StreetName);
            var toponymsToAdd = await _repositoryWrapper.ToponymRepository
                    .GetAllAsync(predicate: t => toponymsName.Contains(t.StreetName));

            streetcodeContent.Toponyms.AddRange(toponymsToAdd);
        }

        private async Task UpdateStatisticRecordsAsync(IEnumerable<StatisticRecordUpdateDTO> records)
        {
            await UpdateEntitiesAsync(records, _repositoryWrapper.StreetcodeCoordinateRepository);
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

        private async Task UpdateFactsAsync(IEnumerable<FactUpdateDTO> facts)
        {
            await UpdateEntitiesAsync(facts, _repositoryWrapper.FactRepository);
        }

        private async Task UpdateStreetcodeTagsAsync(IEnumerable<StreetcodeTagUpdateDTO> tags)
        {
            await UpdateEntitiesAsync(tags, _repositoryWrapper.StreetcodeTagIndexRepository);
        }

        private async Task UpdateImagesAsync(IEnumerable<StreetcodeImageUpdateDTO> images)
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
            var create = _mapper.Map<IEnumerable<T>>(toCreate);
            var delete = _mapper.Map<IEnumerable<T>>(toDelete);
            var update = _mapper.Map<IEnumerable<T>>(toUpdate);
            await repository.CreateRangeAsync(create);
            repository.DeleteRange(delete);
            repository.UpdateRange(update);
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
