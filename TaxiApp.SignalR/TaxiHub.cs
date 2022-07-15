using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TaxiApp.SharedModels;

namespace TaxiApp.SignalR
{
    public class TaxiHub : Hub
    {
        //Первый параметр метода SendAsync() указывает на метод, который будет получать ответ от сервера,
        //а второй параметр представляет набор значений, которые посылаются в ответе клиенту. 
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("Connected", Context.ConnectionId );
        }

        public async Task Connected()
        {
            await this.Clients.All.SendAsync("Connected", Context.ConnectionId);
        }

        public async Task NewCustomer(Customer customer)
        {
            await this.Clients.All.SendAsync("NewCustomer", customer);
        }

    }
}
