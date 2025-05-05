using Streetcode.BLL.DTO.Streetcode.TextContent.Term;
namespace Streetcode.BLL.DTO.Streetcode.TextContent
{
    public class GetAllTermsResponseDto
    {
        public int TotalAmount { get; set; }
        public IEnumerable<TermDto> Terms { get; set; } = new List<TermDto>();
    }
}
