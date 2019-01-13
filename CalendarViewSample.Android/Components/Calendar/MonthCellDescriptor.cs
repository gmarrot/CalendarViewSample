using System;

namespace CalendarViewSample.Droid.Components.Calendar {
    public class MonthCellDescriptor : Java.Lang.Object {

        public MonthCellDescriptor(DateTime date, bool isCurrentMonth, bool isSelectable, bool isSelected,
                                   bool isToday, bool isHighlighted, int value, RangeState rangeState, StyleDescriptor style) {
            DateTime = date;
            Value = value;
            IsCurrentMonth = isCurrentMonth;
            IsSelected = isSelected;
            IsHighlighted = isHighlighted;
            IsToday = isToday;
            IsSelectable = isSelectable;
            RangeState = rangeState;
            Style = style;
        }

        public DateTime DateTime { get; set; }

        public int Value { get; set; }

        public bool IsCurrentMonth { get; set; }

        public bool IsSelected { get; set; }

        public bool IsToday { get; set; }

        public bool IsSelectable { get; set; }

        public bool IsHighlighted { get; set; }

        public RangeState RangeState { get; set; }

        public StyleDescriptor Style { get; set; }

        public override string ToString() {
            return "MonthCellDescriptor { "
                + "Date=" + DateTime
                + ", Value=" + Value
                + ", IsCurrentMonth=" + IsCurrentMonth
                + ", IsSelected=" + IsSelected
                + ", IsToday=" + IsToday
                + ", IsSelectable=" + IsSelectable
                + ", IsHighlighted=" + IsHighlighted
                + ", RangeSTate=" + RangeState
                + " }";
        }

    }
}
