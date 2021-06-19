using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebPortal.Models
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<ApplicationIdentityUser> ApplicationIdentityUsers { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
    }
}
