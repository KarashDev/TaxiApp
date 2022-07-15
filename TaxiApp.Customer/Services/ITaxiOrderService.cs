using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Customer.Services
{
    public interface ITaxiOrderService
    {
        Task SendOrder(HubConnection connection, string methodName, SharedModels.Customer customer);
    }

    public class TaxiOrderSender : ITaxiOrderService
    {
        public async Task SendOrder(HubConnection connection, string methodName, SharedModels.Customer customer)
        {
            await connection.InvokeAsync("NewCustomer", customer);
        }
    }
}
