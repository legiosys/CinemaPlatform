using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;
using Domain.Models;
using Domain.DTOs;
using Domain.Exceptions;


namespace Domain.FilmLogic
{
    public interface IIMDB
    {
        Task<Film> CheckFilmOnImdb(Film film);
    } 
    public class OMDB : IIMDB
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }
        
        private readonly ILogger _logger;
        public OMDB(ILogger<OMDB> logger, IConfiguration configuration)
        {
            _logger = logger;
            var omdb = configuration.GetSection("OMDB");        
            Url = omdb["Url"];
            ApiKey = omdb["ApiKey"];
        }
        public async Task<Film> CheckFilmOnImdb(Film film)
        {
            var result = await Url.SetQueryParams(new { apikey = this.ApiKey, i = film.ImdbId })
                .GetJsonAsync<Dto_ImdbFilm>();
            _logger.LogInformation("imdb result: {0}", result.Title);
            if (result.Title == null) throw new NotFoundException("Film doesn't exist on IMDB!", $"imdbID:{film.ImdbId}", "CheckFilmOnImdb");
            film.Name ??= result.Title;
            film.Description ??= result.Plot;
            return film;
        }

    }
}
