using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EverCoach.Api.Application.Commands;
using EverCoach.Domain.AggregatesModel.CoachAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EverCoach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("MyPolicy")]
    public class CoachController : ControllerBase
    {
        private readonly ICoachRepository _coachRepository;
       
        // GET: api/Coach
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coach>>> GetCoaches()
        {
            return await _context.Coaches.ToListAsync();
        }

        // GET: api/Coach/5
        [HttpGet("{id}")]
        public async Task<IActionResult<Coach>> GetCoach(Guid id)
        {
            var coach = await _coachRepository.GetByIdAsync(id);
            if (coach == null)
            {
                return NotFound();
            }
            return coach;
        }

        //POST: api/Coach
        [HttpPost]
        public async Task<ActionResult> CreateCoach([FromBody] CreateCoachCommand command)
        {
            var coach = new Coach(command.Name,command.Email,command.Age,command.PhoneNum);
            _coachRepository.Add(coach);
            return  Ok();
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
