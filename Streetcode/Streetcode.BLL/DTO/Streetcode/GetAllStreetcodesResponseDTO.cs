namespace Streetcode.BLL.DTO.Streetcode;

public class GetAllStreetcodesResponseDto
{
    public int TotalAmount { get; set; }
    public IEnumerable<StreetcodeDto> Streetcodes { get; set; } = new List<StreetcodeDto>();
}
