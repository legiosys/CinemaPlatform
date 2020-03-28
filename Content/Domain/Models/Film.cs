using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Film
    {
        public int FilmId { get; set; }
        [Required]
        public string ImdbId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        //public void Patch(string name, string descr, DateTime start, DateTime finish)
        //{
        //    if (!string.IsNullOrWhiteSpace(name)) this.Name = name;
        //}
    }
}
