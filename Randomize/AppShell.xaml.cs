using Microsoft.Maui.Controls;
using Randomize.View;
using Randomize.ViewModel;

namespace Randomize
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new CalendarViewModel(); 

            Routing.RegisterRoute(nameof(EventDetailPage), typeof(EventDetailPage));
            Routing.RegisterRoute(nameof(UpdateEventPage), typeof(UpdateEventPage));
            Routing.RegisterRoute(nameof(DeleteEventPage), typeof(DeleteEventPage));
        }
    }
}
