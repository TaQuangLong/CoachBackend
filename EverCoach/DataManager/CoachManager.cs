using EverCoach.Models;
using EverCoach.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverCoach.DataManager
{
    public class CoachManager: IDataRepository<Coach>
    {
        readonly CoachContext _coachContext;
        public CoachManager(CoachContext context)
        {
            _coachContext = context;
        }
        public IEnumerable<Coach> GetAll()
        {
            return _coachContext.Coaches.ToList();
        }
        public Coach Get(long id)
        {
            return _coachContext.Coaches.FirstOrDefault(e => e.Id == id);
        }
        public void Add(Coach entity)
        {
            _coachContext.Coaches.Add(entity);
            _coachContext.SaveChanges();
        }
        public void Update(Coach coach, Coach entity)
        {
            coach.Name = entity.Name;
            coach.PhoneNum = entity.PhoneNum;
            coach.Sex = entity.Sex;
            coach.Address = entity.Address;
            coach.Age = entity.Age;
            coach.Dob = entity.Dob;
            coach.Email = entity.Email;

            _coachContext.SaveChanges();
        }
        public void Delete(Coach coach)
        {
            _coachContext.Coaches.Remove(coach);
            _coachContext.SaveChanges();
        }

        public IEnumerable<Coach> GetALL()
        {
            throw new NotImplementedException();
        }
    }
}
