using IdentityServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Initilizations
{
    public static class SeedSampleData
    {
      
        public static async Task Seed(AppIdentityDbContext _context)
        {           
            //await _context.Database.MigrateAsync();
            if (_context.Users.Any()) { }
            var user = new AppUser()
            {
                UserName = "longtq",
                Email = "longtg@gmail.com",
                PhoneNumber = "0123456789",
            };
            IPasswordHasher<AppUser> _passwordHasher = new PasswordHasher<AppUser>();
            user.PasswordHash = _passwordHasher.HashPassword(user, "DigiMed123");
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
