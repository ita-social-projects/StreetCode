using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.AdditionalContent;

public class TagDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<StreetcodeDTO> Streetcodes { get; set; }
}
