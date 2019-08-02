using EverCoach.Domain.AggregatesModel.CoachAggregate;
using EverCoach.Domain.AggregatesModel.SeedWork;
using EverCoach.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EverCoach.Infrastructure
{
    public class CoachContext : DbContext, IUnitOfWork
    {
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
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseNpgsql("Host=postgresql-data;Port=5432;Username=postgres;Password=DigiMed123;Database=CoachManagement", b =>
            {
                b.MigrationsHistoryTable("__EFMigrationsHistory");
            });

        }
       

    }


}
