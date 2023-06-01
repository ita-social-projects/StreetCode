using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Streetcode.Update.TextContent
{
    public class RelatedFigureUpdateDTO : IModelState
    {
        public ModelState ModelState { get; set; }
        public int ObserverId { get; set; }
        public int TargetId { get; set; }
    }
}
