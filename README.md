# Dot.Versioning


1. 安装：Install-Package Dot.Versioning

2. 继承 IVersioningService 接口，如：
```C#
public interface IService : IVersioningService
{
    void Display();
}
```

3. 为接口实现打上标签，如：
```C#
[Versioning(Version = "1.0")]
public class ServiceV1 : IService
{
    public void Display()
    {
        Console.Writeline("IService V1.0");
    }
}

[Versioning(Version = "2.0")]
public class ServiceV2 : IService
{
    public void Display()
    {
        Console.Writeline("IService V2.0");
    }
}
```

4. 在 Startup 中注册 Versioning
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddVersioning();
})
```

5. 在要使用多版本服务的地方注入 IVersioningServiceFactory

6. 使用 IVersioningServiceFactory 获得具体版本的服务。
获取策略：根据版本号向下兼容，例如:
IService 已注入版本号为 1.0 的实现 ServiceV1; 版本号为 2.0 的实现 ServiceV2.
传入目标版本号大于等于 2.0 时将得到 ServiceV2.
传入目标版本号介于 1.0 与 2.0(不包含）时将得到 ServiceV1.
如果目标版本号小于所有已注册版本号（如 0.9）时将返回版本号最小的实现 ServiceV1.
```C#
_serviceFactory.GetService<IService>("0.9").Display();
_serviceFactory.GetService<IService>("1.0").Display();
_serviceFactory.GetService<IService>("2.0").Display();
_serviceFactory.GetService<IService>("9999.9999.9999.9999").Display();
```
