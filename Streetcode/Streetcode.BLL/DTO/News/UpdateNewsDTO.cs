using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.News
{
    public class UpdateNewsDto : CreateUpdateNewsDto
    {
        public int Id { get; set; }
    }
}
