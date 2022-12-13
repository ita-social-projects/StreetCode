using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Partners;

public class PartnerDTO
{

    public int Id;

    public ImageDTO Image;

    public string Title;

    public string Description;

    public List<UrlDTO> Urls;

}