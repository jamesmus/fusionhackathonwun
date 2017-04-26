namespace Wun.GatewayApi.Service.MessageBus.MessageCommands
{
    public interface IMessageCommand
    {
        void Execute<TTweetMessage>(TTweetMessage tweetMessage);
    }
}
