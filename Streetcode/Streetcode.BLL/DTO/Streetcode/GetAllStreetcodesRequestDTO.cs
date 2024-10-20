namespace Streetcode.BLL.DTO.Streetcode;

public class GetAllStreetcodesRequestDTO
{
    public int? Page { get; set; } = null;
    public int? Amount { get; set; } = null;
    public string? Title { get; set; } = null;
    public string? Sort { get; set; } = null;
    public string? Filter { get; set; } = null;
}
