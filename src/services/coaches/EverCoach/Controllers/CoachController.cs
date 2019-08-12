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
    [Route("api/v1/[controller]")]
    [ApiController]
    //[EnableCors("MyPolicy")]
    public class CoachController : ControllerBase
    {
        private readonly ICoachRepository _coachRepository;
        public CoachController(ICoachRepository coachRepository)
        {
            _coachRepository = coachRepository;
        }
       
        // GET: api/Coach
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var coaches = await _coachRepository.GetAllAsync();
                return new OkObjectResult(coaches);
            }
             catch(Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/Coach/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCoach(int id)
        {
            var coach = await _coachRepository.GetByIdAsync(id);
            if (coach == null)
            {
                return NotFound();
            }
            return Ok(coach);
        }

        //POST: api/Coach
        [HttpPost]
        public async Task<IActionResult> CreateCoach([FromBody] CreateCoachCommand command)
        {
            var coach = new Coach(command.Name,command.Email,command.Age,command.PhoneNum);
            _coachRepository.Add(coach);
            try
            {
                await _coachRepository.CommitAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return  Ok();
        }

        //PUT: api/Coach/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoach(int id, [FromBody]UpdateCoachCommand item)
        {
            var coach = await _coachRepository.GetByIdAsync(id);
            coach.Update(item.Name,item.Email);
            await _coachRepository.CommitAsync();
            return Ok();
        }
        //DELETE: api/Coach/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoach(int id)
        {
            _coachRepository.Delete(id);
            await _coachRepository.CommitAsync();
            return Ok();
        }
    }
}
