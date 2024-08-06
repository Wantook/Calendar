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
            // Check if Event and _events are properly initialized
            if (Event == null || _events == null)
            {
                System.Console.WriteLine("Event or _events is null.");
                return;
            }

            _events.Remove(Event);

            
            SaveEvents();

            
            var calendarViewModel = Shell.Current.BindingContext as CalendarViewModel;
            if (calendarViewModel == null)
            {
                System.Console.WriteLine("CalendarViewModel is null.");
                return;
            }
            calendarViewModel.RemoveEvent(Event);

            
            await Shell.Current.GoToAsync("//CalendarPage");
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("//CalendarPage");
        }

        private void SaveEvents()
        {
            try
            {
                var sb = new System.Text.StringBuilder();
                foreach (var ev in _events)
                {
                    sb.AppendLine($"{ev.Date:yyyy-MM-dd}|{ev.Title}|{ev.Description}");
                }
                System.IO.File.WriteAllText(GetFilePath(), sb.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error saving events: {ex.Message}");
            }
        }

        private string GetFilePath()
        {
            return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "events.txt");
        }
    }
}
