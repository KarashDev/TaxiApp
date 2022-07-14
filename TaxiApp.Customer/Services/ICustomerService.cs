using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Customer.Models;

namespace TaxiApp.Customer.Services
{

    public interface ICustomerService /*: IDataService<Account>*/
    {
        Task<Models.Customer> Create(Models.Customer user);
        Task<Models.Customer> GetByUsername(string username);
        Task<Models.Customer> GetByEmail(string email);
    }

    public class CustomerService : ICustomerService
    {
        public Task<Models.Customer> Create(Models.Customer user)
        {
            throw new NotImplementedException();
        }

        public Task<Models.Customer> GetByEmail(string email)
        {
            throw new NotImplementedException();

            //using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            //{
            //    return await context.Accounts
            //        .Include(a => a.AccountHolder)
            //        .Include(a => a.AssetTransactions)
            //        .FirstOrDefaultAsync(a => a.AccountHolder.Email == email);
            //}
        }

        public Task<Models.Customer> GetByUsername(string username)
        {
            throw new NotImplementedException();
        }
    }

}
