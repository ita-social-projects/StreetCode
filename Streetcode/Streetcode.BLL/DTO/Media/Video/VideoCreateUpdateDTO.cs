namespace Streetcode.BLL.DTO.Media.Video;

public abstract class VideoCreateUpdateDto
{
    public string? Description { get; set; }
    public string Url { get; set; } = null!;
}