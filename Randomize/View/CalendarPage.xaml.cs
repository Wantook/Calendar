using Microsoft.Maui.Controls;
using Randomize.ViewModel;
using System;
using System.Linq;

namespace Randomize.View
{
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            InitializeComponent();
        }

        private void OnDateSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is DayModel selectedDay)
            {
                var viewModel = (CalendarViewModel)BindingContext;
                viewModel.SelectedDate = selectedDay.Date;

                
                viewModel.ShowEventDetails();
            }
        }

        private async void OnEventClicked(object sender, EventArgs e)
        {
            var frame = sender as Frame;

            if (frame != null)
            {
                
                await frame.ScaleTo(1.2, 100, Easing.CubicIn);
                await frame.ScaleTo(1, 100, Easing.CubicOut);
            }
        }
    }
}
