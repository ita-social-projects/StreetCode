using Streetcode.BLL.DTO.Streetcode.Update.Interface;
using Streetcode.BLL.DTO.Timeline;

namespace Streetcode.BLL.DTO.Streetcode.Update.TextContent
{
    public class TimelineItemUpdateDTO : TimelineItemDTO, IChanged
    {
        public bool? IsChanged { get; set; }
    }
}
