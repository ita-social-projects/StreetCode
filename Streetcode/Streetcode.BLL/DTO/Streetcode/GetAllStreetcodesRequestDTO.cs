namespace Streetcode.BLL.DTO.Streetcode;

public class GetAllStreetcodesRequestDTO
{
    public int Page { get; set; } = 1;
    public int Amount { get; set; } = 10;
    public string? Title { get; set; }
    public string? Sort { get; set; }
    public string? Filter { get; set; }
}
