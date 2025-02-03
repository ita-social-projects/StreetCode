namespace Streetcode.BLL.DTO.AdditionalContent.Coordinates;

public abstract class CoordinateDto
{
    public int Id { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longtitude { get; set; }
}