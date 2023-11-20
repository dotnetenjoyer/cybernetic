using Cybernetic.Infrastructure.Abstraction.Services;
using Cybernetic.Infrastructure.Implementation.Services;
using Cybernetic.UseCases.Schedules.GenerateSchedule;
using Microsoft.Extensions.DependencyInjection;

namespace Cybernetic.Desktop.Infrastructure.DependencyInjection;

/// <summary>
/// Register infrastructure dependencies.
/// </summary>
public class InfrastructureModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services collection.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddMediatR(conf => 
            conf.RegisterServicesFromAssembly(typeof(GenerateScheduleCommand).Assembly));

        services.AddTransient<INameGenerator, NameGenerator>();
    }
}