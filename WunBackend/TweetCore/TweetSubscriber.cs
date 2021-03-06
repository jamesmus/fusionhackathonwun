﻿using System;
using StackExchange.Redis;
using Wun.Backend.TweetModel;

namespace Wun.Backend.TweetCore
{
    public class TweetSubscriber : IDisposable
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly ISubscriber _redisSubscriber;

        public TweetSubscriber(string connectionString, string channelName, Action<Tweet> callback)
        {
            _redisConnection = ConnectionMultiplexer.Connect(connectionString);
            _redisSubscriber = _redisConnection.GetSubscriber();
            _redisSubscriber.Subscribe(channelName, (channel, value) => callback(Tweet.Create(value.ToString())));
        }

        public void Dispose()
        {
            _redisSubscriber?.UnsubscribeAll();
            _redisConnection?.Dispose();
        }
    }
}