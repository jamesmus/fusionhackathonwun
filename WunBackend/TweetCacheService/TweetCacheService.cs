using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using StackExchange.Redis;
using System.Text;
using Wun.Backend.TweetFeedHandler;

namespace TweetCacheService
{
	/// <summary>
	/// An instance of this class is created for each service instance by the Service Fabric runtime.
	/// </summary>
	internal sealed class TweetCacheService : StatelessService
	{
		ConnectionMultiplexer _redisConnection;
		private const string redis_connectionString = "";
		private const String channelName = "wun/fast-path";

		ConnectionMultiplexer _redisTweetCacheConnection;
		private const string redis_connectionStringTweetCache = "";
		private const String channelNameTweetCache = "wun/fast-path";

		private String ReadTweet()
		{
			String tweet = "";

			_redisConnection.GetSubscriber().Subscribe(channelName, (channel, message) =>
			{
				tweet = message;
			});

			return tweet;
		}

		public void PublishTweet(string tweet)
		{
			_redisConnection.GetSubscriber().Publish(channelNameTweetCache+"/"+ GetScreenName(tweet), tweet);
		}

		private String GetScreenName(string _tweet)
		{
			Tweet tweet = Tweet.CreateTweet(_tweet);
			return tweet.ScreenName;
		}

		public TweetCacheService(StatelessServiceContext context)
				: base(context)
		{ }

		/// <summary>
		/// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
		/// </summary>
		/// <returns>A collection of listeners.</returns>
		protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
		{
			return new ServiceInstanceListener[0];
		}

		/// <summary>
		/// This is the main entry point for your service instance.
		/// </summary>
		/// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
		protected override async Task RunAsync(CancellationToken cancellationToken)
		{
			
			while (true)
			{
				cancellationToken.ThrowIfCancellationRequested();

				String tweet = ReadTweet();
				PublishTweet(tweet);

				await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
			}
		}
	}
}
