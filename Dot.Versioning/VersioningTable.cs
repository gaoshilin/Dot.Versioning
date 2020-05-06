using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Dot.Versioning
{
    public class VersioningTable
    {
        private readonly Dictionary<Type, SortedDictionary<decimal, Type>> _table = new Dictionary<Type, SortedDictionary<decimal, Type>>();

        public IReadOnlyDictionary<Type, SortedDictionary<decimal, Type>> Table => _table;

        public VersioningTable(IServiceCollection services, List<Type> types)
        {
            var allTypes = types.Where(x => typeof(IVersioningService).IsAssignableFrom(x) && x != typeof(IVersioningService)).ToList();

            var interfaceTypes = allTypes.Where(x => x.IsInterface).ToList();

            var versioningTypes = allTypes.Where(x => x.IsClass && !x.IsAbstract && x.GetCustomAttributes<VersioningAttribute>().Any()).ToList();

            foreach (var interfaceType in interfaceTypes)
            {
                var implTypes = versioningTypes.Where(x => interfaceType.IsAssignableFrom(x)).ToList();
                foreach (var implType in implTypes)
                {
                    var versioningAttrs = implType.GetCustomAttributes<VersioningAttribute>();
                    foreach (var versioningAttr in versioningAttrs)
                    {
                        if (!_table.TryGetValue(interfaceType, out SortedDictionary<decimal, Type> versions))
                            versions = _table[interfaceType] = new SortedDictionary<decimal, Type>(new VersionDescendingComparer());

                        var versionValue = GetVersionValue(versioningAttr.Version);
                        versions[versionValue] = implType;

                        var name = $"{implType.Name}:{versionValue}";
                        services.AddNamed(name, implType, versioningAttr.ServiceLifetime);
                    }
                }
            }
        }

        private decimal GetVersionValue(string version, Func<string, Exception> exFactory = null)
        {
            int maxVersionLength = 4; //eg:1.10.12.13
            var unit = 7; //decimal.MaxValue.ToString().Length / maxVersionLength = 7;
            var multiplier = (int)Math.Pow(10, unit);

            try
            {
                var value = 0M;

                if (string.IsNullOrEmpty(version))
                    return value;

                var list = version.Split('.')
                                  .Select(o => Convert.ToUInt32(o))
                                  .ToList();

                if (list.Count > maxVersionLength)
                    throw new ArgumentException();

                if (list.Any(o => o > multiplier * 10))
                    throw new ArgumentException();

                for (var i = 0; i < list.Count; i++)
                {
                    var power = maxVersionLength - i - 1;
                    value += list[i] * (decimal)Math.Pow(multiplier, power);
                }

                return value;
            }
            catch
            {
                if (exFactory != null)
                    throw exFactory(version);
                else
                    throw new ArgumentException($"service version '{version}' error");
            }
        }

        public (Type, decimal) GetServiceType<T>(string version)
            where T : IVersioningService
        {
            var versionValue = GetVersionValue(version);
            if (!_table.TryGetValue(typeof(T), out SortedDictionary<decimal, Type> versions))
                throw new Exception($"Service '{typeof(T).Name}' not found!");

            foreach (var kv in versions)
            {
                if (kv.Key <= versionValue)
                    return (kv.Value, kv.Key);
            }

            return (versions.Last().Value, versions.Last().Key);
        }
    }
}