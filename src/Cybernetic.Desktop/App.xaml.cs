using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Cybernetic.Desktop.Views;

namespace Cybernetic.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs eventArgs)
        {
            var compositionRoot = CompositionRoot.GetInstance();

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}