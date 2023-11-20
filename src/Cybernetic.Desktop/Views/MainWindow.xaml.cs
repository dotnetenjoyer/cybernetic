using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cybernetic.UseCases.Schedules.GenerateSchedule;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cybernetic.Desktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var compositionRoot = CompositionRoot.GetInstance();

            var mediator = compositionRoot.ServiceProvider.GetRequiredService<IMediator>();

            var command = new GenerateScheduleCommand()
            {
                MinLayersCount = 3,
                MaxLayersCount = 3,
                MinTasksCountPerLayer = 4,
                MaxTasksCountPerLayer = 8,
                StartTime = DateTime.Now.Date.AddDays(-5),
                EndTime = DateTime.Now
            };
            
            var schedule = mediator.Send(command).GetAwaiter().GetResult();
        }
    }
}