using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Globalization;
using Randomize.Model; 

namespace Randomize.ViewModel
{
    public partial class CalendarViewModel : ObservableObject
    {
        private DateTime _selectedDate;
        private string _selectedEventDescription;
        private string _eventTitle;
        private string _eventDescription;
        private ObservableCollection<Event> _events;
        private ObservableCollection<int> _availableYears;
        private ObservableCollection<string> _availableMonths;

        public CalendarViewModel()
        {
            _events = new ObservableCollection<Event>();
            Days = new ObservableCollection<DayModel>();
            AvailableYears = new ObservableCollection<int>(Enumerable.Range(1900, DateTime.Now.Year - 1899));
            AvailableMonths = new ObservableCollection<string>(DateTimeFormatInfo.CurrentInfo.MonthNames.Where(m => !string.IsNullOrEmpty(m)));
            InitializeDays();
        }

       
        public ObservableCollection<DayModel> Days { get; set; }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public string SelectedEventDescription
        {
            get => _selectedEventDescription;
            set => SetProperty(ref _selectedEventDescription, value);
        }

        public string EventTitle
        {
            get => _eventTitle;
            set => SetProperty(ref _eventTitle, value);
        }

        public string EventDescription
        {
            get => _eventDescription;
            set => SetProperty(ref _eventDescription, value);
        }

        public ObservableCollection<Event> Events
        {
            get => _events;
            set => SetProperty(ref _events, value);
        }

        public ObservableCollection<int> AvailableYears { get; set; }

        public ObservableCollection<string> AvailableMonths { get; set; }

        public int SelectedYear
        {
            get => SelectedDate.Year;
            set => SetProperty(ref _selectedDate, new DateTime(value, SelectedDate.Month, 1));
        }

        public string SelectedMonth
        {
            get => SelectedDate.ToString("MMMM");
            set
            {
                var monthNumber = DateTime.ParseExact(value, "MMMM", null).Month;
                SetProperty(ref _selectedDate, new DateTime(SelectedDate.Year, monthNumber, 1));
            }
        }

       
        public IRelayCommand AddEventCommand => new RelayCommand(AddEvent);

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
                    Events = eventsForDay,
                    DateTapGestureRecognizer = new TapGestureRecognizer
                    {
                        Command = new Command(() => OnDayClicked(currentDate))
                    }
                };

                Days.Add(day);
            }
        }

        private void OnDayClicked(DateTime date)
        {
            SelectedDate = date;
            ShowEventDetails();
        }

        public void ShowEventDetails()
        {
            var day = Days.FirstOrDefault(d => d.Date.Date == SelectedDate.Date);
            if (day != null && day.Events.Any())
            {
                var eventDescriptions = day.Events.Select(e => e.Description);
                SelectedEventDescription = string.Join(Environment.NewLine, eventDescriptions);
            }
            else
            {
                SelectedEventDescription = string.Empty;
            }
        }

        private void AddEvent()
        {
            if (string.IsNullOrWhiteSpace(EventTitle) || string.IsNullOrWhiteSpace(EventDescription))
                return;

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

        private void SaveEvents()
        {
            try
            {
                var json = JsonSerializer.Serialize(Events);
                File.WriteAllText(GetFilePath(), json);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error saving events: {ex.Message}");
            }
        }

        public void LoadEvents()
        {
            try
            {
                var filePath = GetFilePath();
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var events = JsonSerializer.Deserialize<ObservableCollection<Event>>(json);
                    Events = events ?? new ObservableCollection<Event>();
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
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "events.json");
        }
    }

    public class DayModel : ObservableObject
    {
        public DateTime Date { get; set; }
        public int Day { get; set; }
        public required ObservableCollection<Event> Events { get; set; }
        public required TapGestureRecognizer DateTapGestureRecognizer { get; set; }
    }
}
