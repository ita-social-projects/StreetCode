namespace Streetcode.BLL.DTO.Streetcode
{
    public class StreetcodeFilterResultDTO
    {
        public int StreetcodeId { get; set; }
        public string StreetcodeTransliterationUrl { get; set; } = null!;
        public int StreetcodeIndex { get; set; }
        public string? BlockName { get; set; }
        public string Content { get; set; } = null!;
        public string? SourceName { get; set; }
        public int FactId { get; set; } = 0;
        public int TimelineItemId { get; set; } = 0;
    }
}
