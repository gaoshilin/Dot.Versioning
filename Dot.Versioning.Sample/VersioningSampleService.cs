using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dot.Versioning.Sample
{
    public class VersioningSampleService : IHostedService
    {
        private readonly IVersioningServiceFactory _serviceFactory;

        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddVersioning();
                    services.AddHostedService<VersioningSampleService>();
                })
                .RunConsoleAsync();
        }

        public VersioningSampleService(IVersioningServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _serviceFactory.GetService<IService>("0.9").Display();
            _serviceFactory.GetService<IService>("1.0").Display();
            _serviceFactory.GetService<IService>("2.0").Display();
            _serviceFactory.GetService<IService>("9999.9999.9999.9999").Display();

            _serviceFactory.GetService<IAlipayService>("alipay.trade.barcode.pay", "0.9").Display();
            _serviceFactory.GetService<IAlipayService>("alipay.trade.barcode.pay", "1.0").Display();
            _serviceFactory.GetService<IAlipayService>("alipay.trade.barcode.pay", "2.0").Display();
            _serviceFactory.GetService<IAlipayService>("alipay.trade.barcode.pay", "9999.9999.9999.9999").Display();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
