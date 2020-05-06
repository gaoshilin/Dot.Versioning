using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace Dot.Versioning
{
    public class VersioningBuilder
    {
        private readonly IServiceCollection _services;
        private IVersioningServiceFactory _factory;
        private Func<IServiceProvider, IVersioningServiceFactory> _factoryFactory;

        public VersioningBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public VersioningBuilder UseVersioningFactory(IVersioningServiceFactory factory)
        {
            _factory = factory;
            return this;
        }

        public VersioningBuilder UseVersioningFactory(Func<IServiceProvider, IVersioningServiceFactory> factoryFactory)
        {
            _factoryFactory = factoryFactory;
            return this;
        }

        public void Build()
        {
            // Type取值：project-自建工程 packages-Nuget程序包 referenceassembly-框架程序集
            var libraries = DependencyContext.Default.CompileLibraries.Where(x => x.Type == "project").ToList();

            var types = libraries.Select(x => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(x.Name)))
                                 .SelectMany(x => x.GetTypes())
                                 .ToList();

            _services.AddSingleton(new VersioningTable(_services, types));

            if (_factory != null)
                _services.AddSingleton(_factory);
            else if (_factoryFactory != null)
                _services.AddSingleton(_factoryFactory);
            else
                _services.AddSingleton<IVersioningServiceFactory, DefaultVersioningServiceFactory>();
        }
    }
}