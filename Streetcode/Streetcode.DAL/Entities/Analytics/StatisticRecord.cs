using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.DAL.Entities.Analytics
{
    public class StatisticRecord
    {
        public int Id { get; set; }
        public int QrId { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
    }
}
