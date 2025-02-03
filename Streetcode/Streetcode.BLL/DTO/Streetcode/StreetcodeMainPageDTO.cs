namespace Streetcode.BLL.DTO.Streetcode
{
    public class StreetcodeMainPageDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Alias { get; set; }
        public string? Teaser { get; set; }
        public string? Text { get; set; }
        public int ImageId { get; set; }
        public string TransliterationUrl { get; set; } = null!;
    }
}
