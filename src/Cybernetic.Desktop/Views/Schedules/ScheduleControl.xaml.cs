using System.Windows.Controls;

namespace Cybernetic.Desktop.Views.Schedules;

public partial class ScheduleControl : UserControl
{
    public ScheduleControl()
    {
        InitializeComponent();
    }

    private void ScheduleScroll_OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (sender == ScheduleScroll)
        {
            RulerScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
        }
    }
}