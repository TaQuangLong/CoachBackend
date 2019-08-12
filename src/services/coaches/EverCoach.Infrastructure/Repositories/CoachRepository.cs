using Dgm.Core.SeedWork;
using EverCoach.Domain.AggregatesModel.CoachAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EverCoach.Infrastructure.Repositories
{
    public class CoachRepository
        : Repository<Coach, CoachContext>, ICoachRepository
    {
        public CoachRepository(CoachContext dbContext) : base(dbContext)
        {

        }
    }
}
