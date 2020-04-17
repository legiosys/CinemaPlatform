using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.FilmLogic
{
    public class FilmAdapter
    {
        private readonly DomainContext _context;
        private readonly ILogger _logger;
        private readonly Validator _validator;
        public FilmAdapter(DomainContext context, ILogger<FilmAdapter> logger, Validator validator)
        {
            _context = context;
            _logger = logger;
            _validator = validator;
        }

        private async Task<Film> GetFilmById(string id)
        {
            var film = await _context.Films.FirstOrDefaultAsync(f => f.ImdbId == id);
            if (film == null) throw new NotFoundException("Film doesn't exist!", $"imdbID:{id}", "GetFilmById");
            return film;
        }
        //Gets film by IMDB ID
        public async Task<Film> GetFilm(string imdbid)
        {
            var film = await GetFilmById(imdbid);
            return film;
        }
        //Gets all films
        public async Task<IEnumerable<Film>> GetFilms()
        {
            return await _context.Films.ToListAsync();
        }
        //Adds new film to DB
        public async Task AddFilm(Film postfilm)
        {
            var film = await _validator.Validate(postfilm, "POST");
            _context.Films.Add(film);
            await _context.SaveChangesAsync();
        }
        //Updates film
        public async Task UpdateFilm(Film putfilm)
        {
            var film = await GetFilmById(putfilm.ImdbId);
            film.Name = putfilm.Name;
            film.Description = putfilm.Description;
            film.StartDate = putfilm.StartDate;
            film.FinishDate = putfilm.FinishDate;
            film = await _validator.Validate(film, "PUT");
            _context.Films.Update(film);
            await _context.SaveChangesAsync();
        }
        //Patches film
        public async Task PatchFilm(string imdbid, JsonPatchDocument<Film> patchFilm)
        {
            var film = await GetFilmById(imdbid);
            patchFilm.ApplyTo(film);
            film = await _validator.Validate(film, "PATCH");
            _context.Films.Update(film);
            await _context.SaveChangesAsync();
        }
        //Removes film from DB
        public async Task<Film> DeleteFilm(string imdbid)
        {
            _logger.LogInformation("DELETE: {0}", imdbid);
            var film = await GetFilmById(imdbid);
            _context.Films.Remove(film);
            await _context.SaveChangesAsync();
            return film;
        }
    }
}
