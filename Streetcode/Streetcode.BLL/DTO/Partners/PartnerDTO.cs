using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Partners;

public class PartnerDTO
{
    public IdentifierDTO Identifier;
    public ImageDTO Image;
    public string Description;
    public UrlDTO TargetUrl;
}