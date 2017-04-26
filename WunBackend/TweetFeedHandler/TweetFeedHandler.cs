using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using StackExchange.Redis;
using System.Re

namespace Wun.Backend.TweetFeedHandler
{
	/// <summary>
	/// An instance of this class is created for each service instance by the Service Fabric runtime.
	/// </summary>
	internal sealed class TweetFeedHandler : StatelessService
	{
		public TweetFeedHandler(StatelessServiceContext context)
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
			TweetPublisher publisher = new TweetPublisher("");
			TwitterClient client = new TwitterClient();
			client.ConsumerKey = "THxIAtcutrZVGoIVOVOinhLLk";
			client.ConsumerSecret = "gvc5ZdCFX0zOVuJEbu7n4FFospswbHVwqnNoHXms1lxcW8Ikng";
			client.Username ="dt07715098";
			client.Password = "mbdt2017";
			using (var subscription = (await client.GetTweetStreamAsync()).Subscribe(publisher.PublishTweet))
			{
				while (true)
				{
					cancellationToken.ThrowIfCancellationRequested();
					await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
				}
			}
		}
	}
}
