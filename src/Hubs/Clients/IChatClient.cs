using System.Threading.Tasks;
using marauderserver.Models;

namespace marauderserver.Hubs.Clients
{
    public interface IChatClient
    {
        Task ReceiveMessage(MessageComment messagecomment);
    }
}