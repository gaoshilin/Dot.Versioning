namespace Dot.Versioning
{
    public interface IVersioningServiceFactory
    {
        T GetService<T>(string version) where T : IVersioningService;
        T GetService<T>(string name, string version) where T : IVersioningService;
    }
}