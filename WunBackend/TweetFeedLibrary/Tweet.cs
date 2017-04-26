using System;

namespace Wun.Backend.TweetFeedHandler
{
    public class Tweet
    {
        public string ScreenName { get; }
        public string Text { get; }

				public DateTime Timestamp { get; }

        public Tweet(string screenName, string text,DateTime timestamp)
        {
            ScreenName = screenName;
            Text = text;
						Timestamp = timestamp;
        }
    }
}