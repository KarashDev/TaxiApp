using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Customer.Services
{
    public interface ITaxiOrderSender
    {
        Task SendOrder(string coordinate);
    }

    public class TaxiOrderSender : ITaxiOrderSender
    {
        public Task SendOrder(string coordinate)
        {
            return Task.CompletedTask;


        }
    }
}
