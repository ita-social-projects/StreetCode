using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.DTO.Streetcode.Update.Interface;

namespace Streetcode.BLL.DTO.Streetcode.Update.TextContent
{
    public class FactUpdateDTO : FactDTO, IChanged
    {
        public bool? Changed { get; set; }
    }
}
