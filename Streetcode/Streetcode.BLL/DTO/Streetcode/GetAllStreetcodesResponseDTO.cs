namespace Streetcode.BLL.DTO.Streetcode;

public class GetAllStreetcodesResponseDTO
{
    public int Pages { get; set; }
    public IEnumerable<StreetcodeDTO> Streetcodes { get; set; }
}
