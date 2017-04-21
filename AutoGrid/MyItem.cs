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
        private ScaleTransform _scaleTransform;
        private Transform _translateTransform;
        private bool _isMouseDown;

        static MyItem() {
//            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyItem), new FrameworkPropertyMetadata(typeof(MyItem)));
        }

        public MyItem() {
            BorderThickness = new Thickness(2);
            BorderBrush = Brushes.Chartreuse;
            _translateTransform = new TranslateTransform();
            _scaleTransform = new ScaleTransform(1.0, 1.0);
            RenderTransformOrigin = new Point(0.5, 0.5);
            PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
            DragDelta += OnDragDelta;
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (!_isMouseDown) {
                Console.WriteLine("Mouse was NOT down");
                return;
            }
            Console.WriteLine("Mouse was down");
            _isMouseDown = false;
            if (!_isDragged) {
//                Grow();
                Console.WriteLine("Mouse was NOT dragged");
                BorderBrush = Brushes.Black;
            } else {
                Console.WriteLine("Mouse was dragged");
                ((GridAdapter) Parent).MaybeRemove(this);
                ((MainWindow) Application.Current.MainWindow).RetrieveFromTemp();
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            base.OnMouseLeftButtonDown(e);
            Console.WriteLine("Mouse is down");
//            Shrink();
            // don't mark as dragged until movement occurs
            _isMouseDown = true;
            _isDragged = false;
//            Panel.SetZIndex(this, 0);
//            Panel.SetZIndex((UIElement) Parent, 0);
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs e) {
            _isDragged = true; // only mark as dragged when actual movement occurs
            Console.WriteLine("Mouse is dragging");
//            Panel.SetZIndex(this, 999);
//            Panel.SetZIndex((UIElement) Parent, 999);
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
//                Show();
                Move(left, top, TimeSpan.FromMilliseconds(250), 0);
            } else {
                Canvas.SetLeft(this, left);
                Canvas.SetRight(this, right);
                Canvas.SetTop(this, top);
                Canvas.SetBottom(this, bottom);
                if (animate) {
//                    Show();
                    Move(left, top, TimeSpan.FromMilliseconds(250), 0);
                }
            }
        }

        public void Shrink() {
            const double by = -0.2;
            ResizeBy(by, by, 75, null);
        }

        public void Grow() {
            const double by = 0.2;
            ResizeBy(by, by, 75, null);
        }

        public void Hide(EventHandler onCompleted) {
            ResizeBy(-1, -1, 200, onCompleted);
        }

        public void Hide(long duration, EventHandler onCompleted) {
            ResizeBy(-1, -1, duration, onCompleted);
        }

        public void Show() {
            Opacity(1, 350);
        }

        public void Show(long duration) {
            Opacity(1, duration);
        }

        public void Opacity(double opacity, long duration) {
            DoubleAnimation opacityAnim = new DoubleAnimation() {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
            };

            Storyboard.SetTarget(opacityAnim, this);
            Storyboard.SetTargetProperty(opacityAnim, new PropertyPath(OpacityProperty));

            Storyboard opacityStoryboard = new Storyboard();
            opacityStoryboard.Children.Add(opacityAnim);

            opacityStoryboard.Begin();
        }

        public void Show(long duration, EventHandler onCompleted) {
            RenderTransform = _scaleTransform;

            DoubleAnimation wAnim = new DoubleAnimation() {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
            };

            DoubleAnimation hAnim = new DoubleAnimation() {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
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
            RenderTransform = _scaleTransform;

            DoubleAnimation wAnim = new DoubleAnimation() {
                By = widthBy,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
            };

            DoubleAnimation hAnim = new DoubleAnimation() {
                By = heightBy,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
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

        public void Move(double left, double top, TimeSpan duration, long delay) {
            Vector offset = VisualTreeHelper.GetOffset(this);
            double oldLeft = offset.X;
            double oldTop = offset.Y;

//            Point transform = TransformToAncestor((Visual) Parent).Transform(new Point(0, 0));
//            Point transform = TransformToVisual((Visual) Parent).Transform(((Visual) Parent).TransformToAncestor((Visual) Parent).Transform(new Point()));

//            Console.WriteLine("Move: left: {0}, top: {1}", transform.X, transform.Y);

            RenderTransform = new TranslateTransform();

            DoubleAnimation xAnim = new DoubleAnimation(oldLeft, left, duration) {
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
                BeginTime = TimeSpan.FromMilliseconds(delay),
                FillBehavior = FillBehavior.Stop
            };

            DoubleAnimation yAnim = new DoubleAnimation(oldTop, top, duration) {
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
                BeginTime = TimeSpan.FromMilliseconds(delay),
                FillBehavior = FillBehavior.Stop
            };

            Storyboard.SetTarget(xAnim, this);
            Storyboard.SetTargetProperty(xAnim, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTarget(yAnim, this);
            Storyboard.SetTargetProperty(yAnim, new PropertyPath("(Canvas.Top)"));

            Storyboard moveStoryboard = new Storyboard();
            moveStoryboard.Children.Add(xAnim);
            moveStoryboard.Children.Add(yAnim);

            moveStoryboard.Completed += (sender, args) => {
                Canvas.SetLeft(this, left);
                Canvas.SetTop(this, top);
            };

            moveStoryboard.Begin();
        }
    }
}