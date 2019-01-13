using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AColor = Android.Graphics.Color;
using ATypeface = Android.Graphics.Typeface;

namespace CalendarViewSample.Droid.Components.Calendar {
    public class StyleDescriptor {

        public AColor BackgroundColor { get; set; } = Color.FromHex("#FFFFFFFF").ToAndroid();

        public AColor DateForegroundColor { get; set; } = Color.FromHex("#FF778088").ToAndroid();

        public AColor DateBackgroundColor { get; set; } = Color.FromHex("#FFF5F7F9").ToAndroid();

        public AColor InactiveDateForegroundColor { get; set; } = Color.FromHex("#40778088").ToAndroid();

        public AColor InactiveDateBackgroundColor { get; set; } = Color.FromHex("#FFF5F7F9").ToAndroid();

        public AColor SelectedDateForegroundColor { get; set; } = Color.FromHex("#FFFFFFFF").ToAndroid();

        public AColor SelectedDateBackgroundColor { get; set; } = Color.FromHex("#FF379BFF").ToAndroid();

        public AColor TitleForegroundColor { get; set; } = Color.FromHex("#FF778088").ToAndroid();

        public AColor TitleBackgroundColor { get; set; } = Color.FromHex("#FFFFFFFF").ToAndroid();

        public AColor TodayForegroundColor { get; set; } = Color.FromHex("#FF778088").ToAndroid();

        public AColor TodayBackgroundColor { get; set; } = Color.FromHex("#CCFFCC").ToAndroid();

        public AColor DayOfWeekLabelForegroundColor { get; set; } = Color.FromHex("#FF778088").ToAndroid();

        public AColor DayOfWeekLabelBackgroundColor { get; set; } = Color.FromHex("#FFFFFFFF").ToAndroid();

        public AColor HighlightedDateForegroundColor { get; set; } = Color.FromHex("#FF778088").ToAndroid();

        public AColor HighlightedDateBackgroundColor { get; set; } = Color.FromHex("#CCFFCC").ToAndroid();

        public AColor DateSeparatorColor { get; set; } = Color.FromHex("#FFBABABA").ToAndroid();

        public ATypeface MonthTitleFont { get; set; }

        public ATypeface DateLabelFont { get; set; }

        public bool ShouldHighlightDaysOfWeekLabel { get; set; }

    }
}
