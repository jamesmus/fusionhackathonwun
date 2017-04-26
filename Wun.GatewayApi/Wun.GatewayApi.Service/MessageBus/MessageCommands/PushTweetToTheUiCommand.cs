using Microsoft.AspNetCore.SignalR;

namespace Wun.GatewayApi.Service.MessageBus.MessageCommands
{
    public class PushTweetToTheUiCommand : IMessageCommand
    {
        private readonly IHubContext _hubContext;

        public PushTweetToTheUiCommand(IHubContext hubContext)
        {
            _hubContext = hubContext;
        }

        public void Execute<TTweetMessage>(TTweetMessage tweetMessage)
        {
            _hubContext.Clients.All.newTweet(tweetMessage);
        }
    }
}
