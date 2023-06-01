using Streetcode.BLL.DTO.Streetcode.Update.Interface;

namespace Streetcode.BLL.DTO.Streetcode.Update.TextContent
{
    public class RelatedFigureUpdateDTO : IChanged
    {
        public bool? IsChanged { get; set; }
        public int ObserverId { get; set; }
        public int TargetId { get; set; }
	}
}
