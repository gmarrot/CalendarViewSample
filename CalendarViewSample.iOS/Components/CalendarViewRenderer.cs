using System;
using System.ComponentModel;
using CalendarViewSample.Components;
using CalendarViewSample.iOS.Components;
using CalendarViewSample.iOS.Components.Calendar;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CalendarView), typeof(CalendarViewRenderer))]
namespace CalendarViewSample.iOS.Components {
    public class CalendarViewRenderer : ViewRenderer<CalendarView, CalendarMonthView> {

        readonly object elementLock = new object();
        bool isElementChanging;
        bool disposed;

        public CalendarViewRenderer() {
            isElementChanging = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CalendarView> e) {
            base.OnElementChanged(e);

            if (e.NewElement == null) return;

            if (Control == null) {
                var calendarView = new CalendarMonthView(DateTime.MinValue, true, e.NewElement.ShowNavigationArrows);
                SetNativeControl(calendarView);
                calendarView.OnDateSelected += OnDateSelected;
                calendarView.MonthChanged += MonthChanged;
            }

            Control.HighlightDaysOfWeeks(e.NewElement.HighlightedDaysOfWeek);
            SetColors();
            SetFonts();

            Control.SetMinAllowedDate(e.NewElement.MinDate);
            Control.SetMaxAllowedDate(e.NewElement.MaxDate);
            Control.SetDisplayedMonthYear(e.NewElement.DisplayedMonth, false);
        }

        void MonthChanged(DateTime dateTime) {
            ProtectFromEventCycle(() => {
                Element.NotifyDisplayedMonthChanged(dateTime);
            });
        }

        void OnDateSelected(DateTime dateTime) {
            ProtectFromEventCycle(() => {
                Element.NotifyDateSelected(dateTime);
            });
        }

        void ProtectFromEventCycle(Action action) {
            bool changing;

            lock (elementLock) {
                changing = isElementChanging;
            }

            if (changing) return;

            try {
                isElementChanging = true;
                action();
            } finally {
                isElementChanging = false;
            }
        }


        void SetFonts() {
            if (Element.DateLabelFont != Font.Default) {
                Control.StyleDescriptor.DateLabelFont = Element.DateLabelFont.ToUIFont();
            }
            if (Element.MonthTitleFont != Font.Default) {
                Control.StyleDescriptor.MonthTitleFont = Element.MonthTitleFont.ToUIFont();
            }
        }

        void SetColors() {
            if (Element.BackgroundColor != Color.Default) {
                BackgroundColor = Element.BackgroundColor.ToUIColor();
                Control.BackgroundColor = Element.BackgroundColor.ToUIColor();
                Control.StyleDescriptor.BackgroundColor = Element.BackgroundColor.ToUIColor();
            }

            //Month title
            if (Element.ActualMonthTitleBackgroundColor != Color.Default)
                Control.StyleDescriptor.TitleBackgroundColor = Element.ActualMonthTitleBackgroundColor.ToUIColor();
            if (Element.ActualMonthTitleForegroundColor != Color.Default)
                Control.StyleDescriptor.TitleForegroundColor = Element.ActualMonthTitleForegroundColor.ToUIColor();

            //Navigation color arrows
            //          if(Element.ActualNavigationArrowsColor != Color.Default){
            //              _leftArrow.Color = Element.ActualNavigationArrowsColor.ToAndroid();
            //              _rightArrow.Color = Element.ActualNavigationArrowsColor.ToAndroid();
            //          }else{
            //              _leftArrow.Color = Control.StyleDescriptor.TitleForegroundColor;
            //              _rightArrow.Color = Control.StyleDescriptor.TitleForegroundColor;
            //          }

            //Day of week label
            if (Element.ActualDayOfWeekLabelBackroundColor != Color.Default) {
                Control.StyleDescriptor.DayOfWeekLabelBackgroundColor = Element.ActualDayOfWeekLabelBackroundColor.ToUIColor();
            }
            if (Element.ActualDayOfWeekLabelForegroundColor != Color.Default) {
                Control.StyleDescriptor.DayOfWeekLabelForegroundColor = Element.ActualDayOfWeekLabelForegroundColor.ToUIColor();
            }

            Control.StyleDescriptor.ShouldHighlightDaysOfWeekLabel = Element.ShouldHighlightDaysOfWeekLabels;

            //Default date color
            if (Element.ActualDateBackgroundColor != Color.Default) {
                Control.StyleDescriptor.DateBackgroundColor = Element.ActualDateBackgroundColor.ToUIColor();
            }
            if (Element.ActualDateForegroundColor != Color.Default) {
                Control.StyleDescriptor.DateForegroundColor = Element.ActualDateForegroundColor.ToUIColor();
            }

            //Inactive Default date color
            if (Element.ActualInactiveDateBackgroundColor != Color.Default) {
                Control.StyleDescriptor.InactiveDateBackgroundColor = Element.ActualInactiveDateBackgroundColor.ToUIColor();
            }
            if (Element.ActualInactiveDateForegroundColor != Color.Default) {
                Control.StyleDescriptor.InactiveDateForegroundColor = Element.ActualInactiveDateForegroundColor.ToUIColor();
            }

            //Today date color
            if (Element.ActualTodayDateBackgroundColor != Color.Default) {
                Control.StyleDescriptor.TodayBackgroundColor = Element.ActualTodayDateBackgroundColor.ToUIColor();
            }
            if (Element.ActualTodayDateForegroundColor != Color.Default) {
                Control.StyleDescriptor.TodayForegroundColor = Element.ActualTodayDateForegroundColor.ToUIColor();
            }

            //Highlighted date color
            if (Element.ActualHighlightedDateBackgroundColor != Color.Default) {
                Control.StyleDescriptor.HighlightedDateBackgroundColor = Element.ActualHighlightedDateBackgroundColor.ToUIColor();
            }
            if (Element.ActualHighlightedDateForegroundColor != Color.Default) {
                Control.StyleDescriptor.HighlightedDateForegroundColor = Element.ActualHighlightedDateForegroundColor.ToUIColor();
            }



            //Selected date
            if (Element.ActualSelectedDateBackgroundColor != Color.Default)
                Control.StyleDescriptor.SelectedDateBackgroundColor = Element.ActualSelectedDateBackgroundColor.ToUIColor();
            if (Element.ActualSelectedDateForegroundColor != Color.Default)
                Control.StyleDescriptor.SelectedDateForegroundColor = Element.ActualSelectedDateForegroundColor.ToUIColor();

            //Selection styles
            Control.StyleDescriptor.SelectionBackgroundStyle = Element.SelectionBackgroundStyle;
            Control.StyleDescriptor.TodayBackgroundStyle = Element.TodayBackgroundStyle;

            //Divider
            //TODO: Implement it on iOS
            if (Element.DateSeparatorColor != Color.Default)
                Control.StyleDescriptor.DateSeparatorColor = Element.DateSeparatorColor.ToUIColor();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
            base.OnElementPropertyChanged(sender, e);
            ProtectFromEventCycle(() => {
                if (e.PropertyName == CalendarView.DisplayedMonthProperty.PropertyName) {
                    Control.SetDisplayedMonthYear(Element.DisplayedMonth, false);
                } else if (e.PropertyName == CalendarView.SelectedDateProperty.PropertyName) {
                    //Maybe someone will find time to make date deselectable...
                    if (Element.SelectedDate != null) {
                        Control.SetDate(Element.SelectedDate.Value, false);
                    }
                } else if (e.PropertyName == CalendarView.MinDateProperty.PropertyName) {
                    Control.SetMinAllowedDate(Element.MinDate);
                } else if (e.PropertyName == CalendarView.MaxDateProperty.PropertyName) {
                    Control.SetMaxAllowedDate(Element.MaxDate);
                }
            });

        }

        protected override void Dispose(bool disposing) {
            if (disposing && !disposed) {
                if (Control != null) {
                    Control.OnDateSelected -= OnDateSelected;
                    Control.MonthChanged -= MonthChanged;
                    Control.Dispose();
                }
                disposed = true;
            }

            base.Dispose(disposing);
        }

    }
}
