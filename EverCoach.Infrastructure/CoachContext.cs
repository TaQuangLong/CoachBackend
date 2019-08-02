using EverCoach.Domain.AggregatesModel.CoachAggregate;
using EverCoach.Domain.AggregatesModel.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EverCoach.Infrastructure
{
    public class CoachContext : DbContext, IUnitOfWork
    {
        public DbSet<Coach> Coaches{get;set;}

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
           => optionsBuilder.UseNpgsql("Host = localhost; Port=5432;Username=postgres;Password=123456;Database=CoachManagement");
    }
}
