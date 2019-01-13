using System;
using CalendarViewSample.Components;
using Xamarin.Forms;

namespace CalendarViewSample.Pages {
    public partial class CalendarViewPage : ContentPage {

        readonly CalendarView _calendarView;

        readonly RelativeLayout _relativeLayout;
        readonly StackLayout _stacker;

        public CalendarViewPage() {
            InitializeComponent();

            _relativeLayout = new RelativeLayout {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            Content = _relativeLayout;

            _calendarView = new CalendarView {
                MinDate = CalendarView.FirstDayOfMonth(DateTime.Now),
                MaxDate = CalendarView.LastDayOfMonth(DateTime.Now.AddMonths(6)),
                HighlightedDateBackgroundColor = Color.FromRgb(227, 227, 227),
                ShouldHighlightDaysOfWeekLabels = false,
                SelectionBackgroundStyle = CalendarView.BackgroundStyle.CircleFill,
                TodayBackgroundStyle = CalendarView.BackgroundStyle.CircleOutline,
                HighlightedDaysOfWeek = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday },
                ShowNavigationArrows = true,
                MonthTitleFont = Font.OfSize("Open 24 Display St", NamedSize.Medium)
            };

            _relativeLayout.Children.Add(_calendarView,
                                         Constraint.Constant(0),
                                         Constraint.Constant(0),
                                         Constraint.RelativeToParent(p => p.Width),
                                         Constraint.RelativeToParent(p => p.Height * 2 / 3));

            _stacker = new StackLayout {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            _relativeLayout.Children.Add(_stacker,
                                         Constraint.Constant(0),
                                         Constraint.RelativeToParent(p => p.Height * 2 / 3),
                                         Constraint.RelativeToParent(p => p.Width),
                                         Constraint.RelativeToParent(p => p.Height * 1 / 3)
            );
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _calendarView.DateSelected += HandleOnDateSelected;
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();
            _calendarView.DateSelected -= HandleOnDateSelected;
        }

        void HandleOnDateSelected(object sender, DateTime selectedDate) {
            _stacker.Children.Add(new Label {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Text = string.Format("Date was selected {0:d}", selectedDate)
            });
        }

    }
}
