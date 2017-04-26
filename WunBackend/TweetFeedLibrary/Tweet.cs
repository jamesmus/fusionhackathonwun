namespace TwitterConsole
{
    public class Tweet
    {
        public string ScreenName { get; }
        public string Text { get; }

        public Tweet(string screenName, string text)
        {
            ScreenName = screenName;
            Text = text;
        }
    }
}