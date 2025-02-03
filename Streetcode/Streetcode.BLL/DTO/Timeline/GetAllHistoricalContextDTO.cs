namespace Streetcode.BLL.DTO.Timeline
{
    public class GetAllHistoricalContextDTO
    {
        public int TotalAmount { get; set; }
        public IEnumerable<HistoricalContextDTO> HistoricalContexts { get; set; } = new List<HistoricalContextDTO>();
    }
}
