using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TaxiApp.Customer.Services;
using TaxiApp.SharedModels;

namespace TaxiApp.Customer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();

            var mainWindow = serviceProvider.GetService<MainWindow>();

            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MainWindow>();

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ICustomerDbService, CustomerService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<ITaxiOrderService, TaxiOrderSender>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // ОТКЛЮЧЕНО, ПОСКОЛЬКУ ВОЗНИКАЛИ ПРОБЛЕМЫ ПРИ ЗАПУСКЕ ОБОИХ КЛИЕНТОВ ОДНОВРЕМЕННО; ОСТАВИЛ НА СЛУЧАЙ РАБОТЫ С ОДНИМ КЛИЕНТОМ
            //using (Db.AppContext db = new Db.AppContext())
            //{

            //    db.Database.EnsureDeleted();
            //    db.Database.EnsureCreated();
            //    Thread.Sleep(5000);
            //    db.Database.Migrate();

            //    SharedModels.Customer customer = new SharedModels.Customer() { id = 1, username = "Ivan K.", passwordHash = "13151" };
            //    db.Customers.Add(customer);
            //    db.SaveChanges();
            //}

            base.OnStartup(e);
        }
    }
}
