using Wun.Backend.TweetCore;

namespace Wun.Backend.DelayedTweetService
{
    // HACK
    public static class ServiceContext
    {
        public static TweetPublisher TweetPublisher  { get; set; }
    }
}