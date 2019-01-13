using System;

namespace CalendarViewSample.Droid.Components.Calendar {
    public class DateSelectedEventArgs : EventArgs {

        public DateSelectedEventArgs(DateTime date) {
            SelectedDate = date;
        }

        public DateTime SelectedDate { get; }

    }
}
