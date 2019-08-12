using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dgm.Core.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Dgm.Core.Repositories
{
    public abstract class BaseDbContext: DbContext, IUnitOfWork
    {
        protected BaseDbContext(DbContextOptions options): base(options)
        {

        }

        protected abstract string GetMigrationSchema();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseNpgsql("Host=postgresql-data;Port=5432;Username=postgres;Password=DigiMed123;Database=CoachManagement", b =>
            {
                b.MigrationsHistoryTable("__EFMigrationsHistory");
            });

        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
