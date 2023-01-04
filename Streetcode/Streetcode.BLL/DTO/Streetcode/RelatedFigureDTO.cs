using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Streetcode;

public class RelatedFigureDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public ImageDTO? Image { get; set; }
    public IEnumerable<TagDTO> Tags { get; set; }
}
