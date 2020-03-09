using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Row
    {
        public int RowId { get; set; }
        public char Letter { get; set; }
        public int Seats { get; set; } 

        public int HallId { get; set; }
        public Hall Hall { get; set; }
    }
}
