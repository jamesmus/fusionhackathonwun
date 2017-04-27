using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.Azure.EventHubs;
using System.Text;
using StackExchange.Redis;
using Wun.Backend.TweetCore;

namespace SubscribeAndForwardService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class SubscribeAndForwardService : StatelessService
    {
        public SubscribeAndForwardService(StatelessServiceContext context)
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

            string tweetSubscriberConnectionString = configurationPackage.Settings.Sections["TweetPublisher"].Parameters["ConnectionString"].Value;

            var connectionStringBuilder = new EventHubsConnectionStringBuilder(eventHubSection.Parameters["ConnectionString"].Value)
            {
                EntityPath = eventHubSection.Parameters["EntityPath"].Value
            };

            EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            try
            {
                using (new TweetSubscriber(tweetSubscriberConnectionString, "wun/fast-path", async t =>
                {
                    try
                    {
                        await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(t.ToString())));
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                    }

                }))
                {
                    while (true)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                    }
                }
            }
            finally
            {
                await eventHubClient.CloseAsync();
            }
        }
    }
}
