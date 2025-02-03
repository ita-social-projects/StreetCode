using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Timeline.Update
{
    public class TimelineItemCreateUpdateDto : IModelState
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public DateViewPattern DateViewPattern { get; set; }
        public ModelState ModelState { get; set; } = ModelState.Updated;
        public IEnumerable<HistoricalContextCreateUpdateDto> HistoricalContexts { get; set; } = new List<HistoricalContextCreateUpdateDto>();
    }
}
