using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Streetcode.TextContent.Text
{
  public class TextUpdateDTO : TextDTO, IModelState
  {
      public ModelState ModelState { get; set; } = ModelState.Updated;
  }
}
