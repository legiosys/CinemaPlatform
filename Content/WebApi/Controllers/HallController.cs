using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.DTOs;
using Domain.Exceptions;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class HallController : ControllerBase
    {
        private readonly DomainContext _context;
        private readonly ILogger _logger;

        public HallController(DomainContext context, ILogger<HallController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>Creates new hall</summary>
        /// <remarks>Sample request: 
        /// 
        ///     PUT /Hall
        ///     {
        ///        "Name": "First hall",
        ///        "Rows": [{
        ///             "Letter": "A",
        ///             "Seats" : 20
        ///        },
        ///        {
        ///             "Letter": "B",
        ///             "Seats": 25
        ///        }]
        ///     }
        ///     
        /// </remarks>
        /// <returns>A newly created Hall id</returns>
        /// <response code="400">Bad input parameters</response>
        [HttpPut]
        public async Task<int> PutHall(Dto_AddNew_Hall dtohall)
        {
            _logger.LogInformation($"Putting hall {dtohall.Name}");
            Hall hall = new Hall(dtohall.Name, Row.ToListFromDto(dtohall.Rows));
            _context.Halls.Add(hall);
            await _context.SaveChangesAsync();
            return hall.HallId;
        }

        /// <summary>Shows all halls</summary>
        /// <remarks>Sample request: 
        /// 
        /// GET /Hall/All
        /// 
        /// </remarks>
        /// <returns>List of halls</returns>
        [HttpGet("All")]
        public async Task<IEnumerable<Dto_Hall>> GetHalls()
        {
            _logger.LogInformation("Getting all halls");
            var halls = await GetHallsWithDependencies();
            return halls.Select(h => CreateDtoHall(h));
        }

        /// <summary>Shows hall</summary>
        /// <remarks>Sample request: 
        /// 
        ///     GET /Hall/1
        ///     
        /// </remarks>
        /// <param name="id">ID of Hall</param>
        /// <returns>Hall</returns>
        /// <response code="404">Hall with {HallId} not found</response>
        [HttpGet("{id}")]
        public async Task<Dto_Hall> GetHall(int id)
        {
            _logger.LogInformation($"Getting hall with ID#{id}");
            var hall = await GetHallWithDependencies(id);
            return CreateDtoHall(hall);
        }

        /// <summary>Changes rows of the hall</summary>
        /// <remarks>Sample request: 
        /// 
        ///     POST {
        ///         "HallId": 1,
        ///         "Letter": "a",
        ///         "Seats": 15
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Changes applied</response>
        /// <response code="400">Bad input parameters</response>
        /// <response code="404">Hall with {HallId} not found</response>
        [HttpPost("AddRow")]
        [HttpPost("ChangeRow")]
        public async Task<IActionResult> AddOrChangeRow(Dto_AddEdit_Row row)
        {
            _logger.LogInformation($"Changing hall#{row.HallId} row Letter:{row.Letter}, Seats:{row.Seats}");
            Hall hall = await GetHallWithDependencies(row.HallId);
            hall.AddOrChangeRow(row.Letter,row.Seats);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>Removes row of the hall</summary>
        /// <remarks>Sample request: 
        /// 
        ///     POST {
        ///         "HallId": 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Changes applied</response>
        /// <response code="400">Bad input parameters</response>
        /// <response code="404">Hall with {HallId}  or Row{Letter} not found</response>
        [HttpPost("RemoveRow")]
        public async Task<IActionResult> RemoveRow(Dto_Remove_Row row)
        {
            _logger.LogInformation($"Removing hall#{row.HallId} row Letter:{row.Letter}");
            Hall hall = await GetHallWithDependencies(row.HallId);
            hall.RemoveRow(row.Letter);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>Closes hall for reconstruction</summary>
        /// <remarks>Sample request: 
        /// 
        ///     POST {
        ///         "HallId": 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Changes applied</response>
        /// <response code="400">Bad input parameters</response>
        /// <response code="404">Hall with {HallId} not found</response>
        [HttpPost("CloseForReconstruction")]
        public async Task<IActionResult> CloseForReconstruction(Dto_CloseForRec_Hall dtohall)
        {
            _logger.LogInformation($"Closing hall#{dtohall.HallId} for reconstruction");
            Hall hall = await GetHallWithDependencies(dtohall.HallId);
            hall.CloseForReconstruction();
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>Opens hall after reconstruction</summary>
        /// <remarks>Sample request: 
        /// 
        ///     POST {
        ///         "HallId": 1,
        ///         "Name" : "New name"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Changes applied</response>
        /// <response code="400">Bad input parameters</response>
        /// <response code="404">Hall with {HallId} not found</response>
        [HttpPost("OpenAfterReconstruction")]
        public async Task<IActionResult> OpenAfterReconstruction(Dto_OpenAfterRec_Hall dtohall)
        {
            _logger.LogInformation($"Opening hall#{dtohall.HallId} after reconstruction, new name:{dtohall.Name}");
            Hall hall = await GetHallWithDependencies(dtohall.HallId);
            hall.OpenAfterReconstruction(dtohall.Name);
            await _context.SaveChangesAsync();
            return Ok();
        }


        private Task<List<Hall>> GetHallsWithDependencies()
        {
            return _context.Halls.Include(hall => hall.Rows).ToListAsync();
        }

        private async Task<Hall> GetHallWithDependencies(int id)
        {
            var hall = (await GetHallsWithDependencies()).FirstOrDefault(h => h.HallId == id);
            if (hall == null ) throw new NotFoundException("Hall doesn't exist!", $"HallId:{id}", "GetHallWithDependencies");
            return hall;
        }
        
        private IEnumerable<Dto_Row> GetHallRows(List<Row> rows)
        {
            return rows.Select(r => new Dto_Row(r.Letter, r.Seats));
        }

        private Dto_Hall CreateDtoHall(Hall hall)
        {
            return new Dto_Hall(
                hall.HallId,
                hall.Name,
                hall.Reconstruction,
                GetHallRows(hall.Rows)
                );
        }
    }
}
