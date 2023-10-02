using marauderserver.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;
using marauderserver.Models;

namespace marauderserver.Hubs
{
    public class EditorHub : Hub
    {
        public async Task NewShape(Shape shape)
        {
            await Clients.All.SendAsync("shapeReceived", shape);
        }
    }
}