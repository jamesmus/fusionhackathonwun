using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Runtime;
using Wun.Backend.TweetCore;

namespace TweetCacheService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class TweetCacheService : StatelessService
    {
        public TweetCacheService(StatelessServiceContext context)
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
            string tweetSubscriberConnectionString = configurationPackage.Settings.Sections["TweetCacheAndMessageBus"].Parameters["ConnectionString"].Value;
            string tweetCacheConnectionString = tweetSubscriberConnectionString;

            using(var tweetCache = new TweetCache(tweetCacheConnectionString))
            using (new TweetSubscriber(tweetSubscriberConnectionString, "wun/fast-path", async t => await tweetCache.SetAsync(t)))
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
