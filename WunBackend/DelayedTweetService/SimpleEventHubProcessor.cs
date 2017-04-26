using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System.Threading.Tasks;
using Wun.Backend.TweetFeedHandler;
using StackExchange.Redis;

namespace DelayedTweetService
{
	public class SimpleEventProcessor : IEventProcessor
	{
		String redisConnectionString = "";
		String redisChannelName="";
		ConnectionMultiplexer redisConnection;

		public SimpleEventProcessor()
		{
			redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);
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

		public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
		{
			foreach (var eventData in messages)
			{
				string _tweet= Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
				Tweet tweet = Tweet.CreateTweet(_tweet);
				if(tweet.Timestamp>DateTime.Now.AddMinutes(-2))
				{
					PublishDelayedTweet(_tweet);
				}
			}
			return context.CheckpointAsync();
		}

		private void PublishDelayedTweet(String tweet)
		{
			redisConnection.GetSubscriber().Publish(redisChannelName, tweet);
		}
	}
}
