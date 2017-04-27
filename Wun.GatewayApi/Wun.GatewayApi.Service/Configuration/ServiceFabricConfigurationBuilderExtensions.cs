using System.Fabric;
using Microsoft.Extensions.Configuration;

namespace Wun.GatewayApi.Service.Configuration
{
    public static class ServiceFabricConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddServiceFabricConfiguration(this IConfigurationBuilder builder, ServiceContext serviceContext)
        {
            builder.Add(new ServiceFabricConfigurationSource(serviceContext));
            return builder;
        }
    }
}
