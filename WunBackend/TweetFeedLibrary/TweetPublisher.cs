using System;
using System.Text;
using StackExchange.Redis;

namespace Wun.Backend.TweetFeedHandler
{
    public class TweetPublisher : IDisposable
    {
        private readonly ConnectionMultiplexer _redisConnection;

        public int CacheCount { get; set; } = 5;
        public string NotificationChannelName { get; set; } = "wun/fast-path";

        public TweetPublisher(string connectionString)
        {
            _redisConnection = ConnectionMultiplexer.Connect(connectionString);
        }

        public void PublishTweet(Tweet tweet)
        {
            _redisConnection.GetSubscriber().Publish(NotificationChannelName, tweet.SerializeJSON());
        }

        public void Dispose()
        {
            _redisConnection?.Dispose();
        }
    }
}