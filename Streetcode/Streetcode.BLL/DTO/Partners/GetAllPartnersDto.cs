namespace Streetcode.BLL.DTO.Partners;

public class GetAllPartnersDto
{
    public int TotalAmount { get; set; }

    public IEnumerable<PartnerDto> Partners { get; set; } = new List<PartnerDto>();
}
