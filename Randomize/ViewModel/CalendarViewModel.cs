using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Randomize.Model;
using Randomize.View;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomize.ViewModel
{
    public partial class CalendarViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime selectedDate;

        [ObservableProperty]
        private DayModel selectedDay;

        [ObservableProperty]
        private string eventTitle;

        [ObservableProperty]
        private string eventDescription;

        public ObservableCollection<DayModel> Days { get; private set; }
        public ObservableCollection<Event> Events { get; private set; }
        public ObservableCollection<int> AvailableYears { get; private set; }
        public ObservableCollection<string> AvailableMonths { get; private set; }

        public CalendarViewModel()
        {
            Days = new ObservableCollection<DayModel>();
            Events = new ObservableCollection<Event>();
            AvailableYears = new ObservableCollection<int>(Enumerable.Range(1900, DateTime.Now.Year - 1899));
            AvailableMonths = new ObservableCollection<string>(DateTimeFormatInfo.CurrentInfo.MonthNames.Where(m => !string.IsNullOrEmpty(m)));
            InitializeDays();
            LoadEvents();
        }

        public int SelectedYear
        {
            get => SelectedDate.Year;
            set
            {
                if (value != SelectedDate.Year)
                {
                    var newDate = new DateTime(value, SelectedDate.Month, SelectedDate.Day);
                    SelectedDate = newDate;
                    InitializeDays();
                }
            }
        }

        public string SelectedMonth
        {
            get => SelectedDate.ToString("MMMM");
            set
            {
                if (DateTime.TryParseExact(value, "MMMM", CultureInfo.CurrentCulture, DateTimeStyles.None, out var newDate))
                {
                    if (newDate.Month != SelectedDate.Month)
                    {
                        SelectedDate = new DateTime(SelectedDate.Year, newDate.Month, SelectedDate.Day);
                        InitializeDays();
                    }
                }
            }
        }

        [RelayCommand]
        private void AddEvent()
        {
            if (!string.IsNullOrWhiteSpace(EventTitle) && !string.IsNullOrWhiteSpace(EventDescription))
            {
                var newEvent = new Event
                {
                    Title = EventTitle,
                    Description = EventDescription,
                    Date = SelectedDate
                };

                Events.Add(newEvent);
                SaveEvents();
                InitializeDays();
            }
        }

        [RelayCommand]
        private async Task NavigateToEventDetails(DateTime date)
        {
            var selectedEvent = Events.FirstOrDefault(e => e.Date.Date == date.Date);
            if (selectedEvent != null)
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Event", selectedEvent }
                };
                await Shell.Current.GoToAsync($"{nameof(EventDetailPage)}", true, navigationParameter);
            }
        }

        private void InitializeDays()
        {
            Days.Clear();
            var daysInMonth = DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month);

            for (int i = 1; i <= daysInMonth; i++)
            {
                var currentDate = new DateTime(SelectedDate.Year, SelectedDate.Month, i);
                var eventsForDay = new ObservableCollection<Event>(Events.Where(e => e.Date.Date == currentDate.Date));

                var day = new DayModel
                {
                    Date = currentDate,
                    Day = i,
                    Events = eventsForDay
                };

                Days.Add(day);
            }
        }

        private void SaveEvents()
        {
            try
            {
                var sb = new StringBuilder();
                foreach (var ev in Events)
                {
                    sb.AppendLine($"{ev.Date:yyyy-MM-dd}|{ev.Title}|{ev.Description}");
                }
                File.WriteAllText(GetFilePath(), sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving events: {ex.Message}");
            }
        }

        private void LoadEvents()
        {
            try
            {
                var filePath = GetFilePath();
                if (File.Exists(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 3 && DateTime.TryParse(parts[0], out var date))
                        {
                            Events.Add(new Event
                            {
                                Date = date,
                                Title = parts[1],
                                Description = parts[2]
                            });
                        }
                    }
                    InitializeDays();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading events: {ex.Message}");
            }
        }

        private string GetFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "events.txt");
        }

        internal void ShowEventDetails()
        {
            if (SelectedDay != null)
            {
                var eventDescriptions = SelectedDay.Events.Select(e => e.Description);
                EventDescription = string.Join(Environment.NewLine, eventDescriptions);
            }
            else
            {
                EventDescription = string.Empty;
            }
        }

        public void RemoveEvent(Event eventToRemove)
        {
            var dayModel = Days.FirstOrDefault(d => d.Date.Date == eventToRemove.Date.Date);
            if (dayModel != null)
            {
                var eventInDay = dayModel.Events.FirstOrDefault(e => e.Title == eventToRemove.Title && e.Description == eventToRemove.Description);
                if (eventInDay != null)
                {
                    dayModel.Events.Remove(eventInDay);
                    Events.Remove(eventToRemove);
                    SaveEvents();
                }
            }
        }
    }
}
