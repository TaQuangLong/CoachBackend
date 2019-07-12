using EverCoach.Models;
using EverCoach.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverCoach.DataManager
{
    public class CoachManager: DataRepository<Coach>,ICoachRepository
    {
        public CoachManager(CoachContext coachContext):base(coachContext)
        {

        }

        public async Task<IEnumerable<Coach>> GetAllCoachAsync()
        {
            return await FindAll()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Coach> GetCoachByIdAsync(long id)
        {
            return await FindByCondition(o => o.Id.Equals(id))
                .DefaultIfEmpty(new Coach())
                .SingleAsync();
        }

        public async Task CreateCoachAsync(Coach coach)
        {
            Create(coach);
            await SaveAsync();
        }

        public async Task UpdateCoachAsync(Coach dbCoach, Coach coach)
        {
            coach.Name = dbCoach.Name;
            coach.PhoneNum = dbCoach.PhoneNum;
            coach.Sex = dbCoach.Sex;
            coach.Address = dbCoach.Address;
            coach.Age = dbCoach.Age;
            coach.Dob = dbCoach.Dob;
            coach.Email = dbCoach.Email;
            await SaveAsync();
        }
        public async Task DeleteCoachAsync(Coach coach)
        {
            Delete(coach);
            await SaveAsync();
        }

        
    }
}
