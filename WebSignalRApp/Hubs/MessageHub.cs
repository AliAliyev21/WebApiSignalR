using Microsoft.AspNetCore.SignalR;
using WebSignalRApp.Helpers;

namespace WebSignalRApp.Hubs
{
    public class MessageHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Others.SendAsync("ReceiveConnectInfo", "User connected");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Others.SendAsync("ReceiveDisconnectInfo", "User disconnected");
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message + "'s Offer : ", FileHelper.Read());
        }

        public async Task UpdateBid(double data, string user)
        {
            await Clients.All.SendAsync("ReceiveBidUpdate", FileHelper.Read(), user);
        }
    }
}
