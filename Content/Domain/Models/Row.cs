using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Domain.DTOs;
using Domain.Exceptions;

namespace Domain.Models
{
    public class Row
    {
        public int RowId { get; private set; }
        public char Letter { get; private set; }
        public int Seats { get; private set; } 
        public int HallId { get; private set; }
        public Hall Hall { get; private set; }

        private Row() { }
        public Row(char letter, int seats)
        {
            if (!Char.IsLetter(letter)) throw new BadArgumentException("Argument is not a letter!", "Letter");
            letter = char.ToUpper(letter);
            Letter = letter;
            if (seats < 1) throw new BadArgumentException("Argument could not be less then 1!", "Seats");
            Seats = seats;
        }

        public void SetSeats(int seats)
        {
            if(seats < 1) throw new BadArgumentException("Argument could not be less then 1!", "Seats");
            this.Seats = seats;
        }

        static public List<Row> ToListFromDto(IEnumerable<Dto_Row> dtorows)
        {
            var rows = new List<Row>();
            foreach (var row in dtorows)
                rows.Add(new Row(row.Letter, row.Seats));
            return rows;
        }
    }
}
