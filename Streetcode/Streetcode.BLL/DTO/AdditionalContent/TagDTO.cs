using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.AdditionalContent;

public class TagDTO
{
    public int Id;
    public string Title;
    public IEnumerable<StreetcodeDTO> Streetcodes;
}