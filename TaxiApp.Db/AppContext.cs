using Microsoft.EntityFrameworkCore;
using System;
using TaxiApp.SharedModels;

namespace TaxiApp.Db
{
    public class AppContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"User ID=postgres;Password=Karash.301194.;Host=localhost;Port=5432;Database=postgre15;Pooling=true;");
        }
    }
}
