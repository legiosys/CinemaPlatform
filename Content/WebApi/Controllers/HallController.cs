using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class HallController : ControllerBase
    {
        private readonly DomainContext _context;

        public HallController(DomainContext context)
        {
            _context = context;
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
        [HttpPut]
        public async Task<ActionResult<int>> PutHall(Hall.Json jsonhall)
        {
            Hall hall = new Hall(jsonhall);
            _context.Halls.Add(hall);
            await _context.SaveChangesAsync();
            return hall.HallId;
        }

        /// <summary>Shows all halls</summary>
        /// <remarks>Sample request: GET /Hall/All</remarks>
        /// <returns>List of halls</returns>
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<Hall>>> GetHalls()
        {
            return await _context.Halls.Include(hall => hall.Rows).ToListAsync();
        }

        /// <summary>Shows hall</summary>
        /// <remarks>Sample request: 
        /// 
        ///     GET /Hall/1
        ///     
        /// </remarks>
        /// <param name="id">ID of Hall</param>
        /// <returns>Hall</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Hall>> GetHall(int id)
        {
            var hall = await GetHallWithDependencies(id);
            
            if (hall == null)
            {
                return NotFound();
            }

            return hall;
        }

        
        // POST: Hall/5/AddRow || Hall/5/ChangeRow
        [HttpPost("{id}/AddRow")]
        [HttpPost("{id}/ChangeRow")]
        public async Task<ActionResult<Row>> AddOrChangeRow(int id, Row row)
        {
            Hall hall = await GetHallWithDependencies(id);
            if (hall == null)
            {
                return NotFound();
            }
            hall.AddOrChangeRow(row);
            await _context.SaveChangesAsync();

            return Ok();
        }


        // POST: Hall/5/RemoveRow
        [HttpPost("{id}/RemoveRow")]
        public async Task<ActionResult<Row>> RemoveRow(int id, Row row)
        {
            Hall hall = await GetHallWithDependencies(id);
            if (hall == null)
            {
                return NotFound();
            }
            hall.RemoveRow(row);
            await _context.SaveChangesAsync();

            return Ok();
        }


        // POST: Hall/CloseForReconstruction
        [HttpPost("CloseForReconstruction")]
        public async Task<ActionResult<Hall>> CloseForReconstruction(Hall.Json jsonhall)
        {
            Hall hall = await _context.Halls.FindAsync(jsonhall.Id);
            if (hall == null)
            {
                return NotFound();
            }
            hall.CloseForReconstruction();
            await _context.SaveChangesAsync();

            return Ok();
        }


        // POST: Hall/OpenAfterReconstruction
        [HttpPost("OpenAfterReconstruction")]
        public async Task<ActionResult<Hall>> OpenAfterReconstruction(Hall.Json jsonhall)
        {
            Hall hall = await _context.Halls.FindAsync(jsonhall.Id);
            if (hall == null)
            {
                return NotFound();
            }
            hall.OpenAfterReconstruction(jsonhall);
            await _context.SaveChangesAsync();

            return Ok();
        }


        private Task<Hall> GetHallWithDependencies(int id)
        {
            return _context.Halls.Include(hall => hall.Rows).FirstOrDefaultAsync(h => h.HallId == id);
        }
    }
}
