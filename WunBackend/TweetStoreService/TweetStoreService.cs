using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;

namespace TweetStoreService
{
	/// <summary>
	/// An instance of this class is created for each service instance by the Service Fabric runtime.
	/// </summary>
	internal sealed class TweetStoreService : StatelessService
	{
		private const string EhConnectionString = "Endpoint=sb://pricefeeds-daniel-dev-fastmarkets.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=peRxCtWVYDBs4y30LwYk+5mzwRrEjy2WSW3yds/Kz3s=";
		private const string EhEntityPath = "danielpricefeedhub";

		private const string StorageContainerName = "danieldevstoragecontainer";
		private const string StorageAccountName = "fastmarketsdanieldev";
		private const string StorageAccountKey = "54ny0kei98zFUKlzt4WGv9lHmFd5ADBh+sZiCv/c0QbUN/A6W8oDOL7WVRjaGurGwhK/q8yi00PzTN6J0vZMXg==";

		private static readonly string StorageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, StorageAccountKey);


		public TweetStoreService(StatelessServiceContext context)
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
			var eventProcessorHost = new EventProcessorHost(
				EhEntityPath,
				PartitionReceiver.DefaultConsumerGroupName,
				EhConnectionString,
				StorageConnectionString,
				StorageContainerName);

			while (true)
			{
				cancellationToken.ThrowIfCancellationRequested();

				// Registers the Event Processor Host and starts receiving messages
				await eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>();

				await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
			}
		}
	}
}
