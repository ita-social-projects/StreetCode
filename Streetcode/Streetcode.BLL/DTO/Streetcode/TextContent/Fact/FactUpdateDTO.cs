using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Streetcode.TextContent.Fact
{
  public class FactUpdateDto : FactDto, IModelState
  {
    public ModelState ModelState { get; set; }
    public int StreetcodeId { get; set; }
    public string ImageDescription { get; set; }
  }
}
