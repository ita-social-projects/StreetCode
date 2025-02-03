namespace Streetcode.BLL.DTO.Media.Video
{
    public class VideoUpdateDto : VideoCreateUpdateDto
    {
        public int Id { get; set; }
        public int StreetcodeId { get; set; }
    }
}
