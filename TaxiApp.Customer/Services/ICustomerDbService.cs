using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.SharedModels;
using TaxiApp.Db;
using System.Linq;

namespace TaxiApp.Customer.Services
{

    public interface ICustomerDbService /*: IDataService<Account>*/
    {
        Task Create(SharedModels.Customer customer);
        Task<SharedModels.Customer> GetByUsername(string customerName);
    }

    public class CustomerService : ICustomerDbService
    {
        public Task Create(SharedModels.Customer customer)
        {
            using(Db.AppContext db = new Db.AppContext())
            {
                customer.id = db.Customers.Count() + 1;
                db.Customers.Add(customer);
                db.SaveChanges();

                return Task.CompletedTask;
            }

            //throw new NotImplementedException();
        }

        public Task<SharedModels.Customer> GetByUsername(string customerName)
        {
            using (Db.AppContext db = new Db.AppContext())
            {
                var matchedCustomer = db.Customers.FirstOrDefault(c => c.username == customerName);

                return Task.FromResult<SharedModels.Customer>(matchedCustomer);
            }
        }

        
    }

}
