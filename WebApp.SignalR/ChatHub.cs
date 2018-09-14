using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebApp.SignalR
{
    internal class ChatHub :Hub
    {
        public async Task Send(string message, string userName)
        {
            //await Clients.All.SendAsync("Send", message, userName);

          await  Clients.Others.SendAsync("Send", message, userName);
        }
    }
}