using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Runtime;
using Wun.Backend.TweetModel;
using Wun.Backend.TweetCore;

namespace Wun.Backend.TweetFeedHandler
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class TweetFeedHandlerService : StatelessService
    {
        public TweetFeedHandlerService(StatelessServiceContext context)
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
            string connectionString = configurationPackage.Settings.Sections["TweetPublisher"].Parameters["ConnectionString"].Value;

            var publisher = new TweetPublisher(connectionString, _ => "wun/fast-path");
            var client = new TwitterClient
            {
                // Move along - there's nothing to see here...
                ConsumerKey = "THxIAtcutrZVGoIVOVOinhLLk",
                ConsumerSecret = "gvc5ZdCFX0zOVuJEbu7n4FFospswbHVwqnNoHXms1lxcW8Ikng",
                Username = "dt07715098",
                Password = "mbdt2017"
            };
            using ((await client.GetTweetStreamAsync()).Subscribe(async t => await publisher.PublishTweetAsync(t)))
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
