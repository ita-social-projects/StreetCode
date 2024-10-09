using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Jobs
{
    public class JobShortDto
	{
		public int Id { get; set; }
		[Required(AllowEmptyStrings = false)]
		[StringLength(50, ErrorMessage = "Max Length is 50")]
		public string Title { get; set; } = null!;
		public bool Status { get; set; }
		[Required(AllowEmptyStrings = false)]
		[StringLength(15, ErrorMessage = "Max Length is 15")]
		public string Salary { get; set; } = null!;
    }
}
