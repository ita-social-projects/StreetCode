using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update.Interfaces;

namespace Streetcode.BLL.DTO.Streetcode.Update.TextContent
{
    public class HistoricalContextUpdateDTO : HistoricalContextDTO, IDeleted
    {
        public bool IsDeleted { get; set; } = false;
    }
}
