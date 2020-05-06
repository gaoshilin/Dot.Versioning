namespace Dot.Versioning
{
    public interface IVersioningServiceFactory
    {
        T GetService<T>(string version) where T : IVersioningService;
    }
}