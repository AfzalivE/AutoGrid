using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AutoGrid {
    public class MyItem : Label {
        static MyItem() {
//            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyItem), new FrameworkPropertyMetadata(typeof(MyItem)));
        }

        public MyItem() {
            BorderThickness = new Thickness(2);
            BorderBrush = Brushes.Chartreuse;
            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            RenderTransformOrigin = new Point(0.5, 0.5);
            RenderTransform = scale;
        }

        public void SetContainerRect(int left, int top, int right, int bottom) {
            SetContainerRect(left, top, right, bottom, false);
        }

        public void SetContainerRect(int left, int top, int right, int bottom, bool animate) {
            Console.WriteLine("Setting container rect for: {0}, left: {1}, top: {2}, right: {3}, bottom: {4}", Content, left, top, right,
                              bottom);
            this.Width = right - left;
            this.Height = bottom - top;
            if (Parent != null) {
                Move(left, top, TimeSpan.FromMilliseconds(250), 0);
            } else {
                Canvas.SetLeft(this, left);
                Canvas.SetRight(this, right);
                Canvas.SetTop(this, top);
                Canvas.SetBottom(this, bottom);
                if (animate) {
                    Show(350, null);
                }
            }
        }

        public void Move(double left, double top, TimeSpan duration, long delay) {
            Vector offset = VisualTreeHelper.GetOffset(this);
            double oldLeft = offset.X;
            double oldTop = offset.Y;
            TranslateTransform trans = new TranslateTransform();

            RenderTransform = trans;

            DoubleAnimation xAnim = new DoubleAnimation(oldLeft, left, duration) {
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
                BeginTime = TimeSpan.FromMilliseconds(delay)
            };

            DoubleAnimation yAnim = new DoubleAnimation(oldTop, top, duration) {
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
                BeginTime = TimeSpan.FromMilliseconds(delay)
            };

            Storyboard.SetTarget(xAnim, this);
            Storyboard.SetTargetProperty(xAnim, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTarget(yAnim, this);
            Storyboard.SetTargetProperty(yAnim, new PropertyPath("(Canvas.Top)"));

            Storyboard moveStoryboard = new Storyboard();
            moveStoryboard.Children.Add(xAnim);
            moveStoryboard.Children.Add(yAnim);
            moveStoryboard.Begin();
        }

        public void Shrink() {
            double by = -0.2;
            ResizeBy(@by, @by, 350, null);
        }

        public void Grow() {
            double by = 0.2;
            ResizeBy(@by, @by, 350, null);
        }

        public void Hide(EventHandler onCompleted) {
            ResizeBy(-1, -1, 200, onCompleted);
        }

        public void Hide(long duration, EventHandler onCompleted) {
            ResizeBy(-1, -1, duration, onCompleted);
        }

        public void Show() {
            ResizeBy(1, 1, 350, null);
        }

        public void Show(long duration) {
            ResizeBy(1, 1, duration, null);
        }

        public void Show(long duration, EventHandler onCompleted) {
            DoubleAnimation wAnim = new DoubleAnimation() {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut}
            };

            DoubleAnimation hAnim = new DoubleAnimation() {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut}
            };

            Storyboard.SetTarget(hAnim, this);
            Storyboard.SetTargetProperty(hAnim, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(wAnim, this);
            Storyboard.SetTargetProperty(wAnim, new PropertyPath("RenderTransform.ScaleY"));

            Storyboard moveStoryboard = new Storyboard();
            moveStoryboard.Children.Add(hAnim);
            moveStoryboard.Children.Add(wAnim);

            if (onCompleted != null) {
                moveStoryboard.Completed += onCompleted;
            }
            moveStoryboard.Begin();
        }

        public void ResizeBy(double widthBy, double heightBy, long duration, EventHandler onCompleted) {

            DoubleAnimation wAnim = new DoubleAnimation() {
                By = widthBy,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut}
            };

            DoubleAnimation hAnim = new DoubleAnimation() {
                By = heightBy,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut}
            };

            Storyboard.SetTarget(hAnim, this);
            Storyboard.SetTargetProperty(hAnim, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(wAnim, this);
            Storyboard.SetTargetProperty(wAnim, new PropertyPath("RenderTransform.ScaleY"));

            Storyboard moveStoryboard = new Storyboard();
            moveStoryboard.Children.Add(hAnim);
            moveStoryboard.Children.Add(wAnim);

            if (onCompleted != null) {
                moveStoryboard.Completed += onCompleted;
            }
            moveStoryboard.Begin();
        }
    }
}