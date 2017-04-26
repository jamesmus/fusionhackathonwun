using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Wun.GatewayApi.Tests.Stubs
{
    public class RedisSubscriberStub : ISubscriber
    {
        private string _testedChannel;
        private Action<RedisChannel, RedisValue> _testedAction;

        public Task<TimeSpan> PingAsync(CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public bool TryWait(Task task)
        {
            throw new NotImplementedException();
        }

        public void Wait(Task task)
        {
            throw new NotImplementedException();
        }

        public T Wait<T>(Task<T> task)
        {
            throw new NotImplementedException();
        }

        public void WaitAll(params Task[] tasks)
        {
            throw new NotImplementedException();
        }

        public ConnectionMultiplexer Multiplexer { get; }
        public TimeSpan Ping(CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public EndPoint IdentifyEndpoint(RedisChannel channel, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<EndPoint> IdentifyEndpointAsync(RedisChannel channel, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public bool IsConnected(RedisChannel channel = new RedisChannel())
        {
            throw new NotImplementedException();
        }

        public long Publish(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
        {
            if (channel == _testedChannel)
            {
                _testedAction(channel, message);
            }

            return 0;
        }

        public Task<long> PublishAsync(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(RedisChannel channel, Action<RedisChannel, RedisValue> handler, CommandFlags flags = CommandFlags.None)
        {
            SubscribeAsync(channel, handler).Wait();
        }

        public Task SubscribeAsync(RedisChannel channel, Action<RedisChannel, RedisValue> handler, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() =>
            {
                _testedChannel = channel;
                _testedAction = handler;
            });
        }

        public EndPoint SubscribedEndpoint(RedisChannel channel)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(RedisChannel channel, Action<RedisChannel, RedisValue> handler = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeAll(CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task UnsubscribeAllAsync(CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task UnsubscribeAsync(RedisChannel channel, Action<RedisChannel, RedisValue> handler = null, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }
    }
}
