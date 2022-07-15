using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TaxiApp.SharedModels;

namespace TaxiApp.SignalR
{
    public class TaxiHub : Hub
    {
        public async Task NewCustomer(Customer customer)
        {
            await this.Clients.All.SendAsync("NewCustomer", customer);
        }

        public async Task AcceptOrder(Driver driver)
        {
            await this.Clients.All.SendAsync("AcceptOrder", driver);
        }

        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("Connected", Context.ConnectionId );
        }

        public async Task Connected()
        {
            await this.Clients.All.SendAsync("Connected", Context.ConnectionId);
        }
    }
}
