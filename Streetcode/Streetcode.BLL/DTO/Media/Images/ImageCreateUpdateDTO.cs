using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Media.Images
{
    public class ImageCreateUpdateDTO : IModelState
    {
        public int Id { get; set; }
        public int? StreetcodeId { get; set; }
        public ModelState ModelState { get; set; }
    }
}
