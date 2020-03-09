using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Hall
    {
        public int HallId { get; set; }
        public string Name { get; set; }

        public List<Row> Rows { get; set; }
    }
}
