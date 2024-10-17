using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Jobs
{
    public class GetAllJobsDTO
    {
        public int TotalAmount { get; set; }
        public IEnumerable<JobDto> Jobs { get; set; } = new List<JobDto>();
    }
}
