using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Driver.Services
{
    public interface ITaxiAnswerService
    {
        Task SendOrder(HubConnection connection, string methodName, SharedModels.Driver driver);
    }

    public class TaxiAnswerSender : ITaxiAnswerService
    {
        public async Task SendOrder(HubConnection connection, string methodName, SharedModels.Driver driver)
        {
            await connection.InvokeAsync("AcceptOrder", driver);
        }
    }
}
