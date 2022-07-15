using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.SharedModels;
using TaxiApp.Db;
using System.Linq;

namespace TaxiApp.Driver.Services
{
    public interface IDriverDbService /*: IDataService<Account>*/
    {
        Task Create(SharedModels.Driver driver);
        Task<SharedModels.Driver> GetByUsername(string driverName);
    }

    public class DriverDbService : IDriverDbService
    {
        public Task Create(SharedModels.Driver driver)
        {
            using(Db.AppContext db = new Db.AppContext())
            {
                driver.id = db.Drivers.Count() + 1;
                db.Drivers.Add(driver);
                db.SaveChanges();

                return Task.CompletedTask;
            }

            //throw new NotImplementedException();
        }

        public Task<SharedModels.Driver> GetByUsername(string driverName)
        {
            using (Db.AppContext db = new Db.AppContext())
            {
                var matchedDriver = db.Drivers.FirstOrDefault(c => c.username == driverName);

                return Task.FromResult<SharedModels.Driver>(matchedDriver);
            }
        }

        
    }

}
