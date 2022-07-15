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
using System.Threading.Tasks;
using System.Windows;
using TaxiApp.Driver.Services;
using TaxiApp.SharedModels;


namespace TaxiApp.Driver
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
            services.AddTransient<IDriverDbService, DriverDbService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<ITaxiAnswerService, TaxiAnswerSender>();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            // ОТКЛЮЧЕНО, ПОСКОЛЬКУ ВОЗНИКАЛИ ПРОБЛЕМЫ ПРИ ЗАПУСКЕ ОБОИХ КЛИЕНТОВ ОДНОВРЕМЕННО; ОСТАВИЛ НА СЛУЧАЙ РАБОТЫ С ОДНИМ КЛИЕНТОМ
            //using (Db.AppContext db = new Db.AppContext())
            //{
            //    db.Database.EnsureDeleted();
            //    db.Database.EnsureCreated();

            //    db.Database.Migrate();

            //    SharedModels.Driver driver = new SharedModels.Driver() { id = 1, username = "Mike R.", passwordHash = "1315136" };
            //    db.Drivers.Add(driver);
            //    db.SaveChanges();
            //}

            base.OnStartup(e);
        }
    }
}

