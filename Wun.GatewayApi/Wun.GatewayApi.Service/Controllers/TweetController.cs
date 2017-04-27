using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.Extensions.Options;
using Wun.GatewayApi.Service.Configuration;
using Wun.GatewayApi.Service.Hubs;
using Wun.GatewayApi.Service.MessageBus;
using Wun.GatewayApi.Service.MessageBus.MessageCommands;
using Wun.GatewayApi.Service.MessageBus.Models;

namespace Wun.GatewayApi.Service.Controllers
{
    public class TweetController : Controller
    {
        private readonly IConnectionManager _connectionManager;

        public TweetController(IConnectionManager connectionManager, IMessageBus messageBus, IOptions<AppSettings> appSettings)
        {
            _connectionManager = connectionManager;
            SubscribeToRedisChannels(messageBus,
                new Tuple<string, Func<IHubContext>>(appSettings.Value.RealTimeTweetRedisChannel,
                    () => _connectionManager.GetHubContext<RealTimeTweetsHub>()),
                new Tuple<string, Func<IHubContext>>(appSettings.Value.DelayedTweetRedisChannel,
                    () => _connectionManager.GetHubContext<DelayedTweetsHub>()));
        }

        [HttpPost("api/tweet/test")]
        public void SendATestTweetToSignalRClients()
        {
            new PushTweetToTheUiCommand(_connectionManager.GetHubContext<RealTimeTweetsHub>()).Execute(new TweetMessage
            {
                Created = DateTime.UtcNow,
                DisplayName = "DonaldTrump",
                Content = "Hey from the test tweet"
            });
        }

        private void SubscribeToRedisChannels(IMessageBus messageBus, params Tuple<string, Func<IHubContext>>[] redisChannelsToHubContexts)
        {
            foreach (var redisChannelWithHubContext in redisChannelsToHubContexts)
            {
                var redisChannel = redisChannelWithHubContext.Item1;
                var hubContext = redisChannelWithHubContext.Item2();
                messageBus.SubscribeAsync<TweetMessage>(redisChannel, new PushTweetToTheUiCommand(hubContext));
            }
        }
    }
}
