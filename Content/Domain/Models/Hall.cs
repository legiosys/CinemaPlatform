using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Exceptions;


namespace Domain.Models
{
    public class Hall
    {
        public int HallId { get; private set; }
        public string Name { get; private set; }
        public bool Reconstruction { get; private set; }

        public List<Row> Rows { get; private set; }

        private Hall() { }
        public Hall(string name,List<Row> rows)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new BadArgumentException("Argument is null!","Hall name");
            Name = name;
            Reconstruction = false;
            //if (rows.Count() == 0) throw new BadArgumentException("Hall should contain one or more Rows!","Rows");
            Rows = rows;
        }

        public void CloseForReconstruction()
        {
            if (Reconstruction) throw new BadArgumentException("Hall is already on reconstruction.", $"HallId:{HallId}");
            Reconstruction = true;
        }

        public void OpenAfterReconstruction(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new BadArgumentException("Argument is null!", "Hall name");
            this.Name = name;
            if (!Reconstruction) throw new BadArgumentException("Hall isn't on reconstruction now.", $"HallId:{HallId}");
            this.Reconstruction = false;
        }

        public void AddOrChangeRow(char letter, int seats)
        {
            letter = char.ToUpper(letter);
            var row = Rows.FirstOrDefault(r => r.Letter.Equals(letter));
            if (row == null)
                Rows.Add(new Row(letter, seats));
            else
                row.SetSeats(seats);
        }

        public void RemoveRow(char letter)
        {
            letter = char.ToUpper(letter);
            if (!Char.IsLetter(letter)) throw new BadArgumentException("Argument is not a letter!", "Letter");
            if(Rows.RemoveAll(r => r.Letter.Equals(letter)) < 1) throw new NotFoundException($"Row was not removed! '{letter}' is not exist", "Letter");;
        }

    }
}
