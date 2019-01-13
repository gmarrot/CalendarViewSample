using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;

namespace CalendarViewSample.Droid.Components.Calendar {
    public class CalendarArrowView : Button {

        public enum ArrowDirection {
            Right,
            Left
        }


        ArrowDirection _arrowDirection = ArrowDirection.Left;

        Path _trianglePath;
        Paint _trianglePaint;


        public CalendarArrowView(Context context) : base(context) {
            SharedConstructor();
        }

        public CalendarArrowView(Context context, IAttributeSet attrSet) : base(context, attrSet) {
            SharedConstructor();
        }

        public CalendarArrowView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) {
            SharedConstructor();
        }

        public CalendarArrowView(IntPtr javaReference, Android.Runtime.JniHandleOwnership handleown) : base(javaReference, handleown) {
            SharedConstructor();
        }

        public ArrowDirection Direction {
            get => _arrowDirection;
            set {
                _arrowDirection = value;
                _trianglePath = GetEquilateralTriangle(Width, Height);
                Invalidate();
            }
        }

        public Color Color {
            set {
                _trianglePaint.Color = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Shareds the constructor.
        /// </summary>
        void SharedConstructor() {
            _trianglePaint = new Paint();
            _trianglePaint.SetStyle(Android.Graphics.Paint.Style.Fill);
            _trianglePaint.AntiAlias = true;
            _trianglePaint.Color = Color.Black;
        }


        protected override void OnSizeChanged(int w, int h, int oldw, int oldh) {
            base.OnSizeChanged(w, h, oldw, oldh);
            _trianglePath = GetEquilateralTriangle(w, h);
        }

        protected override void OnDraw(Canvas canvas) {
            base.OnDraw(canvas);
            canvas.DrawPath(_trianglePath, _trianglePaint);
        }


        Path GetEquilateralTriangle(int width, int height) {
            PointF p1, p2, p3;
            if (_arrowDirection == ArrowDirection.Left) {
                p1 = new PointF(0, height / 2);
                p2 = new PointF(width, 0);
                p3 = new PointF(width, height);
            } else {
                p1 = new PointF(width, height / 2);
                p2 = new PointF(0, 0);
                p3 = new PointF(0, height);
            }
            Path path = new Path();
            path.MoveTo(p1.X, p1.Y);
            path.LineTo(p2.X, p2.Y);
            path.LineTo(p3.X, p3.Y);
            return path;
        }

    }
}
