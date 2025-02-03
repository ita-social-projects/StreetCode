using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByFilter
{
    public class GetStreetcodeByFilterHandler : IRequestHandler<GetStreetcodeByFilterQuery, Result<List<StreetcodeFilterResultDto>>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetStreetcodeByFilterHandler(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Result<List<StreetcodeFilterResultDto>>> Handle(GetStreetcodeByFilterQuery request, CancellationToken cancellationToken)
        {
            var searchQuery = request.Filter.SearchQuery.Trim();
            var results = new List<StreetcodeFilterResultDto>();

            results.AddRange(await GetStreetcodeResultsAsync(searchQuery));
            results.AddRange(await GetTextResultsAsync(searchQuery));
            results.AddRange(await GetFactResultsAsync(searchQuery));
            results.AddRange(await GetTimelineResultsAsync(searchQuery));
            results.AddRange(await GetArtGalleryResultsAsync(searchQuery));

            return results;
        }

        private async Task<IEnumerable<StreetcodeFilterResultDto>> GetStreetcodeResultsAsync(string searchQuery)
        {
            var streetcodes = await _repositoryWrapper.StreetcodeRepository.GetAllAsync(
                x => x.Status == DAL.Enums.StreetcodeStatus.Published &&
                    (x.Title!.Contains(searchQuery) || (x.Alias != null && x.Alias.Contains(searchQuery)) || x.Teaser!.Contains(searchQuery)));

            return streetcodes.Select(streetcode => CreateFilterResult(
                streetcode,
                streetcode.Title!.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ? streetcode.Title :
                streetcode.Alias?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) == true ? streetcode.Alias :
                streetcode.Teaser!.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ? streetcode.Teaser :
                streetcode.TransliterationUrl!));
        }

        private async Task<IEnumerable<StreetcodeFilterResultDto>> GetTextResultsAsync(string searchQuery)
        {
            var texts = await _repositoryWrapper.TextRepository.GetAllAsync(
                x => x.Streetcode!.Status == DAL.Enums.StreetcodeStatus.Published,
                i => i.Include(x => x.Streetcode!));

            return texts.Where(text => text.Title!.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                (!string.IsNullOrEmpty(text.TextContent) && text.TextContent.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)))
                .Select(text => CreateFilterResult(text.Streetcode!, text.Title!.Contains(searchQuery) ? text.Title! : text.TextContent!, "Текст", "text"));
        }

        private async Task<IEnumerable<StreetcodeFilterResultDto>> GetFactResultsAsync(string searchQuery)
        {
            var facts = await _repositoryWrapper.FactRepository.GetAllAsync(
                x => x.Streetcode!.Status == DAL.Enums.StreetcodeStatus.Published,
                i => i.Include(x => x.Streetcode!));

            return facts.Where(fact => fact.Title!.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                fact.FactContent!.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                .Select(fact => CreateFilterResult(fact.Streetcode!, fact.Title!, "Wow-факти", "wow-facts", factId: fact.Id));
        }

        private async Task<IEnumerable<StreetcodeFilterResultDto>> GetTimelineResultsAsync(string searchQuery)
        {
            var timelineItems = await _repositoryWrapper.TimelineRepository.GetAllAsync(
                x => x.Streetcode!.Status == DAL.Enums.StreetcodeStatus.Published,
                i => i.Include(x => x.Streetcode!));

            return timelineItems.Where(timelineItem => timelineItem.Title!.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                (!string.IsNullOrEmpty(timelineItem.Description) && timelineItem.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)))
                .Select(timelineItem => CreateFilterResult(timelineItem.Streetcode!, timelineItem.Title!, "Хронологія", "timeline", timelineItemId: timelineItem.Id));
        }

        private async Task<IEnumerable<StreetcodeFilterResultDto>> GetArtGalleryResultsAsync(string searchQuery)
        {
            var streetcodeArts = await _repositoryWrapper.StreetcodeArtRepository.GetAllAsync(
                x => x.StreetcodeArtSlide!.Streetcode != null && x.StreetcodeArtSlide.Streetcode.Status == DAL.Enums.StreetcodeStatus.Published,
                i => i.Include(sArt => sArt.Art).Include(sArt => sArt.StreetcodeArtSlide).ThenInclude(slide => slide!.Streetcode!));

            return streetcodeArts.Where(streetcodeArt => !string.IsNullOrEmpty(streetcodeArt.Art!.Description) &&
                streetcodeArt.Art.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                .SelectMany(streetcodeArt => streetcodeArt.StreetcodeArtSlide!.StreetcodeArts!.Select(art =>
                    CreateFilterResult(art.StreetcodeArtSlide!.Streetcode!, streetcodeArt.Art!.Description!, "Арт-галерея", "art-gallery")));
        }

        private StreetcodeFilterResultDto CreateFilterResult(StreetcodeContent streetcode, string content, string? sourceName = null, string? blockName = null, int factId = 0, int timelineItemId = 0)
        {
            return new StreetcodeFilterResultDto
            {
                StreetcodeId = streetcode.Id,
                StreetcodeTransliterationUrl = streetcode.TransliterationUrl!,
                StreetcodeIndex = streetcode.Index,
                BlockName = blockName!,
                Content = content,
                SourceName = sourceName!,
                FactId = factId,
                TimelineItemId = timelineItemId
            };
        }
    }
}
