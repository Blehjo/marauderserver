using marauderserver.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;
using marauderserver.Models;

namespace marauderserver.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(MessageComment messageComment)
        {
            await Clients.All.SendAsync("messageReceived", messageComment);
        }
    }
}