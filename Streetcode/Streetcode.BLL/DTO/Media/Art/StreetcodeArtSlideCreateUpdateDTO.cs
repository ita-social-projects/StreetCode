using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Media.Art
{
    public class StreetcodeArtSlideCreateUpdateDto : IModelState
    {
        public int SlideId { get; set; }
        public int Index { get; set; }
        public IEnumerable<StreetcodeArtCreateUpdateDto> StreetcodeArts { get; set; } = new List<StreetcodeArtCreateUpdateDto>();
        public int? StreetcodeId { get; set; }
        public StreetcodeArtSlideTemplate Template { get; set; }
        public ModelState ModelState { get; set; }
    }
}
