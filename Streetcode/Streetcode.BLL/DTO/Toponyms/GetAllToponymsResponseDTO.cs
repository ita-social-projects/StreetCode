namespace Streetcode.BLL.DTO.Toponyms;

public class GetAllToponymsResponseDTO
{
    public int Pages { get; set; }
    public IEnumerable<ToponymDTO> Toponyms { get; set; }
}