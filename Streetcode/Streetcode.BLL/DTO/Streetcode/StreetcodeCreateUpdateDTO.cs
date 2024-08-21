using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Streetcode;

public abstract class StreetcodeCreateUpdateDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Index { get; set; }
    public string? Teaser { get; set; }
    public string DateString { get; set; }
    public string? Alias { get; set; }
    public StreetcodeStatus Status { get; set; }
    public StreetcodeType StreetcodeType { get; set; }
    public string Title { get; set; }
    public string TransliterationUrl { get; set; }
    public string? EventStartOrPersonBirthDate { get; set; }
    public string? EventEndOrPersonDeathDate { get; set; }
}