using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTOs
{
    public class Dto_Hall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Reconstruction { get; set; }
        public IEnumerable<Dto_Row> Rows { get; set; }

        public Dto_Hall() { }
        public Dto_Hall(int id, string name, bool reconstruction, IEnumerable<Dto_Row> rows)
        {
            Id = id;
            Name = name;
            Reconstruction = reconstruction;
            Rows = rows;
        }
    }

    public class Dto_AddNew_Hall
    {
        public string Name { get; set; }
        public IEnumerable<Dto_Row> Rows { get; set; }
    }

    public class Dto_OpenAfterRec_Hall
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Dto_CloseForRec_Hall
    {
        public int Id { get; set; }
    }
}
