using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace AutoGrid {
    public abstract class IGridItem : Thumb {
        private TranslateTransform _translateTransform;
        private ScaleTransform _scaleTransform;

        public string Content { get; set; }

        public IGridItem() {
            BorderThickness = new Thickness(2);
            BorderBrush = Brushes.Blue;
            _translateTransform = new TranslateTransform();
            _scaleTransform = new ScaleTransform(1, 1);
            RenderTransformOrigin = new Point(0.5, 0.5);

            DragDelta += OnDragDelta;
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs e) {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            Canvas.SetRight(this, Canvas.GetRight(this) + e.HorizontalChange);
            Canvas.SetBottom(this, Canvas.GetBottom(this) + e.VerticalChange);
        }

        public void SetSize(Rect rect) {
            this.Width = rect.Right - rect.Left;
            this.Height = rect.Bottom - rect.Top;

            Canvas.SetLeft(this, rect.Left);
            Canvas.SetRight(this, rect.Right);
            Canvas.SetTop(this, rect.Top);
            Canvas.SetBottom(this, rect.Bottom);

            Console.WriteLine(rect.ToString());
        }
    }
}