using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using Wun.Backend.TweetModel;

namespace Wun.Backend.TweetCore
{
    public class TweetCache : IDisposable
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _redisDatabase;

        public TweetCache(string connectionString)
        {
            _redisConnection = ConnectionMultiplexer.Connect(connectionString);
            _redisDatabase = _redisConnection.GetDatabase();
        }

        public async Task<Tweet> GetAsync(string screenName)
        {
            string tweetString = await _redisDatabase.StringGetAsync($"wun/latest/{screenName}");
            return tweetString == null ? null : Tweet.Create(tweetString);
        }

        public async Task SetAsync(Tweet tweet)
        {
            await _redisDatabase.StringSetAsync($"wun/latest/{tweet.ScreenName}", tweet.ToString());
        }

        public void Dispose()
        {
            _redisConnection?.Dispose();
        }
    }
}