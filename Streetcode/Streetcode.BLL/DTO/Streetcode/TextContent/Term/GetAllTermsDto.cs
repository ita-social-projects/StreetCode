namespace Streetcode.BLL.DTO.Streetcode.TextContent.Term;

public class GetAllTermsDto
{
    public int TotalAmount { get; set; }

    public IEnumerable<TermDto> Terms { get; set; } = new List<TermDto>();
}
