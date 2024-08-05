using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Randomize.Model
{
    public class DayModel : ObservableObject
    {
        public DateTime Date { get; set; }
        public int Day { get; set; }
        public ObservableCollection<Event> Events { get; set; } = new ObservableCollection<Event>();
    }
}
