using System;
using Cybernetic.Desktop.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Cybernetic.Desktop;

/// <summary>
/// 
/// </summary>
public class CompositionRoot
{
    private static CompositionRoot instance;

    public IServiceProvider ServiceProvider { get; private set; }
    
    /// <summary>
    /// Returns instance of <see cref="CompositionRoot"/>.
    /// </summary>
    /// <returns></returns>
    public static CompositionRoot GetInstance()
    {
        if (instance == null)
        {
            instance = new CompositionRoot();
            instance.Configure();
        }

        return instance;
    }

    private void Configure()
    {
        var services = new ServiceCollection();
        ConfigureService(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    private void ConfigureService(ServiceCollection services)
    {
        DesktopModule.Register(services);
    }
}