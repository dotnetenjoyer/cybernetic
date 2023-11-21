using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Cybernetic.Desktop.MVVM.ViewModels;

/// <summary>
/// Base view model.s
/// </summary>
public class BaseViewModel : ObservableObject
{
    /// <summary>
    /// Indicates whether a view model busy.
    /// </summary>
    public virtual bool IsBusy { get; protected set; }
    
    /// <summary>
    /// Loading view model data.
    /// </summary>
    public virtual Task LoadAsync()
    {
        return Task.CompletedTask;
    }
}