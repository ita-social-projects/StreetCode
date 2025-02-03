namespace Streetcode.BLL.DTO.Sources
{
    public abstract class SourceLinkCreateUpdateCategoryDto
    {
        public string Title { get; set; } = null!;
        public int ImageId { get; set; }
    }
}
