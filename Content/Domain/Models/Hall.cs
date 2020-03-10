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
        public bool Reconstruction { get; set; }

        public List<Row> Rows { get; set; }

        private Hall() { }
        public Hall(Json hall)
        {
            Name = hall.Name;
            Reconstruction = hall.Reconstruction;           
            Rows = new List<Row>(hall.Rows);
        }

        public class Json
        {
            public string Name { get; set; }
            public bool Reconstruction { get; set; }
            public IEnumerable<Row> Rows { get; set; }
        }
    }
}
