using Microsoft.Maui.Controls;
using Randomize.View;

namespace Randomize
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EventDetailPage), typeof(EventDetailPage));
        }
    }
}
