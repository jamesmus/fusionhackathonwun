using System;
using System.Fabric;
using System.Linq;
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
            string connectionString = configurationPackage.Settings.Sections["TweetCacheAndMessageBus"].Parameters["ConnectionString"].Value;


            var publisher = new TweetPublisher(connectionString, _ => "wun/fast-path");
            //var client = new TwitterClient
            //{
            //    // Move along - there's nothing to see here...
            //    ConsumerKey = "eRn4BoNNwCsGmXo8d9avxH1yU",
            //    ConsumerSecret = "qkn9Zhg1cBtXrWEU1u9hFI55mzzOS2ErxLWDd8qsyi1zF6Z478",
            //    Username = "dt07715097",
            //    Password = "mbdt2017"
            //};
            //using ((await client.GetTweetStreamAsync()).Subscribe(async t => await publisher.PublishTweetAsync(t)))
            {
                Random r = new Random();
                while (true)
                {
                    string screenName = new string(Enumerable.Range(0, 10).Select(_ => (char)r.Next('a', 'z')).ToArray());
                    string text = new string(Enumerable.Range(0, 100).Select(_ => (char)r.Next('a', 'z')).ToArray());
                    Tweet t = new Tweet(screenName, text, DateTime.UtcNow);
                    await publisher.PublishTweetAsync(t);
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Delay(TimeSpan.FromMilliseconds(50), cancellationToken);
                }
            }
        }
    }
}
