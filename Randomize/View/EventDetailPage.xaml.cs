using Microsoft.Maui.Controls;
using Randomize.Model;
using Randomize.ViewModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Randomize.View
{
    [QueryProperty(nameof(Event), "Event")]
    [QueryProperty(nameof(Events), "Events")]
    public partial class EventDetailPage : ContentPage
    {
        public Event Event { get; set; }
        public ObservableCollection<Event> Events { get; set; }

        public EventDetailPage()
        {
            InitializeComponent();
            BindingContext = new EventDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is EventDetailViewModel viewModel)
            {
                if (Event != null)
                {
                    viewModel.Event = Event;
                }

                if (Events != null)
                {
                    viewModel.InitializeEvents(Events);
                }
            }
        }
    }
}
