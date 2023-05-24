using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update.Interfaces;

namespace Streetcode.BLL.DTO.Streetcode.Update.TextContent
{
    public class RelatedFigureUpdateDTO : RelatedFigureDTO, IDeleted
    {
        public bool IsDeleted { get; set; }
    }
}
