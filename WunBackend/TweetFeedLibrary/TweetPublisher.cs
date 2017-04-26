using System;
using System.Text;
using StackExchange.Redis;

namespace TwitterConsole
{
    public class TweetPublisher : IDisposable
    {
        private readonly ConnectionMultiplexer _redisConnection;

        public int CacheCount { get; set; } = 5;
        public string NotificationChannelName { get; set; } = "new-tweet-for-user";

        public TweetPublisher(string connectionString)
        {
            _redisConnection = ConnectionMultiplexer.Connect(connectionString);
        }

        public void PublishTweet(Tweet tweet)
        {
            IDatabase redisDatabase = _redisConnection.GetDatabase();
            redisDatabase.ListLeftPush(tweet.ScreenName, tweet.ScreenName);
            redisDatabase.ListTrim(tweet.ScreenName, 0, CacheCount - 1);
            _redisConnection.GetSubscriber().Publish(NotificationChannelName, tweet.ScreenName);
        }

        public void Dispose()
        {
            _redisConnection?.Dispose();
        }
    }
}