using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Randomize.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Randomize.ViewModel
{
    public partial class DeleteEventViewModel : ObservableObject
    {
        [ObservableProperty]
        private Event _event;

        private ObservableCollection<Event> _events;

        public DeleteEventViewModel()
        {
        }

        public void InitializeEvents(ObservableCollection<Event> events)
        {
            _events = events;
        }

        [RelayCommand]
        private async Task DeleteEventAsync()
        {
            System.Diagnostics.Debug.WriteLine("DeleteEventAsync called");
            _events.Remove(Event);
            await Shell.Current.GoToAsync("//MainPage"); // Change to your desired page
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("//MainPage"); // Change to your desired page
        }
    }
}
