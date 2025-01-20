using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.DTO.Event
{
    public class EventDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
