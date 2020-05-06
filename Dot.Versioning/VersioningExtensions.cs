using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dot.Versioning
{
    public static class VersioningExtensions
    {
        public static IServiceCollection AddVersioning(this IServiceCollection services, Action<VersioningBuilder> configure = null)
        {
            var builder = new VersioningBuilder(services);
            configure?.Invoke(builder);
            builder.Build();

            return services;
        }
    }
}