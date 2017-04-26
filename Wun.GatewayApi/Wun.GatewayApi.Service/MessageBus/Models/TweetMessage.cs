using System;

namespace Wun.GatewayApi.Service.MessageBus.Models
{
    public class TweetMessage
    {
        public string DisplayName { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}
