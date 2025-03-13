namespace Streetcode.BLL.DTO.Event
{
    public class GetAllEventsResponseDTO
    {
        public int TotalAmount { get; set; }
        public IEnumerable<object> Events { get; set; } = new List<object>();
    }
}
