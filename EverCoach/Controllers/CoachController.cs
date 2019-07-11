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
        private readonly IDataRepository<Coach> _dataRepository;
        public CoachController(IDataRepository<Coach> dataRepository)
        {
            _dataRepository = dataRepository;
        }
        // GET: api/Coach
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Coach> coaches = _dataRepository.GetAll();
            return Ok(coaches);
        }

        // GET: api/Coach/5
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            Coach coach = _dataRepository.Get(id);
            if (coach == null)
            {
                return NotFound("The Coach record couldn't be found");
            }
            return Ok(coach);
        }

        //POST: api/Coach
        [HttpPost]
        public IActionResult Post([FromBody] Coach coach)
        {
            if (coach == null)
            {
                return BadRequest("Coach is null.");
            }
            _dataRepository.Add(coach);
            return CreatedAtRoute(
                "Get",
                new { Id = coach.Id },
                coach);
        }

        //PUT: api/Coach/5
        [HttpPut("{id}")]
        public IActionResult Put(long id,[FromBody] Coach coach)
        {
            if (coach == null)
            {
                return BadRequest("Coach is null");
            }
            Coach coachToUpdate = _dataRepository.Get(id);
            if(coachToUpdate == null)
            {
                return NotFound("The Coach recod couldn't be found.");
            }
            _dataRepository.Update(coachToUpdate, coach);
            return NoContent();
        }
        //DELETE: api/Coach/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Coach coach = _dataRepository.Get(id);
            if(coach == null)
            {
                return NotFound("The Coach record couldn't found.");
            }
            _dataRepository.Delete(coach);
            
            return NoContent();
        }
    }
}
