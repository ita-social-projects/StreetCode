using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Jobs
{
    public class JobUpdateDto
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Max Length is 50")]
        public string Title { get; set; } = null!;
        public bool Status { get; set; }
        [StringLength(3000, ErrorMessage = "Max Length is 3000")]
        public string Description { get; set; } = null!;
        [Required(AllowEmptyStrings = false)]
        [StringLength(15, ErrorMessage = "Max Length is 15")]
        public string Salary { get; set; } = null!;
    }
}
