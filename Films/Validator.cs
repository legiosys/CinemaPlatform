using Domain.Exceptions;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Films
{
    public class Validator
    {
        static public void Validate(Film film)
        {
            if (film.StartDate > film.FinishDate)
                throw new BadArgumentException("Start Date should be earlier then finish!", $"Start: {film.StartDate}, Finish: {film.FinishDate}", "PostFilm");
        } 
    }
}
