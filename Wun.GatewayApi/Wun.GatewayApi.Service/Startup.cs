using System.Fabric;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using Wun.GatewayApi.Service.Configuration;
using Wun.GatewayApi.Service.ContractResolvers;
using IMessageBus = Wun.GatewayApi.Service.MessageBus.IMessageBus;

namespace Wun.GatewayApi.Service
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, StatelessServiceContext serviceContext)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddServiceFabricConfiguration(serviceContext)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

	        var settings = new JsonSerializerSettings
	        {
		        ContractResolver = new SignalRContractResolver()
	        };

	        var serializer = JsonSerializer.Create(settings);
	        services.Add(new ServiceDescriptor(typeof(JsonSerializer),
		        provider => serializer,
		        ServiceLifetime.Transient));

			services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(serviceProvider => ConnectionMultiplexer.Connect(Configuration.GetSection("GatewayApi")["RedisConnection"]));
            services.AddSingleton(provider => provider.GetService<IConnectionMultiplexer>().GetSubscriber());
            services.AddSingleton<IMessageBus, MessageBus.MessageBus>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddSignalR(options =>
            {
                options.Hubs.EnableDetailedErrors = true;
                options.EnableJSONP = true;
                options.Hubs.EnableJavaScriptProxies = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseCors(builder =>
            {
	            builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
            });
            app.UseMvc();
            app.UseWebSockets();
            app.UseSignalR();
        }
    }
}
