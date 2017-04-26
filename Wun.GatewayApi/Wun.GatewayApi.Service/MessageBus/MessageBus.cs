using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;
using Wun.GatewayApi.Service.MessageBus.MessageCommands;

namespace Wun.GatewayApi.Service.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly ISubscriber _redisSubscriber;

        public MessageBus(ISubscriber redisSubscriber)
        {
            _redisSubscriber = redisSubscriber;
        }

        public async Task Subscribe<TTweetMessage>(string subscriptionName, IMessageCommand messageCommand)
        {
            await _redisSubscriber.SubscribeAsync(subscriptionName, (channel, value) => HandleMessage<TTweetMessage>(value, messageCommand));
        }

        private void HandleMessage<TTweetMessage>(RedisValue redisValue, IMessageCommand command)
        {
            var message = JsonConvert.DeserializeObject<TTweetMessage>(redisValue);
            command.Execute(message);
        }
    }
}
