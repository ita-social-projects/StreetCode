using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Media.Art;

public class ArtDTO : IModelState
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public int ImageId { get; set; }
    public ImageDTO Image { get; set; }
    public ModelState ModelState { get; set; }
    public bool IsPersisted { get; set; }
}
