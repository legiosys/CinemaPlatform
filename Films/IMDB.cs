using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Exceptions;


namespace Films
{
    public interface IIMDB
    {
        Task<Dto_ImdbFilm> GetFilmOnImdb(string imdbid);
    } 
    public class OMDB : IIMDB
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }
        
        public OMDB(IConfiguration configuration)
        {
            var omdb = configuration.GetSection("OMDB");        
            Url = omdb["Url"];
            ApiKey = omdb["ApiKey"];
        }
        public async Task<Dto_ImdbFilm> GetFilmOnImdb(string imdbid)
        {
            var result = await Url.SetQueryParams(new { apikey = this.ApiKey, i = imdbid })
                .GetJsonAsync<Dto_ImdbFilm>();
            if (result.Title == null) throw new NotFoundException("Film doesn't exist on IMDB!", $"imdbID:{imdbid}", "CheckFilmOnImdb");
            return result;
        }

    }
}
