using System.Threading.Tasks;
using Wun.GatewayApi.Service.MessageBus.MessageCommands;

namespace Wun.GatewayApi.Service.MessageBus
{
    public interface IMessageBus
    {
        Task Subscribe<TTweetMessage>(string subscriptionName, IMessageCommand messageCommand);
    }
}
