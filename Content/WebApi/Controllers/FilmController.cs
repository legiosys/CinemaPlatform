using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Domain.Models;
using Domain.FilmLogic;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private FilmAdapter _films;

        public FilmController(FilmAdapter films)
        {
            _films = films;
        }

        /// <summary>Shows all films</summary>
        /// <remarks>Sample request: 
        /// 
        /// GET /Film
        /// 
        /// </remarks>
        [HttpGet]
        public async Task<IEnumerable<Film>> GetFilms()
        {
            return await _films.GetFilms();
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
            return await _films.GetFilm(imdbid);
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
        public async Task<IActionResult> PostFilm(Film postfilm)
        {
            await _films.AddFilm(postfilm);
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
            await _films.UpdateFilm(putfilm);
            return Ok();
        }

        /// <summary>Patches values</summary>
        /// <remarks>Sample request: 
        /// 
        ///     PATCH/tt3896198 
        ///     [
        ///         {
        ///             "op": "replace",
        ///             "path": "/Name",
        ///             "value": "MegaFilm"
        ///         }
        ///     ]
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPatch("{imdbid}")]
        public async Task<IActionResult> PatchFilm(string imdbid,JsonPatchDocument<Film> patchFilm)
        {
            await _films.PatchFilm(imdbid, patchFilm);
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
            return await _films.DeleteFilm(imdbid);
        }       
    }
}
