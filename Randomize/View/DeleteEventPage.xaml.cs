using Microsoft.Maui.Controls;
using Randomize.Model;
using Randomize.ViewModel;
using System.Collections.ObjectModel;

namespace Randomize.View
{
    [QueryProperty(nameof(Event), "Event")]
    [QueryProperty(nameof(Events), "Events")]
    public partial class DeleteEventPage : ContentPage
    {
        public Event Event { get; set; }
        public ObservableCollection<Event> Events { get; set; }

        public DeleteEventPage()
        {
            InitializeComponent();
            BindingContext = new DeleteEventViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is DeleteEventViewModel viewModel)
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
