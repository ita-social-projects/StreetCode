using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Streetcode.Update.TextContent
{
    public class FactUpdateDTO : FactDTO, IModelState
  {
        public ModelState ModelState { get; set; }
    }
}
