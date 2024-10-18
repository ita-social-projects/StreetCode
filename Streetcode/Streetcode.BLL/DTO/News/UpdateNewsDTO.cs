using System.ComponentModel.DataAnnotations;

namespace Streetcode.BLL.DTO.News
{
    public class UpdateNewsDTO : CreateUpdateNewsDTO
    {
        public int Id { get; set; }
    }
}
