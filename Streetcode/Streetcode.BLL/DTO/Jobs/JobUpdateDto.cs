using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Jobs
{
    public class JobUpdateDto : CreateUpdateJobDto
    {
        public int Id { get; set; }
    }
}
