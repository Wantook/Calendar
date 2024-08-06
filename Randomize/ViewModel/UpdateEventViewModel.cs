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
            try
            {
                var existingEvent = _events.FirstOrDefault(e => e.Date == Event.Date && e.Title == Event.Title);
                if (existingEvent != null)
                {
                    existingEvent.Title = Event.Title;
                    existingEvent.Description = Event.Description;
                }

                
                SaveEvents();

                
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine($"Error saving event: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            
            await Shell.Current.GoToAsync("..");
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
                System.Diagnostics.Debug.WriteLine($"Error saving events: {ex.Message}");
            }
        }

        private string GetFilePath()
        {
            return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "events.txt");
        }
    }
}
