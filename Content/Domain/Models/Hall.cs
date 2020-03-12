using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Hall
    {
        public int HallId { get; private set; }
        public string Name { get; private set; }
        public bool Reconstruction { get; private set; }

        public List<Row> Rows { get; private set; }

        private Hall() { }
        public Hall(Json hall)
        {
            Name = hall.Name;
            Reconstruction = hall.Reconstruction;           
            Rows = new List<Row>(hall.Rows);
        }

        public void CloseForReconstruction()
        {
            this.Reconstruction = true;
        }

        public void OpenAfterReconstruction(Json hall)
        {
            this.Name = hall.Name;
            this.Reconstruction = false;
        }

        public void AddOrChangeRow(Row row)
        {
            int rowIndex = Rows.FindIndex(r => r.Letter.Equals(row.Letter));
            if (rowIndex < 0)
                Rows.Add(row);
            else
                Rows[rowIndex].Seats = row.Seats;
        }

        public void RemoveRow(Row row)
        {
            Rows.RemoveAll(r => r.Letter.Equals(row.Letter));
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
