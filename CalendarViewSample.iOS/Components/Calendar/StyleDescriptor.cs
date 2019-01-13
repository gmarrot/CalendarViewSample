using CalendarViewSample.Components;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace CalendarViewSample.iOS.Components.Calendar {
    public class StyleDescriptor {

        public UIColor BackgroundColor { get; set; } = Color.FromHex("#FFFFFFFF").ToUIColor();

        public UIColor DateForegroundColor { get; set; } = UIColor.FromRGBA(0.275f, 0.341f, 0.412f, 1f);

        public UIColor DateBackgroundColor { get; set; } = UIColor.White;

        public UIColor InactiveDateForegroundColor { get; set; } = UIColor.LightGray;

        public UIColor InactiveDateBackgroundColor { get; set; } = UIColor.White;

        public UIColor SelectedDateForegroundColor { get; set; } = Color.FromHex("#FFFFFFFF").ToUIColor();

        public UIColor SelectedDateBackgroundColor { get; set; } = Color.FromHex("#FF379BFF").ToUIColor();

        public UIColor TitleForegroundColor { get; set; } = UIColor.DarkGray;

        public UIColor TitleBackgroundColor { get; set; } = UIColor.LightGray;

        public UIColor TodayForegroundColor { get; set; } = UIColor.Red;//Color.FromHex("#ff778088").ToUIColor();

        public UIColor TodayBackgroundColor { get; set; } = UIColor.DarkGray;//Color.FromHex("#ccffcc").ToUIColor();

        public UIColor DayOfWeekLabelForegroundColor { get; set; } = UIColor.White;

        public UIColor DayOfWeekLabelBackgroundColor { get; set; } = UIColor.LightGray;

        public UIColor HighlightedDateForegroundColor { get; set; } = Color.FromHex("#ff778088").ToUIColor();

        public UIColor HighlightedDateBackgroundColor { get; set; } = Color.FromHex("#ccffcc").ToUIColor();

        public UIColor DateSeparatorColor { get; set; } = Color.FromHex("#ffbababa").ToUIColor();

        public CalendarView.BackgroundStyle SelectionBackgroundStyle { get; set; } = CalendarView.BackgroundStyle.Fill;

        public CalendarView.BackgroundStyle TodayBackgroundStyle { get; set; } = CalendarView.BackgroundStyle.Fill;

        public UIFont DateLabelFont { get; set; } = UIFont.BoldSystemFontOfSize(10);

        public UIFont MonthTitleFont { get; set; } = UIFont.BoldSystemFontOfSize(16);

        public bool ShouldHighlightDaysOfWeekLabel { get; set; }

    }
}
