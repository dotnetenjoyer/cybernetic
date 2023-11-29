using System.Windows.Controls;

namespace Cybernetic.Desktop.Views.Schedules;

public partial class ScheduleControl : UserControl
{
    public ScheduleControl()
    {
        InitializeComponent();
    }

    private void HandelScheduleScrollChange(object sender, ScrollChangedEventArgs eventArgs)
    {
        RulerScroll.ScrollToHorizontalOffset(eventArgs.HorizontalOffset);
    }
}