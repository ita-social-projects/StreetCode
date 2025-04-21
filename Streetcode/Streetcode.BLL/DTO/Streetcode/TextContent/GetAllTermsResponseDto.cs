namespace Streetcode.BLL.DTO.Streetcode.TextContent
{
    public class GetAllTermsResponseDto
    {
        public int TotalAmount { get; set; }
        public IEnumerable<TermDTO> Terms { get; set; } = new List<TermDTO>();
    }
}
