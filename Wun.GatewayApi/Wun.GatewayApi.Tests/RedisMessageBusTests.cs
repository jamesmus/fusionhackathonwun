using System;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using StackExchange.Redis;
using Wun.GatewayApi.Service.MessageBus;
using Wun.GatewayApi.Service.MessageBus.MessageCommands;
using Wun.GatewayApi.Service.MessageBus.Models;
using Wun.GatewayApi.Tests.Stubs;

namespace Wun.GatewayApi.Tests
{
    [TestFixture]
    public class RedisMessageBusTests
    {
        [Test]
        public async Task GivenMessageBusAndSubscriptionToRedisQueue_WhenNewTweetIsPublished_ShouldExecuteTheGivenActionForThatMessage()
        {
            //Given
            string subscriptionName = "real-time-tweets";
            Mock<IMessageCommand> messageCommandMock = new Mock<IMessageCommand>();
            ISubscriber redisSubscriberMock = new RedisSubscriberStub();
            IMessageBus messageBus = new MessageBus(redisSubscriberMock);
            await messageBus.SubscribeAsync<TweetMessage>(subscriptionName, messageCommandMock.Object);

            //When
            redisSubscriberMock.Publish(subscriptionName, JsonConvert.SerializeObject(new TweetMessage
            {
                DisplayName = "donaldTrump",
                Content = "Grab her by the *****",
                Created = DateTime.UtcNow
            }));

            //Then
            messageCommandMock.Verify(command => command.Execute(It.IsAny<TweetMessage>()), Times.Once);
        }

        [Test]
        public async Task GivenMessageHasBeenPublished_WhenPickingUpTheMessage_ItContainsProperFields()
        {
            //Given
            string subscriptionName = "real-time-tweets";
            TestCommandStub messageCommandMock = new TestCommandStub();
            ISubscriber redisSubscriberMock = new RedisSubscriberStub();
            IMessageBus messageBus = new MessageBus(redisSubscriberMock);
            await messageBus.SubscribeAsync<TweetMessage>(subscriptionName, messageCommandMock);
            redisSubscriberMock.Publish(subscriptionName, "{\r\n\t\"DisplayName\": \"donaldTrump\",\r\n\t\"Content\": \"test\",\r\n\t\"Created\": \"2017-04-26T10:00:00Z\"\r\n}");

            //When
            //Then
            Assert.That(messageCommandMock.ReadDisplayName, Is.EqualTo("donaldTrump"));
            Assert.That(messageCommandMock.ReadMessage, Is.EqualTo("test"));
            Assert.That(messageCommandMock.ReadDatetime, Is.EqualTo(new DateTime(2017, 4, 26, 10, 0, 0, DateTimeKind.Utc)));
        }
    }
}
