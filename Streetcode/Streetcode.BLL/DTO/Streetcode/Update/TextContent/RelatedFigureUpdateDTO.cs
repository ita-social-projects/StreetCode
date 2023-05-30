using Streetcode.BLL.DTO.Streetcode.Update.Interface;

namespace Streetcode.BLL.DTO.Streetcode.Update.TextContent
{
    public class RelatedFigureUpdateDTO : RelatedFigureDTO, IChanged
    {
        public bool? Changed { get; set; }
        public int ObserverId { get; set; }
        public int TargetId { get; set; }
	}
}
