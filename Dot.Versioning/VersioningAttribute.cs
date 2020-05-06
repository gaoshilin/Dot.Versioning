using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dot.Versioning
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class VersioningAttribute : Attribute
    {
        public string Named { get; set; } = null;
        public string Version { get; set; }
        public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Scoped;
    }
}