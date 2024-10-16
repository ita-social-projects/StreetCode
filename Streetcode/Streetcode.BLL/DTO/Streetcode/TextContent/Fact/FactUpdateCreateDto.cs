namespace Streetcode.BLL.DTO.Streetcode.TextContent.Fact
{
    public abstract class FactUpdateCreateDto
    {
        public string Title { get; set; }
        public int ImageId { get; set; }
        public string FactContent { get; set; }
        public int Index { get; set; }
        public string? ImageDescription { get; set; }
    }
}