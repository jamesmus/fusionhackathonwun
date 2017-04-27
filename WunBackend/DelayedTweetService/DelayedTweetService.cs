using System;
using System.Fabric;
using System.Fabric.Description;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.ServiceFabric.Services.Runtime;
using Wun.Backend.TweetCore;

namespace Wun.Backend.DelayedTweetService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class DelayedTweetService : StatelessService
    {
        public DelayedTweetService(StatelessServiceContext context)
                : base(context)
        {
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            ConfigurationPackage configurationPackage = Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            ConfigurationSection eventHubSection = configurationPackage.Settings.Sections["EventHub"];

            string tweetPublisherConnectionString = configurationPackage.Settings.Sections["TweetPublisher"].Parameters["ConnectionString"].Value;

            var eventProcessorHost = new EventProcessorHost(
                eventHubSection.Parameters["EntityPath"].Value,
                PartitionReceiver.DefaultConsumerGroupName,
                eventHubSection.Parameters["ConnectionString"].Value,
                eventHubSection.Parameters["StorageConnectionString"].Value,
                eventHubSection.Parameters["StorageContainerName"].Value);

            using (TweetPublisher tweetPublisher =
                new TweetPublisher(tweetPublisherConnectionString, _ => "wun/delayed"))
            {
                ServiceContext.TweetPublisher = tweetPublisher;

                // Registers the Event Processor Host and starts receiving messages
                await eventProcessorHost.RegisterEventProcessorAsync<DelayedTweetProcessor>();

                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                }
            }
        }
    }
}
