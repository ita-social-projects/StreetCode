using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.News
{
    public class GetAllNewsResponseDto
    {
        public int TotalAmount { get; set; }
        public IEnumerable<NewsDto> News { get; set; } = new List<NewsDto>();
    }
}
