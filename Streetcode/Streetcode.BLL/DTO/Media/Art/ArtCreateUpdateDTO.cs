using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Media.Create
{
    public class ArtCreateUpdateDto : IModelState
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public string Description { get; set; } = null!;
        public string Title { get; set; } = null!;
        public ModelState ModelState { get; set; }
    }
}
