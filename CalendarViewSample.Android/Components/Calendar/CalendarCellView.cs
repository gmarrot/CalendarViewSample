using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace CalendarViewSample.Droid.Components.Calendar {
    public class CalendarCellView : TextView {

        public CalendarCellView(Context context) : base(context) {
        }

        public CalendarCellView(Context context, IAttributeSet attrs) : base(context, attrs) {
        }

        public CalendarCellView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) {
        }

        public CalendarCellView(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) {
        }

        public bool Selectable { get; set; }

        public bool IsCurrentMonth { get; set; }

        public bool IsToday { get; set; }

        public bool IsHighlighted { get; set; }

        public RangeState RangeState { get; set; }

        public void SetStyle(StyleDescriptor style) {
            if (style.DateLabelFont != null) {
                Typeface = (style.DateLabelFont);
            }
            if (Selected) {
                SetBackgroundColor(style.SelectedDateBackgroundColor);
                SetTextColor(style.SelectedDateForegroundColor);
            } else if (IsToday) {
                SetBackgroundColor(style.TodayBackgroundColor);
                SetTextColor(style.TodayForegroundColor);
            } else if (IsHighlighted) {
                SetBackgroundColor(style.HighlightedDateBackgroundColor);
                if (IsCurrentMonth) {
                    SetTextColor(style.HighlightedDateForegroundColor);
                } else {
                    SetTextColor(style.InactiveDateForegroundColor);
                }
            } else if (!IsCurrentMonth) {
                SetBackgroundColor(style.InactiveDateBackgroundColor);
                SetTextColor(style.InactiveDateForegroundColor);
            } else {
                SetBackgroundColor(style.DateBackgroundColor);
                SetTextColor(style.DateForegroundColor);
            }
        }

    }
}
