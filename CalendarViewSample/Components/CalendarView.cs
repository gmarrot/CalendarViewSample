using System;
using Xamarin.Forms;

namespace CalendarViewSample.Components {
    public class CalendarView : View {

        public enum BackgroundStyle {
            Fill,
            CircleFill,
            CircleOutline
        }


        public static readonly BindableProperty MinDateProperty =
            BindableProperty.Create(nameof(MinDate), typeof(DateTime), typeof(CalendarView), FirstDayOfMonth(DateTime.Today));

        public static readonly BindableProperty MaxDateProperty =
            BindableProperty.Create(nameof(MaxDate), typeof(DateTime), typeof(CalendarView), LastDayOfMonth(DateTime.Today));

        public static readonly BindableProperty SelectedDateProperty =
            BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(CalendarView), null, BindingMode.TwoWay);

        public static readonly BindableProperty DisplayedMonthProperty =
            BindableProperty.Create(nameof(DisplayedMonth), typeof(DateTime), typeof(CalendarView), DateTime.Now, BindingMode.TwoWay);

        public static readonly BindableProperty DateLabelFontProperty =
            BindableProperty.Create(nameof(DateLabelFont), typeof(Font), typeof(CalendarView), Font.Default);

        public static readonly BindableProperty MonthTitleFontProperty =
            BindableProperty.Create(nameof(MonthTitleFont), typeof(Font), typeof(CalendarView), Font.Default);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty TodayDateForegroundColorProperty =
            BindableProperty.Create(nameof(TodayDateForegroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty TodayDateBackgroundColorProperty =
            BindableProperty.Create(nameof(TodayDateBackgroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty DateForegroundColorProperty =
            BindableProperty.Create(nameof(DateForegroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty DateBackgroundColorProperty =
            BindableProperty.Create(nameof(DateBackgroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty InactiveDateForegroundColorProperty =
            BindableProperty.Create(nameof(InactiveDateForegroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty InactiveDateBackgroundColorProperty =
            BindableProperty.Create(nameof(InactiveDateBackgroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty HighlightedDateForegroundColorProperty =
            BindableProperty.Create(nameof(HighlightedDateForegroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty HighlightedDateBackgroundColorProperty =
            BindableProperty.Create(nameof(HighlightedDateBackgroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty TodayBackgroundStyleProperty =
            BindableProperty.Create(nameof(TodayBackgroundStyle), typeof(BackgroundStyle), typeof(CalendarView), BackgroundStyle.Fill);

        public static readonly BindableProperty SelectionBackgroundStyleProperty =
            BindableProperty.Create(nameof(SelectionBackgroundStyle), typeof(BackgroundStyle), typeof(CalendarView), BackgroundStyle.Fill);

        public static readonly BindableProperty SelectedDateForegroundColorProperty =
            BindableProperty.Create(nameof(SelectedDateForegroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty SelectedDateBackgroundColorProperty =
            BindableProperty.Create(nameof(SelectedDateBackgroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty DayOfWeekLabelForegroundColorProperty =
            BindableProperty.Create(nameof(DayOfWeekLabelForegroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty DayOfWeekLabelBackgroundColorProperty =
            BindableProperty.Create(nameof(DayOfWeekLabelBackgroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty MonthTitleForegroundColorProperty =
            BindableProperty.Create(nameof(MonthTitleForegroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty MonthTitleBackgroundColorProperty =
            BindableProperty.Create(nameof(MonthTitleBackgroundColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty DateSeparatorColorProperty =
            BindableProperty.Create(nameof(DateSeparatorColor), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty ShowNavigationArrowsProperty =
            BindableProperty.Create(nameof(ShowNavigationArrows), typeof(bool), typeof(CalendarView), false);

        public static readonly BindableProperty NavigationArrowsColorProperty =
            BindableProperty.Create(nameof(NavigationArrowsColorProperty), typeof(Color), typeof(CalendarView), Color.Default);

        public static readonly BindableProperty ShouldHighlightDaysOfWeekLabelsProperty =
            BindableProperty.Create(nameof(ShouldHighlightDaysOfWeekLabels), typeof(bool), typeof(CalendarView), false);

        public static readonly BindableProperty HighlightedDaysOfWeekProperty =
            BindableProperty.Create(nameof(HighlightedDaysOfWeek), typeof(DayOfWeek[]), typeof(CalendarView), new DayOfWeek[0]);


        public event EventHandler<DateTime> MonthChanged;
        public event EventHandler<DateTime> DateSelected;


        public CalendarView() {
            if (Device.RuntimePlatform == Device.iOS) {
                HeightRequest = 198 + 20; // This is the size of the original iOS calendar
            } else if (Device.RuntimePlatform == Device.Android) {
                HeightRequest = 300; // This is the size in which Android calendar renders comfortably on most devices
            }
        }


        public DateTime MinDate {
            get { return (DateTime)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }

        public DateTime MaxDate {
            get { return (DateTime)GetValue(MaxDateProperty); }
            set { SetValue(MaxDateProperty, value); }
        }

        public DateTime? SelectedDate {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public DateTime DisplayedMonth {
            get { return (DateTime)GetValue(DisplayedMonthProperty); }
            set { SetValue(DisplayedMonthProperty, value); }
        }

        public Font DateLabelFont {
            get { return (Font)GetValue(DateLabelFontProperty); }
            set { SetValue(DateLabelFontProperty, value); }
        }

        public Font MonthTitleFont {
            get { return (Font)GetValue(MonthTitleFontProperty); }
            set { SetValue(MonthTitleFontProperty, value); }
        }

        public Color TextColor {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public Color TodayDateForegroundColor {
            get { return (Color)GetValue(TodayDateForegroundColorProperty); }
            set { SetValue(TodayDateForegroundColorProperty, value); }
        }

        public Color TodayDateBackgroundColor {
            get { return (Color)GetValue(TodayDateBackgroundColorProperty); }
            set { SetValue(TodayDateBackgroundColorProperty, value); }
        }

        public Color DateForegroundColor {
            get { return (Color)GetValue(DateForegroundColorProperty); }
            set { SetValue(DateForegroundColorProperty, value); }
        }

        public Color DateBackgroundColor {
            get { return (Color)GetValue(DateBackgroundColorProperty); }
            set { SetValue(DateBackgroundColorProperty, value); }
        }

        public Color InactiveDateForegroundColor {
            get { return (Color)GetValue(InactiveDateForegroundColorProperty); }
            set { SetValue(InactiveDateForegroundColorProperty, value); }
        }

        public Color InactiveDateBackgroundColor {
            get { return (Color)GetValue(InactiveDateBackgroundColorProperty); }
            set { SetValue(InactiveDateBackgroundColorProperty, value); }
        }

        public Color HighlightedDateForegroundColor {
            get { return (Color)GetValue(HighlightedDateForegroundColorProperty); }
            set { SetValue(HighlightedDateForegroundColorProperty, value); }
        }

        public Color HighlightedDateBackgroundColor {
            get { return (Color)GetValue(HighlightedDateBackgroundColorProperty); }
            set { SetValue(HighlightedDateBackgroundColorProperty, value); }
        }

        public BackgroundStyle TodayBackgroundStyle {
            get { return (BackgroundStyle)GetValue(TodayBackgroundStyleProperty); }
            set { SetValue(TodayBackgroundStyleProperty, value); }
        }

        public BackgroundStyle SelectionBackgroundStyle {
            get { return (BackgroundStyle)GetValue(SelectionBackgroundStyleProperty); }
            set { SetValue(SelectionBackgroundStyleProperty, value); }
        }

        public Color SelectedDateForegroundColor {
            get { return (Color)GetValue(SelectedDateForegroundColorProperty); }
            set { SetValue(SelectedDateForegroundColorProperty, value); }
        }

        public Color SelectedDateBackgroundColor {
            get { return (Color)GetValue(SelectedDateBackgroundColorProperty); }
            set { SetValue(SelectedDateBackgroundColorProperty, value); }
        }

        public Color DayOfWeekLabelForegroundColor {
            get { return (Color)GetValue(DayOfWeekLabelForegroundColorProperty); }
            set { SetValue(DayOfWeekLabelForegroundColorProperty, value); }
        }

        public Color DayOfWeekLabelBackgroundColor {
            get { return (Color)GetValue(DayOfWeekLabelBackgroundColorProperty); }
            set { SetValue(DayOfWeekLabelBackgroundColorProperty, value); }
        }

        public Color MonthTitleForegroundColor {
            get { return (Color)GetValue(MonthTitleForegroundColorProperty); }
            set { SetValue(MonthTitleForegroundColorProperty, value); }
        }

        public Color MonthTitleBackgroundColor {
            get { return (Color)GetValue(MonthTitleBackgroundColorProperty); }
            set { SetValue(MonthTitleBackgroundColorProperty, value); }
        }

        public Color DateSeparatorColor {
            get { return (Color)GetValue(DateSeparatorColorProperty); }
            set { SetValue(DateSeparatorColorProperty, value); }
        }

        public bool ShowNavigationArrows {
            get { return (bool)GetValue(ShowNavigationArrowsProperty); }
            set { SetValue(ShowNavigationArrowsProperty, value); }
        }

        public Color NavigationArrowsColor {
            get { return (Color)GetValue(NavigationArrowsColorProperty); }
            set { SetValue(NavigationArrowsColorProperty, value); }
        }

        public bool ShouldHighlightDaysOfWeekLabels {
            get { return (bool)GetValue(ShouldHighlightDaysOfWeekLabelsProperty); }
            set { SetValue(ShouldHighlightDaysOfWeekLabelsProperty, value); }
        }

        public DayOfWeek[] HighlightedDaysOfWeek {
            get { return (DayOfWeek[])GetValue(HighlightedDaysOfWeekProperty); }
            set { SetValue(HighlightedDaysOfWeekProperty, value); }
        }

        #region Color helper properties

        internal Color ActualDateBackgroundColor => DateBackgroundColor;

        internal Color ActualDateForegroundColor => (DateForegroundColor != Color.Default) ? DateForegroundColor : TextColor;

        internal Color ActualInactiveDateBackgroundColor => (InactiveDateBackgroundColor != Color.Default) ? InactiveDateBackgroundColor : ActualDateBackgroundColor;

        internal Color ActualInactiveDateForegroundColor => (InactiveDateForegroundColor != Color.Default) ? InactiveDateForegroundColor : ActualDateForegroundColor;

        internal Color ActualTodayDateForegroundColor => (TodayDateForegroundColor != Color.Default) ? TodayDateForegroundColor : ActualDateForegroundColor;

        internal Color ActualTodayDateBackgroundColor => (TodayDateBackgroundColor != Color.Default) ? TodayDateBackgroundColor : ActualDateBackgroundColor;

        internal Color ActualSelectedDateForegroundColor => (SelectedDateForegroundColor != Color.Default) ? SelectedDateForegroundColor : ActualDateForegroundColor;

        internal Color ActualSelectedDateBackgroundColor => (SelectedDateBackgroundColor != Color.Default) ? SelectedDateBackgroundColor : ActualDateBackgroundColor;

        internal Color ActualMonthTitleForegroundColor => (MonthTitleForegroundColor != Color.Default) ? MonthTitleForegroundColor : TextColor;

        internal Color ActualMonthTitleBackgroundColor => (MonthTitleBackgroundColor != Color.Default) ? MonthTitleBackgroundColor : BackgroundColor;

        internal Color ActualDayOfWeekLabelForegroundColor => (DayOfWeekLabelForegroundColor != Color.Default) ? DayOfWeekLabelForegroundColor : TextColor;

        internal Color ActualDayOfWeekLabelBackroundColor => (DayOfWeekLabelBackgroundColor != Color.Default) ? DayOfWeekLabelBackgroundColor : BackgroundColor;

        internal Color ActualNavigationArrowsColor => (NavigationArrowsColor != Color.Default) ? NavigationArrowsColor : ActualMonthTitleForegroundColor;

        internal Color ActualHighlightedDateForegroundColor => HighlightedDateForegroundColor;

        internal Color ActualHighlightedDateBackgroundColor => HighlightedDateBackgroundColor;

        #endregion

        #region Internal event notifiers

        internal void NotifyDisplayedMonthChanged(DateTime date) {
            DisplayedMonth = date;
            MonthChanged?.Invoke(this, date);
        }

        internal void NotifyDateSelected(DateTime dateSelected) {
            SelectedDate = dateSelected;
            DateSelected?.Invoke(this, dateSelected);
        }

        #endregion

        #region Helper methods

        public static DateTime FirstDayOfMonth(DateTime date) {
            return date.AddDays(1 - date.Day);
        }

        public static DateTime LastDayOfMonth(DateTime date) {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        #endregion

    }
}
