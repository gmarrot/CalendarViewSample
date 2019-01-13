using System;
using System.Collections.Generic;
using System.Globalization;
using CoreGraphics;
using Foundation;
using UIKit;

namespace CalendarViewSample.iOS.Components.Calendar {
    public class CalendarMonthView : UIView {

        bool _calendarIsLoaded;

        UIScrollView _scrollView;

        MonthGridView _monthGridView;
        CalendarArrowView _leftArrow;
        CalendarArrowView _rightArrow;

        DateTime? _minDateTime;
        DateTime? _maxDateTime;

        readonly int _headerHeight;

        readonly bool _showHeader;
        readonly bool _showNavArrows;

        public CalendarMonthView(DateTime selectedDate, bool showHeader, bool showNavArrows, float width = 320) {
            _showHeader = showHeader;
            _showNavArrows = showNavArrows;

            if (_showNavArrows) {
                _showHeader = true;
            }

            StyleDescriptor = new StyleDescriptor();
            HighlightDaysOfWeeks(new DayOfWeek[] { });

            if (_showHeader && _headerHeight == 0) {
                _headerHeight = showNavArrows ? 40 : 20;
            }

            Frame = _showHeader ? new CGRect(0, 0, width, 198 + _headerHeight) : new CGRect(0, 0, width, 198);

            BoxWidth = Convert.ToInt32(Math.Ceiling(width / 7));

            BackgroundColor = UIColor.White;

            ClipsToBounds = true;
            CurrentDate = DateTime.Now.Date;
            CurrentMonthYear = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);

            CurrentSelectedDate = selectedDate;

            var swipeLeft = new UISwipeGestureRecognizer(MonthViewSwipedLeft) {
                Direction = UISwipeGestureRecognizerDirection.Left
            };
            AddGestureRecognizer(swipeLeft);

            var swipeRight = new UISwipeGestureRecognizer(MonthViewSwipedRight) {
                Direction =
                    UISwipeGestureRecognizerDirection.Right
            };
            AddGestureRecognizer(swipeRight);

            var swipeUp = new UISwipeGestureRecognizer(MonthViewSwipedUp) { Direction = UISwipeGestureRecognizerDirection.Up };
            AddGestureRecognizer(swipeUp);
        }

        /// <summary>
        /// The box height
        /// </summary>
        public int BoxHeight = 30;

        /// <summary>
        /// The box width
        /// </summary>
        public int BoxWidth = 46;

        /// <summary>
        /// The current month year
        /// </summary>
        public DateTime CurrentMonthYear;

        /// <summary>
        /// The current selected date
        /// </summary>
        public DateTime CurrentSelectedDate;

        /// <summary>
        /// The is date available
        /// </summary>
        public Func<DateTime, bool> IsDateAvailable;

        /// <summary>
        /// The is day marked delegate
        /// </summary>
        public Func<DateTime, bool> IsDayMarkedDelegate;

        /// <summary>
        /// The month changed
        /// </summary>
        public Action<DateTime> MonthChanged;

        /// <summary>
        /// The on date selected
        /// </summary>
        public Action<DateTime> OnDateSelected;

        /// <summary>
        /// The on finished date selection
        /// </summary>
        public Action<DateTime> OnFinishedDateSelection;

        /// <summary>
        /// The swiped up
        /// </summary>
        public Action SwipedUp;

        /// <summary>
        /// Gets the highlighted days of week.
        /// </summary>
        /// <value>The highlighted days of week.</value>
        public Dictionary<int, bool> HighlightedDaysOfWeek { get; private set; }

        /// <summary>
        /// Gets or sets the current date.
        /// </summary>
        /// <value>The current date.</value>
        protected DateTime CurrentDate { get; set; }

        /// <summary>
        /// Gets the style descriptor.
        /// </summary>
        /// <value>The style descriptor.</value>
        public StyleDescriptor StyleDescriptor { get; private set; }

        public override void Draw(CGRect rect) {
            using (var context = UIGraphics.GetCurrentContext()) {
                context.SetFillColor(StyleDescriptor.TitleBackgroundColor.CGColor);
                //Console.WriteLine("Title background color is {0}",_styleDescriptor.TitleBackgroundColor.ToString());
                context.FillRect(new CGRect(0, 0, 320, 18 + _headerHeight));
            }

            DrawDayLabels(rect);

            if (_showHeader) {
                DrawMonthLabel(rect);
            }
        }

        public void SetDate(DateTime newDate, bool animated) {
            var right = true;

            CurrentSelectedDate = newDate;

            var monthsDiff = (newDate.Month - CurrentMonthYear.Month) + 12 * (newDate.Year - CurrentMonthYear.Year);
            if (monthsDiff != 0) {
                if (monthsDiff < 0) {
                    right = false;
                    monthsDiff = -monthsDiff;
                }

                for (var i = 0; i < monthsDiff; i++) {
                    MoveCalendarMonths(right, animated);
                }
            } else {
                //If we have created the layout already
                if (_scrollView != null) {
                    RebuildGrid(true, animated);
                }
            }
        }

        public void SetMaxAllowedDate(DateTime? maxDate) {
            _maxDateTime = maxDate;
        }

        public void SetMinAllowedDate(DateTime? minDate) {
            _minDateTime = minDate;
        }

        public void HighlightDaysOfWeeks(DayOfWeek[] daysOfWeeks) {
            HighlightedDaysOfWeek = new Dictionary<int, bool>();
            for (var i = 0; i <= 6; i++) {
                HighlightedDaysOfWeek[i] = false;
            }
            foreach (var dOw in daysOfWeeks) {
                HighlightedDaysOfWeek[(int)dOw] = true;
            }
        }

        public void SetDisplayedMonthYear(DateTime newDate, bool animated) {
            var right = true;
            var monthsDiff = (newDate.Month - CurrentMonthYear.Month) + 12 * (newDate.Year - CurrentMonthYear.Year);
            if (monthsDiff != 0) {
                if (monthsDiff < 0) {
                    right = false;
                    monthsDiff = -monthsDiff;
                }

                for (var i = 0; i < monthsDiff; i++) {
                    MoveCalendarMonths(right, animated);
                }
            } else {
                //If we have created the layout already
                if (_scrollView != null) {
                    RebuildGrid(true, animated);
                }
            }
        }

        public override void SetNeedsDisplay() {
            base.SetNeedsDisplay();
            if (_monthGridView != null) {
                _monthGridView.Update();
            }
        }

        public override void LayoutSubviews() {
            if (_calendarIsLoaded) {
                return;
            }

            _scrollView = new UIScrollView {
                ContentSize = new CGSize(320, 260),
                ScrollEnabled = false,
                Frame = new CGRect(0, 16 + _headerHeight, 320, Frame.Height - 16),
                BackgroundColor = StyleDescriptor.BackgroundColor
            };

            //_shadow = new UIImageView(UIImage.FromBundle("Images/Calendar/shadow.png"));

            //LoadButtons();

            LoadNavArrows();
            SetNavigationArrows(false);
            LoadInitialGrids();

            BackgroundColor = UIColor.Clear;

            AddSubview(_scrollView);

            //AddSubview(_shadow);

            _scrollView.AddSubview(_monthGridView);

            _calendarIsLoaded = true;
        }

        public void DeselectDate() {
            if (_monthGridView != null) {
                _monthGridView.DeselectDayView();
            }
        }

        public void MoveCalendarMonths(bool right, bool animated) {
            var newDate = CurrentMonthYear.AddMonths(right ? 1 : -1);
            if ((_minDateTime != null && newDate < _minDateTime.Value.Date)
                || (_maxDateTime != null && newDate > _maxDateTime.Value.Date)) {
                if (animated) {
                    var oldX = _monthGridView.Center.X;

                    _monthGridView.Center = new CGPoint(oldX, _monthGridView.Center.Y);
                    Animate(
                        0.25,
                        () => _monthGridView.Center = new CGPoint(_monthGridView.Center.X - (right ? 40 : -40), _monthGridView.Center.Y),
                        () => { Animate(0.25, () => { _monthGridView.Center = new CGPoint(oldX, _monthGridView.Center.Y); }); });
                }
                return;
            }

            CurrentMonthYear = newDate;
            SetNavigationArrows(animated);
            //If we have created the layout already
            if (_scrollView != null) {
                RebuildGrid(right, animated);
            }
        }

        public void RebuildGrid(bool right, bool animated) {
            UserInteractionEnabled = false;

            var gridToMove = CreateNewGrid(CurrentMonthYear);
            var pointsToMove = (right ? Frame.Width : -Frame.Width);

            /*if (left && gridToMove.weekdayOfFirst==0)
                pointsToMove += 44;
            if (!left && _monthGridView.weekdayOfFirst==0)
                pointsToMove -= 44;*/

            gridToMove.Frame = new CGRect(new CGPoint(pointsToMove, 0), gridToMove.Frame.Size);

            _scrollView.AddSubview(gridToMove);

            if (animated) {
                BeginAnimations("changeMonth");
                SetAnimationDuration(0.4);
                SetAnimationDelay(0.1);
                SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
            }

            _monthGridView.Center = new CGPoint(_monthGridView.Center.X - pointsToMove, _monthGridView.Center.Y);
            gridToMove.Center = new CGPoint(gridToMove.Center.X - pointsToMove, gridToMove.Center.Y);

            _monthGridView.Alpha = 0;

            /*_scrollView.Frame = new RectangleF(
                _scrollView.Frame.Location,
                new SizeF(_scrollView.Frame.Width, this.Frame.Height-16));
            
            _scrollView.ContentSize = _scrollView.Frame.Size;*/

            SetNeedsDisplay();

            if (animated) {
                CommitAnimations();
            }

            _monthGridView = gridToMove;

            UserInteractionEnabled = true;

            if (MonthChanged != null) {
                MonthChanged(CurrentMonthYear);
            }
        }

        void LoadNavArrows() {
            _leftArrow = new CalendarArrowView(new CGRect(10, 9, 18, 22)) { Color = StyleDescriptor.TitleForegroundColor };
            _leftArrow.TouchUpInside += HandlePreviousMonthTouch;
            _leftArrow.Direction = CalendarArrowView.ArrowDirection.Left;
            AddSubview(_leftArrow);
            _rightArrow = new CalendarArrowView(new CGRect(320 - 22 - 10, 9, 18, 22)) {
                Color =
                    StyleDescriptor.TitleForegroundColor
            };
            _rightArrow.TouchUpInside += HandleNextMonthTouch;
            _rightArrow.Direction = CalendarArrowView.ArrowDirection.Right;
            AddSubview(_rightArrow);
        }

        /*private void LoadButtons()
        {
            _leftButton = UIButton.FromType(UIButtonType.Custom);
            _leftButton.TouchUpInside += HandlePreviousMonthTouch;
            _leftButton.SetImage(UIImage.FromBundle("Images/Calendar/leftarrow.png"), UIControlState.Normal);
            AddSubview(_leftButton);
            _leftButton.Frame = new RectangleF(10, 0, 44, 42);
            
            _rightButton = UIButton.FromType(UIButtonType.Custom);
            _rightButton.TouchUpInside += HandleNextMonthTouch;
            _rightButton.SetImage(UIImage.FromBundle("Images/Calendar/rightarrow.png"), UIControlState.Normal);
            AddSubview(_rightButton);
            _rightButton.Frame = new RectangleF(320 - 56, 0, 44, 42);
        }*/

        void HandlePreviousMonthTouch(object sender, EventArgs e) {
            MoveCalendarMonths(false, true);
        }

        void HandleNextMonthTouch(object sender, EventArgs e) {
            MoveCalendarMonths(true, true);
        }

        void SetNavigationArrows(bool animated) {
            var isMin = false;
            var isMax = false;
            if (_minDateTime != null) {
                isMin = CurrentMonthYear.Month == _minDateTime.Value.Month && CurrentMonthYear.Year == _minDateTime.Value.Year;
            }
            if (_maxDateTime != null) {
                isMax = CurrentMonthYear.Month == _maxDateTime.Value.Month && CurrentMonthYear.Year == _maxDateTime.Value.Year;
            }

            if (!_showNavArrows) return;

            Action action = () => {
                if (_leftArrow != null) {
                    if (isMin && _leftArrow.Enabled) {
                        _leftArrow.Enabled = false;
                        _leftArrow.Alpha = 0;
                    } else {
                        _leftArrow.Enabled = true;
                        _leftArrow.Alpha = 1;
                    }
                }

                if (_rightArrow != null) {
                    if (isMax && _rightArrow.Enabled) {
                        _rightArrow.Enabled = false;
                        _rightArrow.Alpha = 0;
                    } else {
                        _rightArrow.Enabled = true;
                        _rightArrow.Alpha = 1;
                    }
                }
            };

            if (animated) {
                Animate(0.250, action);
            } else {
                action();
            }
        }

        void MonthViewSwipedUp(UISwipeGestureRecognizer ges) {
            SwipedUp?.Invoke();
        }

        void MonthViewSwipedRight(UISwipeGestureRecognizer ges) {
            MoveCalendarMonths(false, true);
        }

        void MonthViewSwipedLeft(UISwipeGestureRecognizer ges) {
            MoveCalendarMonths(true, true);
        }

        MonthGridView CreateNewGrid(DateTime date) {
            var grid = new MonthGridView(this, date) { CurrentDate = CurrentDate };
            grid.BuildGrid();
            grid.Frame = new CGRect(0, 0, 320, Frame.Height - 16);
            return grid;
        }

        void LoadInitialGrids() {
            _monthGridView = CreateNewGrid(CurrentMonthYear);

            /*var rect = _scrollView.Frame;
            rect.Size = new SizeF { Height = (_monthGridView.Lines + 1) * 44, Width = rect.Size.Width };
            _scrollView.Frame = rect;*/

            //Frame = new RectangleF(Frame.X, Frame.Y, _scrollView.Frame.Size.Width, _scrollView.Frame.Size.Height+16);

            /*var imgRect = _shadow.Frame;
            imgRect.Y = rect.Size.Height - 132;
            _shadow.Frame = imgRect;*/
        }

        void DrawMonthLabel(CGRect rect) {
            var r = new CGRect(new CGPoint(0, 2), new CGSize { Width = 320, Height = _headerHeight });
            //          _styleDescriptor.TitleForegroundColor.SetColor();
            //          DrawString(CurrentMonthYear.ToString("MMMM yyyy"), 
            //              r, _styleDescriptor.MonthTitleFont,
            //              UILineBreakMode.WordWrap, UITextAlignment.Center);
            DrawCenteredString((NSString)CurrentMonthYear.ToString("MMMM yyyy"),
                               StyleDescriptor.TitleForegroundColor,
                               r,
                StyleDescriptor.MonthTitleFont);
        }

        void DrawDayLabels(CGRect rect) {
            var font = StyleDescriptor.DateLabelFont;

            var context = UIGraphics.GetCurrentContext();
            context.SaveState();
            var firstDayOfWeek = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var today = CurrentDate;
            var originalDay = today;
            for (var i = 0; i < 7; i++) {
                var offset = firstDayOfWeek - (int)today.DayOfWeek + i;
                today = today.AddDays(offset);
                var dateRectangle = new CGRect(i * BoxWidth, 2 + _headerHeight, BoxWidth, 15);
                if (StyleDescriptor.ShouldHighlightDaysOfWeekLabel && HighlightedDaysOfWeek[(int)today.DayOfWeek]) {
                    context.SetFillColor(StyleDescriptor.HighlightedDateBackgroundColor.CGColor);
                } else {
                    context.SetFillColor(StyleDescriptor.DayOfWeekLabelBackgroundColor.CGColor);
                }

                context.FillRect(dateRectangle);
                if (StyleDescriptor.ShouldHighlightDaysOfWeekLabel && HighlightedDaysOfWeek[(int)today.DayOfWeek]) {
                    StyleDescriptor.HighlightedDateForegroundColor.SetColor();
                } else {
                    StyleDescriptor.DayOfWeekLabelForegroundColor.SetColor();
                }

                DrawCenteredString(new NSString(today.ToString("ddd")), UIColor.White, dateRectangle, font);
                today = originalDay;
            }

            //          var i = 0;
            //          foreach (var d in Enum.GetNames(typeof(DayOfWeek)))
            //          {
            //              var dateRectangle = new RectangleF(i*BoxWidth, 2 + headerHeight, BoxWidth, 10);
            //              context.SetFillColorWithColor(_styleDescriptor.DayOfWeekLabelBackgroundColor.CGColor);
            //              context.FillRect(dateRectangle);
            //              _styleDescriptor.DayOfWeekLabelForegroundColor.SetColor();
            //              DrawString(d.Substring(0, 3),dateRectangle, font,
            //                  UILineBreakMode.WordWrap, UITextAlignment.Center);
            //              i++;
            //          }
            context.RestoreState();
        }

        static void DrawCenteredString(NSString text, UIColor color, CGRect rect, UIFont font) {
            var paragraphStyle = (NSMutableParagraphStyle)NSParagraphStyle.Default.MutableCopy();
            paragraphStyle.LineBreakMode = UILineBreakMode.TailTruncation;
            paragraphStyle.Alignment = UITextAlignment.Center;
            var attrs = new UIStringAttributes { Font = font, ForegroundColor = color, ParagraphStyle = paragraphStyle };
            var size = text.GetSizeUsingAttributes(attrs);
            var targetRect = new CGRect(rect.X + (float)Math.Floor((rect.Width - size.Width) / 2f),
                                        rect.Y + (float)Math.Floor((rect.Height - size.Height) / 2f),
                                        size.Width,
                                        size.Height);
            text.DrawString(targetRect, attrs);
        }

    }
}
