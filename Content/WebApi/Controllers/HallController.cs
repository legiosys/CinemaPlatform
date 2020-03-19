using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.DTOs;

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
        public async Task<int> PutHall(Dto_AddNew_Hall dtohall)
        {
            
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
        [HttpGet("{id}")]
        public async Task<Dto_Hall> GetHall(int id)
        {
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
        [HttpPost("AddRow")]
        [HttpPost("ChangeRow")]
        public async Task<int> AddOrChangeRow(Dto_AddEdit_Row row)
        {
            Hall hall = await GetHallWithDependencies(row.HallId);
            hall.AddOrChangeRow(row.Letter,row.Seats);
            await _context.SaveChangesAsync();
            return hall.HallId;
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
        [HttpPost("RemoveRow")]
        public async Task<int> RemoveRow(Dto_Remove_Row row)
        {
            Hall hall = await GetHallWithDependencies(row.HallId);
            hall.RemoveRow(row.Letter);
            await _context.SaveChangesAsync();
            return hall.HallId;
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
        [HttpPost("CloseForReconstruction")]
        public async Task<int> CloseForReconstruction(Dto_CloseForRec_Hall dtohall)
        {
            Hall hall = await GetHallWithDependencies(dtohall.Id);
            hall.CloseForReconstruction();
            await _context.SaveChangesAsync();
            return hall.HallId;
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
        [HttpPost("OpenAfterReconstruction")]
        public async Task<int> OpenAfterReconstruction(Dto_OpenAfterRec_Hall dtohall)
        {
            Hall hall = await GetHallWithDependencies(dtohall.Id);
            hall.OpenAfterReconstruction(dtohall.Name);
            await _context.SaveChangesAsync();
            return hall.HallId;
        }


        private Task<List<Hall>> GetHallsWithDependencies()
        {
            return _context.Halls.Include(hall => hall.Rows).ToListAsync();
        }

        private async Task<Hall> GetHallWithDependencies(int id)
        {
            if(id < 1) throw new ArgumentNullException("ID is null!");
            return (await GetHallsWithDependencies()).FirstOrDefault(h => h.HallId == id);
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
