using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System.Threading.Tasks;
using Wun.Backend.TweetModel;
using System.Data;
using System.Data.SqlClient;

namespace TweetStoreService
{
	public class SimpleEventProcessor : IEventProcessor
	{

		public SimpleEventProcessor()
		{

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
				Tweet tweet = Tweet.Create(_tweet);
				StoreTweet(tweet);
			}
			return context.CheckpointAsync();
		}

		private void StoreTweet(Tweet tweet)
		{
			//store the tweet in PAAS DB

			try
			{
				SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
				builder.DataSource = "your_server.database.windows.net";
				builder.UserID = "your_user";
				builder.Password = "your_password";
				builder.InitialCatalog = "your_database";

				SqlConnection connection = new SqlConnection(builder.ConnectionString);
				connection.Open();
				StringBuilder sql = new StringBuilder("insert into tweet_store(username,timestamp,text) values('");
				sql.Append(tweet.ScreenName);
				sql.Append("',");
				sql.Append(tweet.Timestamp.ToString("yyyyMMddHHmmss"));
				sql.Append(",'");
				sql.Append(tweet.Text);
				sql.Append("')");

				SqlCommand command = new SqlCommand(sql.ToString(), connection);
				command.ExecuteNonQuery();

			}
			catch (SqlException e)
			{
				Console.WriteLine(e.ToString());
			}
		}
	}
}
