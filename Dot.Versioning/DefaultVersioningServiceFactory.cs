using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dot.Versioning
{
    public class DefaultVersioningServiceFactory : IVersioningServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly VersioningTable _versioningTable;

        public DefaultVersioningServiceFactory(IServiceProvider serviceProvider, VersioningTable versioningTable)
        {
            _serviceProvider = serviceProvider;
            _versioningTable = versioningTable;
        }

        public T GetService<T>(string version)
            where T : IVersioningService
        {
            var (serviceType, versionValue) = _versioningTable.GetServiceType<T>(version);
            var name = $"{serviceType.Name}:{versionValue}";
            return _serviceProvider.GetNamedService<T>(name);
        }
    }
}