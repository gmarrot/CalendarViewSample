using System;

namespace CalendarViewSample.Droid.Components.Calendar {
    public class MonthDescriptor {

        public MonthDescriptor(int month, int year, DateTime date, string label, StyleDescriptor style) {
            Month = month;
            Year = year;
            Date = date;
            Label = label;
            Style = style;
        }

        public int Month { get; }

        public int Year { get; }

        public DateTime Date { get; }

        public string Label { get; }

        public StyleDescriptor Style { get; }

        public override string ToString() {
            return "MonthDescriptor { "
                + "label=" + Label
                + ", month=" + Month
                + ", year=" + Year
                + " }";
        }

    }
}
