﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Event
{
    public class EventShortDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Title { get; set; }
    }
}
