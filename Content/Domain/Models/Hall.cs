using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;


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
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("'Name' argument is null!");
            Name = name;
            Reconstruction = false;
            //if (rows.Count() == 0) throw new ArgumentNullException("Hall should contain one or more Rows!");
            Rows = rows;
        }

        //static public List<Dto_Hall> ToDtoList(List<Hall> halls)
        //{
        //    var dtos = new List<Dto_Hall>();
        //    //halls.ForEach(dtos.Add)
        //}

        public void CloseForReconstruction()
        {
            if (Reconstruction) throw new InvalidOperationException("Hall is already on reconstruction.");
            Reconstruction = true;
        }

        public void OpenAfterReconstruction(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("'Name' argument is null!");
            this.Name = name;
            if (!Reconstruction) throw new InvalidOperationException("Hall isn't on reconstruction now.");
            this.Reconstruction = false;
        }

        public void AddOrChangeRow(char letter, int seats)
        {
            var row = Rows.FirstOrDefault(r => r.Letter.Equals(letter));
            if (row == null)
                Rows.Add(new Row(letter, seats));
            else
                row.SetSeats(seats);
        }

        public void RemoveRow(char letter)
        {
            if (!Char.IsLetter(letter)) throw new ArgumentException("'Letter' argument is not letter!");
            Rows.RemoveAll(r => r.Letter.Equals(letter));
        }

        public class Json
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Reconstruction { get; set; }
            public IEnumerable<Row> Rows { get; set; }
        }
    }
}
