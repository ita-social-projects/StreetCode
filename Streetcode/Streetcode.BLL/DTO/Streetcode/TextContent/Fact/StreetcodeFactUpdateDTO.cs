using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Streetcode.TextContent.Fact
{
  public class StreetcodeFactUpdateDTO : FactUpdateCreateDto, IModelState
  {
    public int Id { get; set; }
    public ModelState ModelState { get; set; }
    public int StreetcodeId { get; set; }
  }
}
