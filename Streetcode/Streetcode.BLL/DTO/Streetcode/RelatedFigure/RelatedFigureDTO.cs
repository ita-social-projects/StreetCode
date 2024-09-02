using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Streetcode.RelatedFigure;

public class RelatedFigureDTO
{
  public int Id { get; set; }
  public string Title { get; set; } = null!;
  public string Url { get; set; } = null!;
  public string Alias { get; set; } = null!;
  public int ImageId { get; set; }
  public IEnumerable<TagDTO> Tags { get; set; } = new List<TagDTO>();
}
