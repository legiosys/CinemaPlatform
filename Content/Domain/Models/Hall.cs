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
        public Hall(int id, string name, bool reconstruction, List<Row> rows)
        {
            HallId = id;
            Name = name;
            Reconstruction = reconstruction;
            Rows = rows ?? new List<Row>();
        }
    }
}
