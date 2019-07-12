using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EverCoach.Models;
using EverCoach.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EverCoach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachController : ControllerBase
    {
        private ICoachRepository _dataRepository;
        
        // GET: api/Coach
        [HttpGet]
        public async Task<IActionResult> GetAllCoaches()
        {
            try
            {
                var coaches = await _dataRepository.GetAllCoachAsync();
                return Ok(coaches);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Coach/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCoachById(long id)
        {
            try
            {
                var coach = await _dataRepository.GetCoachByIdAsync(id);
                if (coach == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(coach);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        //POST: api/Coach
        [HttpPost]
        public async Task<IActionResult> CreateCoach([FromBody] Coach coach)
        {
            try
            {
                if (coach == null)
                {
                    return BadRequest("Coach is null.");
                }
                await _dataRepository.CreateCoachAsync(coach);
                return CreatedAtRoute(
                    "CoachById",
                    new { Id = coach.Id },
                    coach);
            }
            catch(Exception ex)
            {
                throw ex;
                return StatusCode(500, "Internal server error");
            }
            
        }

        //PUT: api/Coach/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id,[FromBody] Coach coach)
        {
            try
            {
                if (coach == null)
                {
                    return BadRequest("Coach is null");
                }
                Coach coachToUpdate = await _dataRepository.GetCoachByIdAsync(id);
                if (coachToUpdate == null)
                {
                    return NotFound("The Coach recod couldn't be found.");
                }
                await _dataRepository.UpdateCoachAsync(coachToUpdate, coach);
                return NoContent();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        //DELETE: api/Coach/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            try
            {
                Coach coach = await _dataRepository.GetCoachByIdAsync(id);
                if (coach == null)
                {
                    return NotFound("The Coach record couldn't found.");
                }
                await _dataRepository.DeleteCoachAsync(coach);

                return NoContent();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
