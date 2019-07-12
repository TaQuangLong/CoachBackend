using EverCoach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverCoach.Repository
{
    public interface ICoachRepository
    {
        Task<IEnumerable<Coach>> GetAllCoachAsync();
        Task<Coach> GetCoachByIdAsync(long id);
        Task CreateCoachAsync(Coach coach);
        Task UpdateCoachAsync(Coach dbCoach, Coach coach);
        Task DeleteCoachAsync(Coach coach);
    }
}
