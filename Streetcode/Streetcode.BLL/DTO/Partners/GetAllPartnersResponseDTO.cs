using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Partners
{
    public class GetAllPartnersResponseDto
    {
        public int TotalAmount { get; set; }
        public IEnumerable<PartnerDto> Partners { get; set; } = new List<PartnerDto>();
    }
}
