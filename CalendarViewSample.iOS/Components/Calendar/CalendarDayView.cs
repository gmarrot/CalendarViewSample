using System;
using UIKit;
using CoreGraphics;
using CalendarViewSample.Components;
using Foundation;

namespace CalendarViewSample.iOS.Components.Calendar {
    public class CalendarDayView : UIView {

        static NSMutableParagraphStyle _paragraphStyle;

        readonly CalendarMonthView _mv;

        string _text;
        UIColor _oldBackgorundColor;
        bool _active, _today, _selected, _marked, _available, _highlighted;

        public CalendarDayView(CalendarMonthView mv) {
            _mv = mv;
            BackgroundColor = mv.StyleDescriptor.DateBackgroundColor;
        }

        public bool Available {
            get => _available;
            set {
                _available = value;
                SetNeedsDisplay();
            }
        }

        public string Text {
            get => _text;
            set { _text = value; SetNeedsDisplay(); }
        }

        public bool Active {
            get => _active;
            set {
                _active = value;
                SetNeedsDisplay();
            }
        }

        public bool Today {
            get => _today;
            set {
                _today = value;
                SetNeedsDisplay();
            }
        }

        public bool Selected {
            get => _selected;
            set {
                _selected = value;
                SetNeedsDisplay();
            }
        }

        public bool Marked {
            get => _marked;
            set {
                _marked = value;
                SetNeedsDisplay();
            }
        }

        public bool Highlighted {
            get => _highlighted;
            set {
                _highlighted = value;
                SetNeedsDisplay();
            }
        }

        public DateTime Date { get; set; }

        public override void Draw(CGRect rect) {
            //UIImage img = null;
            var color = _mv.StyleDescriptor.InactiveDateForegroundColor;
            BackgroundColor = _mv.StyleDescriptor.InactiveDateBackgroundColor;
            var backgroundStyle = CalendarView.BackgroundStyle.Fill;


            if (!Active || !Available) {
                if (Highlighted) {
                    BackgroundColor = _mv.StyleDescriptor.HighlightedDateBackgroundColor;
                }
                //color = UIColor.FromRGBA(0.576f, 0.608f, 0.647f, 1f);
                //img = UIImage.FromBundle("Images/Calendar/datecell.png");
            } else if (Today && Selected) {
                color = _mv.StyleDescriptor.SelectedDateForegroundColor;
                BackgroundColor = _mv.StyleDescriptor.SelectedDateBackgroundColor;
                backgroundStyle = _mv.StyleDescriptor.SelectionBackgroundStyle;
                //img = UIImage.FromBundle("Images/Calendar/todayselected.png").CreateResizableImage(new UIEdgeInsets(4,4,4,4));
            } else if (Today) {
                color = _mv.StyleDescriptor.TodayForegroundColor;
                BackgroundColor = _mv.StyleDescriptor.TodayBackgroundColor;
                backgroundStyle = _mv.StyleDescriptor.TodayBackgroundStyle;
                //img = UIImage.FromBundle("Images/Calendar/today.png").CreateResizableImage(new UIEdgeInsets(4,4,4,4));
            } else if (Selected || Marked) {
                //color = UIColor.White;
                color = _mv.StyleDescriptor.SelectedDateForegroundColor;
                BackgroundColor = _mv.StyleDescriptor.SelectedDateBackgroundColor;
                backgroundStyle = _mv.StyleDescriptor.SelectionBackgroundStyle;
                //img = UIImage.FromBundle("Images/Calendar/datecellselected.png").CreateResizableImage(new UIEdgeInsets(4,4,4,4));
            } else if (Highlighted) {
                color = _mv.StyleDescriptor.HighlightedDateForegroundColor;
                BackgroundColor = _mv.StyleDescriptor.HighlightedDateBackgroundColor;
            } else {
                color = _mv.StyleDescriptor.DateForegroundColor;
                BackgroundColor = _mv.StyleDescriptor.DateBackgroundColor;
                //img = UIImage.FromBundle("Images/Calendar/datecell.png");
            }

            //if (img != null)
            //img.Draw(new RectangleF(0, 0, _mv.BoxWidth, _mv.BoxHeight));
            var context = UIGraphics.GetCurrentContext();
            if (_oldBackgorundColor != BackgroundColor) {
                if (backgroundStyle == CalendarView.BackgroundStyle.Fill) {
                    context.SetFillColor(BackgroundColor.CGColor);
                    context.FillRect(new CGRect(0, 0, _mv.BoxWidth, _mv.BoxHeight));
                } else {
                    context.SetFillColor(Highlighted
                        ? _mv.StyleDescriptor.HighlightedDateBackgroundColor.CGColor
                        : _mv.StyleDescriptor.DateBackgroundColor.CGColor);

                    context.FillRect(new CGRect(0, 0, _mv.BoxWidth, _mv.BoxHeight));

                    var smallerSide = Math.Min(_mv.BoxWidth, _mv.BoxHeight);
                    var center = new CGPoint(_mv.BoxWidth / 2, _mv.BoxHeight / 2);
                    var circleArea = new CGRect(center.X - smallerSide / 2, center.Y - smallerSide / 2, smallerSide, smallerSide);

                    if (backgroundStyle == CalendarView.BackgroundStyle.CircleFill) {
                        context.SetFillColor(BackgroundColor.CGColor);
                        context.FillEllipseInRect(circleArea.Inset(1, 1));
                    } else {
                        context.SetStrokeColor(BackgroundColor.CGColor);
                        context.StrokeEllipseInRect(circleArea.Inset(2, 2));
                    }
                }
            }


            color.SetColor();
            var inflated = new CGRect(0, 0, Bounds.Width, Bounds.Height);
            //          var attrs = new UIStringAttributes() {
            //              Font = _mv.StyleDescriptor.DateLabelFont,
            //              ForegroundColor = color,
            //              ParagraphStyle = 
            //
            //          };
            //((NSString)Text).DrawString(inflated,attrs);
            //DrawString(Text, inflated,_mv.StyleDescriptor.DateLabelFont,UILineBreakMode.WordWrap, UITextAlignment.Center);
            DrawDateString((NSString)Text, color, inflated);

            //            if (Marked)
            //            {
            //                var context = UIGraphics.GetCurrentContext();
            //                if (Selected || Today)
            //                    context.SetRGBFillColor(1, 1, 1, 1);
            //                else if (!Active || !Available)
            //                  UIColor.LightGray.SetColor();
            //              else
            //                    context.SetRGBFillColor(75/255f, 92/255f, 111/255f, 1);
            //                context.SetLineWidth(0);
            //                context.AddEllipseInRect(new RectangleF(Frame.Size.Width/2 - 2, 45-10, 4, 4));
            //                context.FillPath();
            //
            //            }
            _oldBackgorundColor = BackgroundColor;
        }

        void DrawDateString(NSString dateString, UIColor color, CGRect rect) {
            if (_paragraphStyle == null) {
                _paragraphStyle = (NSMutableParagraphStyle)NSParagraphStyle.Default.MutableCopy();
                _paragraphStyle.LineBreakMode = UILineBreakMode.TailTruncation;
                _paragraphStyle.Alignment = UITextAlignment.Center;

            }
            var attrs = new UIStringAttributes {
                Font = _mv.StyleDescriptor.DateLabelFont,
                ForegroundColor = color,
                ParagraphStyle = _paragraphStyle
            };
            CGSize size = dateString.GetSizeUsingAttributes(attrs);
            var targetRect = new CGRect(rect.X + (float)Math.Floor((rect.Width - size.Width) / 2f),
                                        rect.Y + (float)Math.Floor((rect.Height - size.Height) / 2f),
                                        size.Width,
                                        size.Height);
            dateString.DrawString(targetRect, attrs);
        }

    }
}
