using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update.Interfaces;

namespace Streetcode.BLL.DTO.Streetcode.Update.TextContent
{
    public class RelatedFigureUpdateDTO /*: IDeleted*/
    {
        /*public bool IsDeleted { get; set; }*/
        public int ObserverId { get; set; }
        public int TargetId { get; set; }
    }
}
