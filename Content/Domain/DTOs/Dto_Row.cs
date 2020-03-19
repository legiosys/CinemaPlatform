using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTOs
{
    public class Dto_Row
    {
        public char Letter { get; set; }
        public int Seats { get; set; }

        public Dto_Row() { }
        public Dto_Row(char letter, int seats)
        {
            Letter = letter;
            Seats = seats;
        }

    }

    public class Dto_AddEdit_Row
    {
        public int HallId { get; set; }
        public char Letter { get; set; }
        public int Seats { get; set; }
    }

    public class Dto_Remove_Row
    {
        public int HallId { get; set; }
        public char Letter { get; set; }
    }
}
