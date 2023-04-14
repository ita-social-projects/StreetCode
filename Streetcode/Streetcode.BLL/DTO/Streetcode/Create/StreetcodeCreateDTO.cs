using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Streetcode.Create
{
  public class StreetcodeCreateDTO
  {
    public int Index { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string Title { get; set; }
    public string DateString { get; set; }
    public string Alias { get; set; }
    public string TransliterationUrl { get; set; }
    public StreetcodeType Type { get; set; }
    public StreetcodeStatus Status { get; set; }
    public DateTime EventStartOrPersonBirthDate { get; set; }
    public DateTime EventEndOrPersonDeathDate { get; set; }
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<TagCreateDTO> Tags { get; set; }
    public string Teaser { get; set; }

    public IEnumerable<PartnerShortDTO> Partners { get; set; }
    public string SubTitle { get; set; }
  }
}
