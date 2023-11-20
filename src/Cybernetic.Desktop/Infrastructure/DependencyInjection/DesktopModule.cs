using Cybernetic.Desktop.MVVM.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Cybernetic.Desktop.Infrastructure.DependencyInjection;

/// <summary>
/// Register desktop dependencies. 
/// </summary>
public class DesktopModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services collection.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<ViewModelFactory>();
    }
}

