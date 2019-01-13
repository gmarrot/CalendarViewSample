using System;
using System.ComponentModel;
using Android.Content;
using Android.Views;
using CalendarViewSample.Components;
using CalendarViewSample.Droid.Components;
using CalendarViewSample.Droid.Components.Calendar;
using CalendarViewSample.Droid.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AColor = Android.Graphics.Color;
using ARelativeLayout = Android.Widget.RelativeLayout;
using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(CalendarView), typeof(CalendarViewRenderer))]
namespace CalendarViewSample.Droid.Components {
    public class CalendarViewRenderer : ViewRenderer<CalendarView, ARelativeLayout> {

        AView _containerView;

        bool _isElementChanging;

        CalendarPickerView _picker;

        CalendarArrowView _leftArrow;
        CalendarArrowView _rightArrow;

        public CalendarViewRenderer(Context context) : base(context) {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CalendarView> e) {
            base.OnElementChanged(e);

            if (e.OldElement == null) {
                var inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                _containerView = inflater.Inflate(Resource.Layout.calendar_picker, null);

                _picker = _containerView.FindViewById<CalendarPickerView>(Resource.Id.calendar_view);
                _picker.Init(Element.MinDate, Element.MaxDate, Element.HighlightedDaysOfWeek);
                _picker.OnDateSelected += (object sender, DateSelectedEventArgs evt) => {
                    ProtectFromEventCycle(() => { Element.NotifyDateSelected(evt.SelectedDate); });
                };
                _picker.OnMonthChanged += (object sender, MonthChangedEventArgs mch) => {
                    SetNavigationArrows();
                    ProtectFromEventCycle(() => { Element.NotifyDisplayedMonthChanged(mch.DisplayedMonth); });
                };
                SetDisplayedMonth(Element.DisplayedMonth);
                SetNavigationArrows();
                SetColors();
                SetFonts();
                SetNativeControl((ARelativeLayout)_containerView);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
            base.OnElementPropertyChanged(sender, e);

            ProtectFromEventCycle(() => {
                if (e.PropertyName == CalendarView.DisplayedMonthProperty.PropertyName) {
                    SetDisplayedMonth(Element.DisplayedMonth);
                }
            });
        }

        void ProtectFromEventCycle(Action action) {
            if (!_isElementChanging) {
                _isElementChanging = true;
                action.Invoke();
                _isElementChanging = false;
            }
        }

        void SetDisplayedMonth(DateTime newMonth) {
            if (newMonth >= CalendarView.FirstDayOfMonth(Element.MinDate) &&
                newMonth <= CalendarView.LastDayOfMonth(Element.MaxDate)) {
                var index = newMonth.Month - Element.MinDate.Month + 12 * (newMonth.Year - Element.MinDate.Year);
                SelectMonth(index, false);
            } else {
                throw new Exception("Month must be between MinDate and MaxDate");
            }
        }

        void SetNavigationArrows() {
            if (_leftArrow == null) {
                _leftArrow = _containerView.FindViewById<CalendarArrowView>(Resource.Id.left_arrow);
                _leftArrow.Click += (object sender, EventArgs e) => { SelectMonth(_picker.CurrentItem - 1, true); };
            }
            if (_rightArrow == null) {
                _rightArrow = _containerView.FindViewById<CalendarArrowView>(Resource.Id.right_arrow);
                _rightArrow.Direction = CalendarArrowView.ArrowDirection.Right;
                _rightArrow.Click += (object sender, EventArgs e) => { SelectMonth(_picker.CurrentItem + 1, true); };
            }
            _leftArrow.SetBackgroundColor(AColor.Transparent);
            _rightArrow.SetBackgroundColor(AColor.Transparent);
            if (Element.ShowNavigationArrows) {
                _rightArrow.Visibility = _picker.CurrentItem + 1 != _picker.MonthCount ? ViewStates.Visible : ViewStates.Invisible;
                _leftArrow.Visibility = _picker.CurrentItem != 0 ? ViewStates.Visible : ViewStates.Invisible;
            } else {
                _leftArrow.Visibility = ViewStates.Gone;
                _rightArrow.Visibility = ViewStates.Gone;
            }
        }

        void SelectMonth(int monthIndex, bool animated) {
            if (monthIndex >= 0 && monthIndex < _picker.MonthCount) {
                _picker.ScrollToSelectedMonth(monthIndex, animated);
            }
        }

        void SetFonts() {
            if (Element.DateLabelFont != Font.Default) {
                _picker.StyleDescriptor.DateLabelFont = Element.DateLabelFont.ToExtendedTypeface(Context);
            }
            if (Element.MonthTitleFont != Font.Default) {
                _picker.StyleDescriptor.MonthTitleFont = Element.MonthTitleFont.ToExtendedTypeface(Context);
            }
        }

        void SetColors() {
            if (Element.BackgroundColor != Color.Default) {
                var andColor = Element.BackgroundColor.ToAndroid();
                _containerView.SetBackgroundColor(andColor);
                _picker.SetBackgroundColor(andColor);
                _picker.StyleDescriptor.BackgroundColor = andColor;
            }

            //Month title
            if (Element.ActualMonthTitleBackgroundColor != Color.Default)
                _picker.StyleDescriptor.TitleBackgroundColor = Element.ActualMonthTitleBackgroundColor.ToAndroid();
            if (Element.ActualMonthTitleForegroundColor != Color.Default)
                _picker.StyleDescriptor.TitleForegroundColor = Element.ActualMonthTitleForegroundColor.ToAndroid();

            //Navigation color arrows
            if (Element.ActualNavigationArrowsColor != Color.Default) {
                _leftArrow.Color = Element.ActualNavigationArrowsColor.ToAndroid();
                _rightArrow.Color = Element.ActualNavigationArrowsColor.ToAndroid();
            } else {
                _leftArrow.Color = _picker.StyleDescriptor.TitleForegroundColor;
                _rightArrow.Color = _picker.StyleDescriptor.TitleForegroundColor;
            }

            //Day of week label
            if (Element.ActualDayOfWeekLabelBackroundColor != Color.Default) {
                var andColor = Element.ActualDayOfWeekLabelBackroundColor.ToAndroid();
                _picker.StyleDescriptor.DayOfWeekLabelBackgroundColor = andColor;
            }
            if (Element.ActualDayOfWeekLabelForegroundColor != Color.Default) {
                var andColor = Element.ActualDayOfWeekLabelForegroundColor.ToAndroid();
                _picker.StyleDescriptor.DayOfWeekLabelForegroundColor = andColor;
            }

            _picker.StyleDescriptor.ShouldHighlightDaysOfWeekLabel = Element.ShouldHighlightDaysOfWeekLabels;

            //Default date color
            if (Element.ActualDateBackgroundColor != Color.Default) {
                var andColor = Element.ActualDateBackgroundColor.ToAndroid();
                _picker.StyleDescriptor.DateBackgroundColor = andColor;
            }
            if (Element.ActualDateForegroundColor != Color.Default) {
                var andColor = Element.ActualDateForegroundColor.ToAndroid();
                _picker.StyleDescriptor.DateForegroundColor = andColor;
            }

            //Inactive Default date color
            if (Element.ActualInactiveDateBackgroundColor != Color.Default) {
                var andColor = Element.ActualInactiveDateBackgroundColor.ToAndroid();
                _picker.StyleDescriptor.InactiveDateBackgroundColor = andColor;
            }
            if (Element.ActualInactiveDateForegroundColor != Color.Default) {
                var andColor = Element.ActualInactiveDateForegroundColor.ToAndroid();
                _picker.StyleDescriptor.InactiveDateForegroundColor = andColor;
            }

            //Today date color
            if (Element.ActualTodayDateBackgroundColor != Color.Default) {
                var andColor = Element.ActualTodayDateBackgroundColor.ToAndroid();
                _picker.StyleDescriptor.TodayBackgroundColor = andColor;
            }
            if (Element.ActualTodayDateForegroundColor != Color.Default) {
                var andColor = Element.ActualTodayDateForegroundColor.ToAndroid();
                _picker.StyleDescriptor.TodayForegroundColor = andColor;
            }

            //Highlighted date color
            if (Element.ActualHighlightedDateBackgroundColor != Color.Default) {
                var andColor = Element.ActualHighlightedDateBackgroundColor.ToAndroid();
                _picker.StyleDescriptor.HighlightedDateBackgroundColor = andColor;
            }
            if (Element.ActualHighlightedDateForegroundColor != Color.Default) {
                var andColor = Element.ActualHighlightedDateForegroundColor.ToAndroid();
                _picker.StyleDescriptor.HighlightedDateForegroundColor = andColor;
            }


            //Selected date
            if (Element.ActualSelectedDateBackgroundColor != Color.Default)
                _picker.StyleDescriptor.SelectedDateBackgroundColor = Element.ActualSelectedDateBackgroundColor.ToAndroid();
            if (Element.ActualSelectedDateForegroundColor != Color.Default)
                _picker.StyleDescriptor.SelectedDateForegroundColor = Element.ActualSelectedDateForegroundColor.ToAndroid();

            //Divider
            if (Element.DateSeparatorColor != Color.Default)
                _picker.StyleDescriptor.DateSeparatorColor = Element.DateSeparatorColor.ToAndroid();
        }

    }
}
