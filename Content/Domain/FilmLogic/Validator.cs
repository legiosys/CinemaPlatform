using Domain.Exceptions;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.FilmLogic
{
    public class Validator
    {
        private readonly ILogger _logger;
        private readonly IIMDB _imdb;
        public Validator(ILogger<Validator> logger, IIMDB imdb)
        {
            _logger = logger;
            _imdb = imdb;
        }
        public async Task<Film> Validate(Film film, string method)
        {
            if (method.Equals("POST") || film.Name == null || film.Description == null)
                film = await _imdb.CheckFilmOnImdb(film);
            _logger.LogInformation($"{method}: Changes to dbo.Films: imdbid: {film.ImdbId} \n" +
                $"Name: {film.Name} \n Description: {film.Description} \n" +
                $"StartDate: {film.StartDate} \n FinishDate: {film.FinishDate}");
            if (film.StartDate > film.FinishDate)
                throw new BadArgumentException("Start Date should be earlier then finish!", $"Start: {film.StartDate}, Finish: {film.FinishDate}", "PostFilm");
            return film;
        } 
    }
}
