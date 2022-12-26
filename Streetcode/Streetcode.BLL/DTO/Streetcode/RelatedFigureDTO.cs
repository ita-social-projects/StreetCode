namespace Streetcode.BLL.DTO.Streetcode;

public class RelatedFigureDTO
{
    public int ObserverId { get; set; }
    public int TargetId { get; set; }
    public StreetcodeDTO Observer { get; set; }
    public StreetcodeDTO Target { get; set; }
}