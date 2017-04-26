using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wun.GatewayApi.Service.MessageBus.MessageCommands;
using Wun.GatewayApi.Service.MessageBus.Models;

namespace Wun.GatewayApi.Tests.Stubs
{
    public class TestCommandStub : IMessageCommand
    {
        public string ReadDisplayName { get; set; }
        public string ReadMessage { get; set; }
        public DateTime ReadDatetime { get; set; }

        public void Execute<TTweetMessage>(TTweetMessage tweetMessage)
        {
            if (!(tweetMessage is TweetMessage))
            {
                return;
            }

            var message = tweetMessage as TweetMessage;
            ReadDisplayName = message.DisplayName;
            ReadMessage = message.Message;
            ReadDatetime = message.Created;
        }
    }
}
