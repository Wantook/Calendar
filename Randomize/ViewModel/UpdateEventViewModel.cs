using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Randomize.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Randomize.ViewModel
{
    public partial class UpdateEventViewModel : ObservableObject
    {
        [ObservableProperty]
        private Event _event;

        private ObservableCollection<Event> _events;

        public UpdateEventViewModel()
        {
        }

        public void InitializeEvents(ObservableCollection<Event> events)
        {
            _events = events;
        }

        [RelayCommand]
        private async Task SaveEventAsync()
        {
            System.Diagnostics.Debug.WriteLine("SaveEventAsync called");
            var existingEvent = _events.FirstOrDefault(e => e.Date == Event.Date && e.Title == Event.Title);
            if (existingEvent != null)
            {
                existingEvent.Title = Event.Title;
                existingEvent.Description = Event.Description;
                OnPropertyChanged(nameof(_events));
            }
            await Shell.Current.GoToAsync("//MainPage"); // Change to your desired page
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("//MainPage"); // Change to your desired page
        }
    }
}
