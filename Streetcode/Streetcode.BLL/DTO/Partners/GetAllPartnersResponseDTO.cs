﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Partners
{
    public class GetAllPartnersResponseDTO
    {
        public int TotalAmount { get; set; }
        public IEnumerable<PartnerDTO> Partners { get; set; } = new List<PartnerDTO>();
    }
}
