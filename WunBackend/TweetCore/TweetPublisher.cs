using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using Wun.Backend.TweetModel;

namespace Wun.Backend.TweetCore
{
    public class TweetPublisher : IDisposable
    {
        private readonly Func<Tweet, string> _getChannelName;
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly ISubscriber _redisSubscriber;

        public TweetPublisher(string connectionString, Func<Tweet, string> getChannelName)
        {
            _getChannelName = getChannelName;
            _redisConnection = ConnectionMultiplexer.Connect(connectionString);
            _redisSubscriber = _redisConnection.GetSubscriber();
        }

        public async Task PublishTweetAsync(Tweet tweet)
        {
            await _redisSubscriber.PublishAsync(_getChannelName(tweet), tweet.ToString());
        }

        public void Dispose()
        {
            _redisConnection?.Dispose();
        }
    }
}