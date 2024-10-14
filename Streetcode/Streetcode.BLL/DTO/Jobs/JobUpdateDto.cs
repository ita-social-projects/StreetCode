using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.Jobs
{
    public class JobUpdateDto : CreateUpdateJobDto
    {
        public int Id { get; set; }
    }
}
