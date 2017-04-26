﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using Wun.GatewayApi.Service.ContractResolvers;
using IMessageBus = Wun.GatewayApi.Service.MessageBus.IMessageBus;
using ISubscriber = Microsoft.AspNetCore.SignalR.Messaging.ISubscriber;

namespace Wun.GatewayApi.Service
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
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
            services.AddCors(options => options.AddPolicy("testPolicy", builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()));

            services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(serviceProvider => ConnectionMultiplexer.Connect(Configuration.GetConnectionString("redis")));
            services.AddSingleton(provider => provider.GetService<IConnectionMultiplexer>().GetSubscriber());
            services.AddSingleton<IMessageBus, MessageBus.MessageBus>();

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
            app.UseCors("testPolicy");
            app.UseMvc();
            app.UseWebSockets();
            app.UseSignalR();
        }
    }
}
