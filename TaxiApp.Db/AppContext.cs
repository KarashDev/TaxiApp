using Microsoft.EntityFrameworkCore;
using System;
using TaxiApp.Customer.Models;

namespace TaxiApp.Db
{
    public class AppContext : DbContext
    {
        public DbSet<Customer> Cities { get; set; }
        //public DbSet<RequestHistoryData> RequestHistoryDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Data Source=(localdb)\mssqllocaldb;Initial Catalog=WeatherViewClientDB;Integrated Security=True");
        }

    }
}
