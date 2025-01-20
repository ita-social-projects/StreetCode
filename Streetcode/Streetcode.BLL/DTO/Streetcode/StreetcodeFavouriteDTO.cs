using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Streetcode
{
    public class StreetcodeFavouriteDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Alias { get; set; }
        public int ImageId { get; set; }
        public string TransliterationUrl { get; set; } = null!;
        public StreetcodeType Type { get; set; }
    }
}
