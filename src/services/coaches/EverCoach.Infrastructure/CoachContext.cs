using Dgm.Core.Repositories;
using Dgm.Core.SeedWork;
using EverCoach.Domain.AggregatesModel.CoachAggregate;
using EverCoach.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EverCoach.Infrastructure
{
    public class CoachContext : BaseDbContext
    {
        public const string SchemaName = "coach";
        public DbSet<Coach> Coaches { get; set; }

        public CoachContext(DbContextOptions<CoachContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CoachEntityTypeConfiguration());

        }

        protected override string GetMigrationSchema()
        {
            return SchemaName;
        }
    }


}
