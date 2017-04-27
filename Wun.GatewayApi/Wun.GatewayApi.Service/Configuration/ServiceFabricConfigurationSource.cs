using System.Fabric;
using Microsoft.Extensions.Configuration;

namespace Wun.GatewayApi.Service.Configuration
{
    public class ServiceFabricConfigurationSource : IConfigurationSource
    {
        private readonly ServiceContext _serviceContext;

        public ServiceFabricConfigurationSource(ServiceContext serviceContext)
        {
            _serviceContext = serviceContext;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ServiceFabricConfigurationProvider(_serviceContext);
        }
    }
}
