using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Randomize.Model;
using Randomize.View;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Randomize.ViewModel
{
    public partial class EventDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private Event _event;

        private ObservableCollection<Event> _events;

        public EventDetailViewModel()
        {
        }

        public void InitializeEvents(ObservableCollection<Event> events)
        {
            _events = events;
        }

        [RelayCommand]
        private async Task NavigateToUpdateAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Navigating to UpdateEventPage...");
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Event", Event },
                    { "Events", _events }
                };
                await Shell.Current.GoToAsync($"{nameof(UpdateEventPage)}", true, navigationParameter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error navigating to UpdateEventPage: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task NavigateToDeleteAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Navigating to DeleteEventPage...");
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Event", Event },
                    { "Events", _events }
                };
                await Shell.Current.GoToAsync($"{nameof(DeleteEventPage)}", true, navigationParameter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error navigating to DeleteEventPage: {ex.Message}");
            }
        }
    }
}
