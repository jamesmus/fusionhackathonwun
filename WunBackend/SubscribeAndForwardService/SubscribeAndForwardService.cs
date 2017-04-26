using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.Azure.EventHubs;
using System.Text;
using StackExchange.Redis;

namespace SubscribeAndForwardService
{
	/// <summary>
	/// An instance of this class is created for each service instance by the Service Fabric runtime.
	/// </summary>
	internal sealed class SubscribeAndForwardService : StatelessService
	{
		ConnectionMultiplexer _redisConnection;
		private static EventHubClient eventHubClient;
		private const string EhConnectionString = "";
		private const string EhEntityPath = "";
		private const string redis_connectionString = "";
		private const String channelName = "wun/fast-path";

		public SubscribeAndForwardService(StatelessServiceContext context)
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

		private void PublishTweet(String tweet)
		{
			try
			{				
				eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(tweet)));
			}
			catch (Exception exception)
			{
				Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
			}
		}

		private String ReadTweet()
		{
			String tweet = "";

			_redisConnection.GetSubscriber().Subscribe(channelName, (channel, message) =>
			{
				tweet=message;
			});

			return tweet;
		}

		/// <summary>
		/// This is the main entry point for your service instance.
		/// </summary>
		/// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
		protected override async Task RunAsync(CancellationToken cancellationToken)
		{
			var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
			{
				EntityPath = EhEntityPath
			};

			eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

			_redisConnection = ConnectionMultiplexer.Connect(redis_connectionString);

			while (true)
			{
				cancellationToken.ThrowIfCancellationRequested();

				//read a JSON tweet from REDIS and ....
				String tweet = ReadTweet();
				//republoish onto eventhub
				PublishTweet(tweet);

				await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
			}
		}
	}
}
