using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EverCoach.Models;
using EverCoach.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EverCoach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("MyPolicy")]
    public class CoachController : ControllerBase
    {
        //private readonly IDataRepository<Coach> _dataRepository;
        //public CoachController(IDataRepository<Coach> dataRepository)
        //{
        //    _dataRepository = dataRepository;
        //}
        private readonly CoachContext _context;
        public CoachController(CoachContext context)
        {
            _context = context;
            if (_context.Coaches.Count() == 0)
            {
                //_context.Coaches.Add(new Coach { Name = "Germay", Address = "HaNoi", Age = 25, Dob = Convert.ToDateTime("01-01-1990"), PhoneNum = "09999999", Sex = false });
                _context.SaveChanges();
            }
        }
       
        // GET: api/Coach
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coach>>> GetCoaches()
        {
            return await _context.Coaches.ToListAsync();
        }

        // GET: api/Coach/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Coach>> GetCoach(long id)
        {
            var coach = await _context.Coaches.FindAsync(id);
            if (coach == null)
            {
                return NotFound();
            }
            return coach;
        }

        //POST: api/Coach
        [HttpPost]
        public async Task<ActionResult<Coach>> PostCoach(Coach item)
        {
            _context.Coaches.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCoach), new { id = item.Id }, item);
        }

        //PUT: api/Coach/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoach(long id, Coach item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        //DELETE: api/Coach/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoach(long id)
        {
            var coach = await _context.Coaches.FindAsync(id);
            if (coach == null)
            {
                return NotFound();
            }
            _context.Coaches.Remove(coach);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
