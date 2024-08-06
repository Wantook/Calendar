using Microsoft.Maui.Controls;
using Randomize.Model;
using Randomize.ViewModel;

namespace Randomize.View
{
    [QueryProperty(nameof(Event), "Event")]
    public partial class EventDetailPage : ContentPage
    {
        public EventDetailPage()
        {
            InitializeComponent();
        }

        public Event Event
        {
            set
            {
                BindingContext = new EventDetailViewModel { Event = value };
            }
        }
    }
}
