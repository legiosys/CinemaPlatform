using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Flurl;
using Flurl.Http;
using Domain.DTOs;
using Domain.Models;
using Domain.Exceptions;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private const string APIKEY = "89ca9b02";
        private readonly DomainContext _context;
        private readonly ILogger _logger;

        public FilmController(DomainContext context, ILogger<HallController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>Shows all films</summary>
        /// <remarks>Sample request: 
        /// 
        /// GET /Film
        /// 
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
        {
            return await _context.Films.ToListAsync();
        }

        /// <summary>Shows film</summary>
        /// <remarks>Sample request: 
        /// 
        /// GET /Film/tt3896198
        /// 
        /// </remarks>
        [HttpGet("{imdbid}")]
        public async Task<ActionResult<Film>> GetFilm(string imdbid)
        {
            var film = await GetFilmById(imdbid);          
            return film;
        }

        /// <summary>Creates new film</summary>
        /// <remarks>Sample request: 
        /// 
        ///     POST {
        ///         "imdbId": "tt3896198",
        ///         "name": "SuperFilm",
        ///         "description": "Amazing",
        ///         "startDate": "2020-03-27",
        ///         "finishDate": "2020-03-29"
        ///     }
        ///     
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<Film>> PostFilm(Film postfilm)
        {
            var film = await ValidateFilm(postfilm, "POST");
            _context.Films.Add(film);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>Updates film</summary>
        /// <remarks>Sample request: 
        /// 
        ///     PUT {
        ///         "imdbId": "tt3896198",
        ///         "name": "AmazingFilm",
        ///         "description": "Super",
        ///         "startDate": "2020-04-1",
        ///         "finishDate": "2020-04-30"
        ///     }
        ///     
        /// </remarks>
        [HttpPut]
        public async Task<IActionResult> PutFilm(Film putfilm)
        {
            var film = await GetFilmById(putfilm.ImdbId);
            film.Name = putfilm.Name;
            film.Description = putfilm.Description;
            film.StartDate = putfilm.StartDate;
            film.FinishDate = putfilm.FinishDate;
            film = await ValidateFilm(film, "PUT");
            _context.Entry(film).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }


        /// <summary>Patches values</summary>
        /// <remarks>Sample request: 
        /// 
        ///     PATCH/tt3896198 {
        ///         "op": "replace",
        ///         "path": "/Name",
        ///         "value": "MegaFilm"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPatch("{imdbid}")]
        public async Task<IActionResult> PatchFilm(string imdbid,JsonPatchDocument<Film> patchFilm)
        {
            var film = await GetFilmById(imdbid);
            patchFilm.ApplyTo(film);
            film = await ValidateFilm(film,"PATCH");
            _context.Entry(film).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>Deletes film</summary>
        /// <remarks>Sample request: 
        /// 
        /// DELETE /Film/tt3896198
        /// 
        /// </remarks>
        [HttpDelete("{imdbid}")]
        public async Task<ActionResult<Film>> DeleteFilm(string imdbid)
        {
            _logger.LogInformation("DELETE: {0}", imdbid);
            var film = await GetFilmById(imdbid);
            _context.Films.Remove(film);
            await _context.SaveChangesAsync();
            return film;
        }

        private async Task<Film> GetFilmById(string id)
        {
            var film = await _context.Films.FirstOrDefaultAsync(f => f.ImdbId == id);
            if (film == null) throw new NotFoundException("Film doesn't exist!", $"imdbID:{id}", "GetFilmById");
            return film;
        }

        private async Task<Film> CheckFilmOnImdb(Film film)
        {
            var result = await "http://www.omdbapi.com"
                .SetQueryParams(new { apikey = APIKEY, i = film.ImdbId })
                .GetJsonAsync<Dto_ImdbFilm>();
            _logger.LogInformation("imdb result: {0}", result.Title);
            if (result.Title == null) throw new NotFoundException("Film doesn't exist on IMDB!", $"imdbID:{film.ImdbId}", "CheckFilmOnImdb");
            film.Name ??= result.Title;
            film.Description ??= result.Plot;
            return film;
        }

        private async Task<Film> ValidateFilm (Film film, string method)
        {
            if (method.Equals("POST") || film.Name == null || film.Description == null)
                film = await CheckFilmOnImdb(film);
            _logger.LogInformation($"{method}: Changes to dbo.Films: imdbid: {film.ImdbId} \n" +
                $"Name: {film.Name} \n Description: {film.Description} \n" +
                $"StartDate: {film.StartDate} \n FinishDate: {film.FinishDate}");
            if (film.StartDate > film.FinishDate) 
                throw new BadArgumentException("Start Date should be earlier then finish!", $"Start: {film.StartDate}, Finish: {film.FinishDate}", "PostFilm");
            return film;
        }
    }
}
