using CommunityToolkit.Mvvm.ComponentModel;
using Randomize.Model;

namespace Randomize.ViewModel
{
    public partial class EventDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private Event? _event;
    }
}
