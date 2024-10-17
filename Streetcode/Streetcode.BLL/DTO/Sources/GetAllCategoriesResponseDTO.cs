using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Sources
{
    public class GetAllCategoriesResponseDTO
    {
        public int TotalAmount { get; set; }
        public IEnumerable<SourceLinkCategoryDTO> Categories { get; set; } = new List<SourceLinkCategoryDTO>();
    }
}
