using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;


namespace Wun.Backend.TweetFeedHandler
{
    public class TweetObserver : IObserver<Tweet>
    {
			private IDatabase redis_cache;
			public TweetObserver(IDatabase _redis_cache)
			{
				redis_cache = _redis_cache;
			}
			public virtual void OnCompleted()
			{
				
			}

			public virtual void OnError(Exception e)
			{
				
			}

			public virtual void OnNext(Tweet value)
			{
				String tweetKey = "???????????";
				//publish this tweet to Redis
				redis_cache.StringSet(tweetKey, value.SerializeJSON());
			}
	}
}
