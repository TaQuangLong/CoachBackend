using EverCoach.Domain.AggregatesModel.CoachAggregate;
using EverCoach.Domain.AggregatesModel.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EverCoach.Infrastructure.Repositories
{
    public class CoachRepository
        :ICoachRepository
    {
        private readonly CoachContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public CoachRepository(CoachContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Coach>> GetAllAsync()
        {
            return await _context.Set<Coach>().ToListAsync();
        }

        public async Task<Coach> GetByIdAsync(Guid entityId)
        {
            return await _context.Set<Coach>().FindAsync(entityId);
        }

        public Coach Add(Coach entity)
        {
            var coach = _context.Coaches.Add(entity).Entity;
            return coach;
        }

        public void Update(Coach entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Coach entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public void Delete(Guid entityId)
        {
            var entity = _context.Find<Coach>(entityId);
            _context.Entry(entity).State = EntityState.Deleted;
        }
    }
}
