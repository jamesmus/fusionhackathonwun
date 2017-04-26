using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Wun.GatewayApi.Service.MessageBus;
using Wun.GatewayApi.Service.MessageBus.MessageCommands;
using Wun.GatewayApi.Service.MessageBus.Models;

namespace Wun.GatewayApi.Service.Controllers
{
    public class TweetController : Controller
    {
        private readonly IConnectionManager _connectionManager;
        private const string RealTimeTweetsSubscription = "real-time-tweets";

        public TweetController(IConnectionManager connectionManager, IMessageBus messageBus)
        {
            _connectionManager = connectionManager;
            messageBus.Subscribe<TweetMessage>(RealTimeTweetsSubscription,
                new PushTweetToTheUiCommand(_connectionManager.GetHubContext<RealTimeTweetsHub>()));
        }
    }
}
