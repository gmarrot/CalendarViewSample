using System;
using System.Diagnostics;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Support.V4.Content;

namespace CalendarViewSample.Droid.Components.Calendar {
    public class CalendarGridView : ViewGroup {

        const float FLOAT_FUDGE = 0.5f;
        const int SIDE_PADDING = 0;

        readonly Paint _dividerPaint = new Paint();

        int _oldWidthMeasureSize;
        int _oldHeightMeasureSize;

        int _oldNumRows;

        public CalendarGridView(Context context, IAttributeSet attrs) : base(context, attrs) {
            int dividerColor = ContextCompat.GetColor(context, Resource.Color.calendar_divider);
            _dividerPaint.Color = new Color(dividerColor);
        }

        public Color DividerColor {
            set {
                _dividerPaint.Color = value;
            }
        }

        public int NumRows {
            set {
                if (_oldNumRows != value) {
                    _oldWidthMeasureSize = 0;
                }
                _oldNumRows = value;
            }
        }

        public override void AddView(View child, int index, LayoutParams @params) {
            if (ChildCount == 0) {
                ((CalendarRowView)child).IsHeaderRow = true;
            }

            base.AddView(child, index, @params);
        }

        protected override void DispatchDraw(Canvas canvas) {
            base.DispatchDraw(canvas);

            var row = (ViewGroup)GetChildAt(1);
            int top = row.Top;
            int bottom = Bottom;

            //Left side border.
            int left = row.GetChildAt(0).Left;
            canvas.DrawLine(left + FLOAT_FUDGE, top, left + FLOAT_FUDGE, bottom, _dividerPaint);

            //Each cell's right-side border.
            for (int c = 0; c < 7; c++) {
                float x = left + row.GetChildAt(c).Right - FLOAT_FUDGE;
                canvas.DrawLine(x, top, x, bottom, _dividerPaint);
            }
        }

        protected override bool DrawChild(Canvas canvas, View child, long drawingTime) {
            bool isInvalidated = base.DrawChild(canvas, child, drawingTime);
            //Draw a bottom border
            int bottom = child.Bottom - 1;
            canvas.DrawLine(child.Left, bottom, child.Right - 2, bottom, _dividerPaint);
            return isInvalidated;
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
            Logr.D("Grid.OnMeasure w={0} h={1}", MeasureSpec.ToString(widthMeasureSpec),
                MeasureSpec.ToString(heightMeasureSpec));

            int widthMeasureSize = MeasureSpec.GetSize(widthMeasureSpec);
            int heightMeasureSize = MeasureSpec.GetSize(heightMeasureSpec);
            if (_oldWidthMeasureSize == widthMeasureSize && _oldHeightMeasureSize == heightMeasureSize) {
                Logr.D("SKIP Grid.OnMeasure");
                SetMeasuredDimension(MeasuredWidth, MeasuredHeight);
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            _oldWidthMeasureSize = widthMeasureSize;
            _oldHeightMeasureSize = heightMeasureSize;
            int visibleChildCount = 0;
            for (int c = 0; c < ChildCount; c++) {
                var child = GetChildAt(c);
                if (child.Visibility == ViewStates.Visible) {
                    visibleChildCount++;
                }
            }
            int cellSize = Math.Min((widthMeasureSize - SIDE_PADDING * 2) / 7, heightMeasureSize / visibleChildCount);
            //int cellSize =  widthMeasureSize / 7;
            //Remove any extra pixels since /7 us unlikey to give whole nums.
            widthMeasureSize = cellSize * 7 + SIDE_PADDING * 2;
            int totalHeight = 0;
            int rowWidthSpec = MeasureSpec.MakeMeasureSpec(widthMeasureSize - 2 * SIDE_PADDING, MeasureSpecMode.Exactly);
            int rowHeightSpec = MeasureSpec.MakeMeasureSpec(cellSize, MeasureSpecMode.Exactly);
            for (int c = 0; c < ChildCount; c++) {
                var child = GetChildAt(c);
                if (child.Visibility == ViewStates.Visible) {
                    MeasureChild(child, rowWidthSpec,
                        c == 0 ? MeasureSpec.MakeMeasureSpec(cellSize, MeasureSpecMode.AtMost) : rowHeightSpec);
                    totalHeight += child.MeasuredHeight;
                }
            }
            int measuredWidth = widthMeasureSize; // Fudge factor to make the borders show up right.
            int measuredHeight = heightMeasureSize + 2;
            SetMeasuredDimension(measuredWidth, totalHeight);

            stopwatch.Stop();
            Logr.D("Grid.OnMeasure {0} ms", stopwatch.ElapsedMilliseconds);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b) {
            var stopwatch = Stopwatch.StartNew();
            int heightSoFar = 0;
            for (int c = 0; c < ChildCount; c++) {
                var child = GetChildAt(c);
                int rowHeight = child.MeasuredHeight;
                child.Layout(SIDE_PADDING, heightSoFar, r, heightSoFar + rowHeight);
                heightSoFar += rowHeight;
            }

            stopwatch.Stop();
            Logr.D("Grid.OnLayout {0} ms", stopwatch.ElapsedMilliseconds);
        }

    }
}
