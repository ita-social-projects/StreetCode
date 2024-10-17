namespace Streetcode.BLL.DTO.Streetcode;

public class GetAllStreetcodesResponseDTO
{
    public int TotalAmount { get; set; }
    public IEnumerable<StreetcodeDTO> Streetcodes { get; set; } = new List<StreetcodeDTO>();
}
