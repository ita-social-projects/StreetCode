namespace Streetcode.BLL.DTO.Streetcode;

public class GetAllStreetcodesShortDto
{
    public int TotalAmount { get; set; }

    public IEnumerable<StreetcodeShortDto> StreetcodesShort { get; set; } = new List<StreetcodeShortDto>();
}
