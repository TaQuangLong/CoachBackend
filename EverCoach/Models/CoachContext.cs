using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverCoach.Models
{
    public class CoachContext : DbContext
    {
        public CoachContext(DbContextOptions<CoachContext> options)
            : base(options)
        {
        }
        public DbSet<Coach> Coaches { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host = localhost; Port=5432;Username=postgres;Password=123456;Database=CoachManagement");
    }
}
