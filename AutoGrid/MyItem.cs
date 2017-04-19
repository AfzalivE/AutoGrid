using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AutoGrid {
    public class MyItem : Thumb {
        public string Content;
        private bool _isDragged;

        static MyItem() {
//            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyItem), new FrameworkPropertyMetadata(typeof(MyItem)));
        }

        public MyItem() {
            BorderThickness = new Thickness(2);
            BorderBrush = Brushes.Chartreuse;
            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            RenderTransformOrigin = new Point(0.5, 0.5);
            RenderTransform = scale;
            PreviewMouseLeftButtonUp += OnLeftButtonUp;
//            MouseLeftButtonUp += OnLeftButtonUp;
            DragDelta += OnDragDelta;
        }

        private void OnLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (!_isDragged) {
                BorderBrush = Brushes.Black;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            base.OnMouseLeftButtonDown(e);
            // don't mark as dragged until movement occurs
            _isDragged = false;
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs e) {
            _isDragged = true; // only mark as dragged when actual mvoement occurs
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            Canvas.SetRight(this, Canvas.GetRight(this) + e.HorizontalChange);
            Canvas.SetBottom(this, Canvas.GetBottom(this) + e.VerticalChange);
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
            const double by = -0.2;
            ResizeBy(by, by, 350, null);
        }

        public void Grow() {
            const double by = 0.2;
            ResizeBy(by, by, 350, null);
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