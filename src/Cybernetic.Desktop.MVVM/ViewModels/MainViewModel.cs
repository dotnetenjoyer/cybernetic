using Cybernetic.Desktop.MVVM.Utils;

namespace Cybernetic.Desktop.MVVM.ViewModels;

/// <summary>
/// Main window view model.
/// </summary>
public class MainViewModel : BaseViewModel
{
    /// <summary>
    /// Schedule view model.
    /// </summary>
    public ScheduleViewModel ScheduleViewModel { get; }
    
    /// <summary>
    /// Constructor.
    /// </summary>
    public MainViewModel(ViewModelFactory viewModelFactory)
    {
        ScheduleViewModel = viewModelFactory.Create<ScheduleViewModel>();
    }

    /// <inheritdoc />
    public override Task LoadAsync()
    {
        return ScheduleViewModel.LoadAsync();
    }
}