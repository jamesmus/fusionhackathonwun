using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Wun.GatewayApi.Service.Configuration
{
    public class ServiceFabricConfigurationProvider : ConfigurationProvider
    {
        private readonly ServiceContext _serviceContext;

        public ServiceFabricConfigurationProvider(ServiceContext serviceContext)
        {
            _serviceContext = serviceContext;
        }

        public override void Load()
        {
            var config = _serviceContext.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            foreach (var section in config.Settings.Sections)
            {
                foreach (var parameter in section.Parameters)
                {
                    Data[$"{section.Name}{ConfigurationPath.KeyDelimiter}{parameter.Name}"] = parameter.Value;
                }
            }
        }
    }
}
