using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update.Interfaces;

namespace Streetcode.BLL.DTO.Streetcode.Update.TextContent
{
    public class FactUpdateDTO : FactDTO, IDeleted
    {
        public bool IsDeleted { get; set; } = false;
    }
}
