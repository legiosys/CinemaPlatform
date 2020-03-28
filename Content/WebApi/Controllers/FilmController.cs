using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.Exceptions;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly DomainContext _context;

        public FilmController(DomainContext context)
        {
            _context = context;
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
        public async Task<ActionResult<Film>> PostFilm(Film film)
        {
            //if(film.ImdbId == null) throw BadArgumentException("Imdb")
            CheckDates(film.StartDate, film.FinishDate);
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
            CheckDates(putfilm.StartDate, putfilm.FinishDate);
            film.Name = putfilm.Name;
            film.Description = putfilm.Description;
            film.StartDate = putfilm.StartDate;
            film.FinishDate = putfilm.FinishDate;
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

        private void CheckDates(DateTime start, DateTime finish)
        {
            if (start > finish) throw new BadArgumentException("Start Date should be earlier then finish!", $"Start: {start}, Finish: {finish}", "PostFilm");
        }
    }
}
