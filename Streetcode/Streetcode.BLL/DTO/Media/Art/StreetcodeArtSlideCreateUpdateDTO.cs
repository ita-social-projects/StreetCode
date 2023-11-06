using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Media.Art
{
    public class StreetcodeArtSlideCreateUpdateDTO : IModelState
    {
        public int SlideId { get; set; }
        public int Index { get; set; }
        public IEnumerable<StreetcodeArtCreateUpdateDTO> StreetcodeArts { get; set; }
        public int? StreetcodeId { get; set; }
        public StreetcodeArtSlideTemplate Template { get; set; }
        public ModelState ModelState { get; set; }
    }
}
