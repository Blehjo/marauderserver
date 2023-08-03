using marauderserver.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;

namespace marauderserver.Hubs
{
    public class ChatHub : Hub<IChatClient>
    { }
}