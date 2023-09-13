using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Jobs
{
	public class JobShortDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public bool Status { get; set; }
		public string Salary { get; set; }
	}
}
