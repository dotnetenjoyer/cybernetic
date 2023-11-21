using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Cybernetic.Desktop.MVVM.Utils;
using Cybernetic.Desktop.MVVM.ViewModels;

namespace Cybernetic.Desktop.Views;

/// <summary>
/// Interaction logic for MainWindow.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Loaded += OnMainWindowLoaded;
    }

    private void OnMainWindowLoaded(object sender, RoutedEventArgs e) 
    { 
        var compositionRoot = CompositionRoot.GetInstance(); 
        var viewModelFactory = compositionRoot.ServiceProvider.GetRequiredService<ViewModelFactory>(); 
        var mainViewModel = viewModelFactory.Create<MainViewModel>(); 
        DataContext = mainViewModel;
        mainViewModel.LoadAsync();
    }
}