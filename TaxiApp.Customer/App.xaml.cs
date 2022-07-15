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
using TaxiApp.Customer.Services;
using TaxiApp.SharedModels;

namespace TaxiApp.Customer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //"DefaultConnection": "User ID=postgres;Password=Karash.301194.;Host=localhost;Port=5432;Database=postgres;Pooling=true;"

        // Запуск одной копии приложения
        System.Threading.Mutex mut;

        // Ниже прописана реализация Dependency Injection
        private ServiceProvider serviceProvider;

        // В конструкторе запускаем сервисы DI
        public App()
        {
            bool createdNew;
            string mutName = "MainWindow";
            mut = new System.Threading.Mutex(true, mutName, out createdNew);
            if (!createdNew)
            {
                Shutdown();
            }
            else
            {
                ServiceCollection services = new ServiceCollection();
                ConfigureServices(services);
                serviceProvider = services.BuildServiceProvider();

                var mainWindow = serviceProvider.GetService<MainWindow>();

                // Если работаем через DI, обязательно убирать в app.xaml строку StartupUri="MainWindow.xaml"
                mainWindow.Show();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ICustomerDbService, CustomerService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<ITaxiOrderSender, TaxiOrderSender>();

            //var configuration = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            //services.Configure<AppSettings>(configuration);

            //// Реализация HTTP через Dependency Injection; корневой путь для подключения берется из конфига
            //var baseURI = configuration.GetValue<string>("BaseURI");

            //// Регистрация HttpClient с его baseURI, а также настройка, чтобы прокси не использовался, даже если он включен на машине
            //services.AddHttpClient<IQueueItemBoardService, QueueItemBoardService>(client =>
            //{
            //    client.BaseAddress = new Uri(baseURI);
            //}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { DefaultProxyCredentials = CredentialCache.DefaultCredentials });
        }

       
        protected override void OnStartup(StartupEventArgs e)
        {
            using (Db.AppContext db = new Db.AppContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Database.Migrate();

                SharedModels.Customer customer = new SharedModels.Customer() { id = 1, username = "Ivan K.", passwordHash = "13151" };
                db.Customers.Add(customer);
                db.SaveChanges();
            }

            base.OnStartup(e);
        }
    }
}
