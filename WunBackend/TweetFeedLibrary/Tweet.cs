using System;
using Newtonsoft.Json;

namespace Wun.Backend.TweetFeedHandler
{
	public class Tweet
	{
		[JsonProperty("displayName")]
		public string ScreenName { get; }
		[JsonProperty("created")]
		public DateTime Timestamp { get; }
		[JsonProperty("content")]
		public string Text { get; }
		
		public Tweet(string screenName, string text, DateTime timestamp)
		{
			ScreenName = screenName;
			Text = text;
			Timestamp = timestamp;
		}

		public String SerializeJSON()
		{
			return JsonConvert.SerializeObject(this, 
				new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.IsoDateFormat });
		}

		public static Tweet CreateTweet(String tweet)
		{
			return (Tweet)JsonConvert.DeserializeObject(tweet);
		}
	}
}