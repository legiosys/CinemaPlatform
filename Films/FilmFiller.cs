using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Films
{
    public class FilmFiller
    {
        static public Film Fill(Film film, Dto_ImdbFilm imdbfilm)
        {
            film.Name ??= imdbfilm.Title;
            film.Description ??= imdbfilm.Plot;
            return film;
        }
    }
}
