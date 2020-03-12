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
    [Route("[controller]")]
    [ApiController]
    public class HallController : ControllerBase
    {
        private readonly DomainContext _context;

        public HallController(DomainContext context)
        {
            _context = context;
        }

        // GET: Hall/All Show all halls
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<Hall>>> GetHalls()
        {
            return await _context.Halls.Include(hall => hall.Rows).ToListAsync();
        }

        // GET: Hall/5 Show concrete hall
        [HttpGet("{id}")]
        public async Task<ActionResult<Hall>> GetHall(int id)
        {
            var hall = await _context.Halls.Include(hall => hall.Rows).FirstOrDefaultAsync(h => h.HallId == id);
            
            if (hall == null)
            {
                return NotFound();
            }

            return hall;
        }

        // PUT: Hall/5 Creating new hall
        [HttpPut]
        public async Task<ActionResult<int>> PutHall(Hall.Json jsonhall)
        {
            Hall hall = new Hall(jsonhall);
            _context.Halls.Add(hall);
            await _context.SaveChangesAsync();
            return hall.HallId;
        }


        // POST: Hall/AddRow
        [HttpPost("AddRow")]
        public async Task<ActionResult<Hall>> AddRow(Hall hall)
        {
            _context.Halls.Add(hall);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHall", new { id = hall.HallId }, hall);
        }


        // POST: Hall/RemoveRow
        [HttpPost("RemoveRow")]
        public async Task<ActionResult<Hall>> RemoveRow(Hall hall)
        {
            _context.Halls.Add(hall);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHall", new { id = hall.HallId }, hall);
        }


        // POST: Hall/ChangeRow
        [HttpPost("ChangeRow")]
        public async Task<ActionResult<Hall>> ChangeRow(Hall hall)
        {
            _context.Halls.Add(hall);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHall", new { id = hall.HallId }, hall);
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
                //CreatedAtAction("GetHall", new { id = hall.HallId }, hall);
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


        // DELETE: api/Halls/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Hall>> DeleteHall(int id)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall == null)
            {
                return NotFound();
            }

            _context.Halls.Remove(hall);
            await _context.SaveChangesAsync();

            return hall;
        }

        private bool HallExists(int id)
        {
            return _context.Halls.Any(e => e.HallId == id);
        }
    }
}
