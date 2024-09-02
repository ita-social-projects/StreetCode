using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Streetcode.TextContent
{
    public class RelatedTermCreateDTO
    {
        public string Word { get; set; } = null!;
        public int TermId { get; set; }
    }
}
