using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Wun.Backend.TweetCore;
using Wun.Backend.TweetModel;

namespace Wun.Backend.DelayedTweetService
{
    public class DelayedTweetProcessor : IEventProcessor
    {
        private readonly TweetPublisher _tweetPublisher;

        public DelayedTweetProcessor()
        {
            _tweetPublisher = ServiceContext.TweetPublisher;
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            return null;
        }

        public Task OpenAsync(PartitionContext context)
        {
            return null;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            return null;
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                var tweetString = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                Tweet tweet = Tweet.Create(tweetString);
                TimeSpan delay = tweet.Timestamp - DateTime.UtcNow.AddMinutes(-2);
                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay(delay);
                }
                await _tweetPublisher.PublishTweetAsync(tweet);
            }
            await context.CheckpointAsync();
        }
    }
}
