using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Streetcode.BLL.DTO.News;

public abstract class CreateUpdateNewsDTO
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(100, ErrorMessage = "Max Length is 100")]
    public string Title { get; set; } = null!;

    [Required(AllowEmptyStrings = false)]
    [StringLength(15000, ErrorMessage = "Max Length is 15000")]
    public string Text { get; set; } = null!;
    public int ImageId { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(200, ErrorMessage = "Max Length is 200")]
    public string URL { get; set; } = null!;
    public DateTime CreationDate { get; set; }
}