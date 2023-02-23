using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Streetcode;

public abstract class StreetcodeDTO
{
    public int Id { get; set; }
    public int Index { get; set; }
    public Stage Stage { get; set; }
    public string StreetcodeType { get; set; }
    public DateTime EventStartOrPersonBirthDate { get; set; }
    public DateTime EventEndOrPersonDeathDate { get; set; }
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<TagDTO> Tags { get; set; }
    public string Teaser { get; set; }
}