using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.AdditionalContent.Tag
{
    public class GetAllTagsResponseDTO
    {
        public int TotalAmount { get; set; }
        public IEnumerable<TagDTO> Tags { get; set; } = new List<TagDTO>();
    }
}
