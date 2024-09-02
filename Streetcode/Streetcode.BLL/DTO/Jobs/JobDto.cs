using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Jobs
{
    public class JobDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool Status { get; set; }
        public string Description { get; set; } = null!;
        public string Salary { get; set; } = null!;
    }
}