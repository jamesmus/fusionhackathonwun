using System;
using Microsoft.AspNetCore.Mvc;
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
            messageBus.SubscribeAsync<TweetMessage>(appSettings.Value.RealTimeTweetRedisChannel,
                new PushTweetToTheUiCommand(_connectionManager.GetHubContext<RealTimeTweetsHub>()));
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
    }
}
