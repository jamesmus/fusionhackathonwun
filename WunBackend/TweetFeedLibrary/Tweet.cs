using System;
using Newtonsoft.Json;

namespace Wun.Backend.TweetFeedHandler
{
	public class Tweet
	{
		[JsonProperty("screen-name")]
		public string ScreenName { get; }
		[JsonProperty("at")]
		public DateTime Timestamp { get; }
		[JsonProperty("text")]
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
	}
}