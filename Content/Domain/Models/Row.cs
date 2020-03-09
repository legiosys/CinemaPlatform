using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Row
    {
        public int RowId { get; private set; }
        public char Letter { get; private set; }
        public int Seats { get; private set; } 

        public int HallId { get; set; }
        public Hall Hall { get; set; }

        private Row() { }
        public Row(int id, char letter, int seats)
        {
            RowId = id;
            Letter = letter;
            Seats = seats;
        }
    }
}
