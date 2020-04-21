using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Films
{
    public class FilmService
    {
        private readonly DomainContext _context;
        private readonly ILogger _logger;
        private readonly IIMDB _imdb;
        public FilmService(DomainContext context, ILogger<FilmService> logger, IIMDB imdb)
        {
            _context = context;
            _logger = logger;
            _imdb = imdb;
        }

        private async Task<Film> GetFilmById(string id)
        {
            var film = await _context.Films.FirstOrDefaultAsync(f => f.ImdbId == id);
            if (film == null) throw new NotFoundException("Film doesn't exist!", $"imdbID:{id}", "GetFilmById");
            return film;
        }
        
        private void Log(Film film,string method)
        {
            _logger.LogInformation($"{method}: Changes to dbo.Films: imdbid: {film.ImdbId} \n" +
                $"Name: {film.Name} \n Description: {film.Description} \n" +
                $"StartDate: {film.StartDate} \n FinishDate: {film.FinishDate}");
        }

        private async Task<Film> ImdbRequest(Film film)
        {
            var imdbfilm = await _imdb.GetFilmOnImdb(film.ImdbId);
            return FilmFiller.Fill(film, imdbfilm);
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
            var film = await ImdbRequest(postfilm);
            Validator.Validate(film);
            Log(film,"POST");
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
            if (film.Name == null || film.Description == null)
                film = await ImdbRequest(film);
            Validator.Validate(film);
            Log(film,"PUT");
            _context.Films.Update(film);
            await _context.SaveChangesAsync();
        }
        //Patches film
        public async Task PatchFilm(string imdbid, JsonPatchDocument<Film> patchFilm)
        {
            var film = await GetFilmById(imdbid);
            patchFilm.ApplyTo(film);
            if (film.Name == null || film.Description == null)
                film = await ImdbRequest(film);
            Validator.Validate(film);
            Log(film,"PATCH");
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
