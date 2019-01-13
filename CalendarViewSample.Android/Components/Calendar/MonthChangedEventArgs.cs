using System;

namespace CalendarViewSample.Droid.Components.Calendar {
    public class MonthChangedEventArgs : EventArgs {

        public MonthChangedEventArgs(DateTime date) {
            DisplayedMonth = date;
        }

        public DateTime DisplayedMonth { get; }

    }
}
