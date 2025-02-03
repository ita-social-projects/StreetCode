namespace Streetcode.BLL.DTO.Toponyms;

public class GetAllToponymsResponseDto
{
    public int Pages { get; set; }
    public IEnumerable<ToponymDto> Toponyms { get; set; } = new List<ToponymDto>();
}