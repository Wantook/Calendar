using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using Randomize.Model;

namespace Randomize.ViewModel
{
    public partial class CalendarViewModel : ObservableObject
    {
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private string _eventTitle;
        public string EventTitle
        {
            get => _eventTitle;
            set => SetProperty(ref _eventTitle, value);
        }

        private string _eventDescription;
        public string EventDescription
        {
            get => _eventDescription;
            set => SetProperty(ref _eventDescription, value);
        }

        public ObservableCollection<Event> Events { get; set; }

        public CalendarViewModel()
        {
            Events = new ObservableCollection<Event>();
            SelectedDate = DateTime.Today;
        }

        [RelayCommand]
        public void AddEvent()
        {
            Events.Add(new Event
            {
                Date = SelectedDate,
                Title = EventTitle,
                Description = EventDescription
            });

            EventTitle = string.Empty;
            EventDescription = string.Empty;
        }
    }
}
