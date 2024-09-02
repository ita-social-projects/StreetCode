using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Media.Art;

public class ArtDTO : IModelState
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int ImageId { get; set; }
    public ImageDTO Image { get; set; } = null!;
    public ModelState ModelState { get; set; }
    public bool IsPersisted { get; set; }
}
