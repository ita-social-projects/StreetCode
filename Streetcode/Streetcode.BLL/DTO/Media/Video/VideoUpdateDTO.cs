namespace Streetcode.BLL.DTO.Media.Video
{
    public class VideoUpdateDTO : VideoCreateUpdateDTO
    {
        public int Id { get; set; }
        public int StreetcodeId { get; set; }
    }
}
