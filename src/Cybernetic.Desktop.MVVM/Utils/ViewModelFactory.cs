using Microsoft.Extensions.DependencyInjection;

namespace Cybernetic.Desktop.MVVM.Utils;

public class ViewModelFactory
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="serviceProvider"></param>
    public ViewModelFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Create a new instance of view model.
    /// </summary>
    /// <param name="parameters">View model parameters.</param>
    /// <typeparam name="T">Type of view model.</typeparam>
    /// <returns>New view model instance.</returns>
    public T Create<T>(params object[] parameters)
    {
        return ActivatorUtilities.CreateInstance<T>(serviceProvider, parameters);
    }
}