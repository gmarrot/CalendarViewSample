using CoreGraphics;
using UIKit;

namespace CalendarViewSample.iOS.Components.Calendar {
    public class CalendarArrowView : UIButton {

        public enum ArrowDirection {
            Right,
            Left
        }


        ArrowDirection _arrowDirection = ArrowDirection.Left;
        UIColor _color;


        public CalendarArrowView(CGRect frame) {
            Frame = frame;
            UserInteractionEnabled = true;
            BackgroundColor = UIColor.Clear;
        }


        public ArrowDirection Direction {
            get => _arrowDirection;
            set {
                _arrowDirection = value;
                SetBackgroundImage(GenerateImageForButton(Frame), UIControlState.Normal);
                SetNeedsDisplay();
            }
        }

        public UIColor Color {
            get => _color;
            set {
                _color = value;
                SetNeedsDisplay();
            }
        }

        UIImage GenerateImageForButton(CGRect rect) {
            UIGraphics.BeginImageContextWithOptions(rect.Size, false, 0);
            UIImage image;

            using (var context = UIGraphics.GetCurrentContext()) {
                CGPoint p1, p2, p3;
                if (_arrowDirection == ArrowDirection.Left) {
                    p1 = new CGPoint(0, rect.Height / 2);
                    p2 = new CGPoint(rect.Width, 0);
                    p3 = new CGPoint(rect.Width, rect.Height);
                } else {
                    p1 = new CGPoint(rect.Width, rect.Height / 2);
                    p2 = new CGPoint(0, 0);
                    p3 = new CGPoint(0, rect.Height);
                }

                context.SetFillColor(UIColor.Clear.CGColor);
                context.FillRect(rect);
                context.SetFillColor(Color.CGColor);
                context.MoveTo(p1.X, p1.Y);
                context.AddLineToPoint(p2.X, p2.Y);
                context.AddLineToPoint(p3.X, p3.Y);
                context.FillPath();
                image = UIGraphics.GetImageFromCurrentImageContext();
            }

            UIGraphics.EndImageContext();
            return image;
        }

    }
}
