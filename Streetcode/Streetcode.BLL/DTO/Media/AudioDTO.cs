using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Media;

public class AudioDTO
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string BlobStorageName { get; set; }

    public int StreetcodeId { get; set; }
}