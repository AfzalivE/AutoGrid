using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using AutoGrid.GridImpl;

namespace AutoGrid {
    public abstract class IGridItem : Thumb {
        public TranslateTransform TranslateTransform;
        public ScaleTransform ScaleTransform;
        private bool _isDragged;
        private bool _isMouseDown;

        public string Content { get; set; }

        public IGridItem() {
            BorderThickness = new Thickness(2);
            BorderBrush = Brushes.Blue;
            TranslateTransform = new TranslateTransform();
            ScaleTransform = new ScaleTransform(1, 1);
//            RenderTransformOrigin = new Point(0.5, 0.5);

            DragDelta += OnDragDelta;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e) {
            if (!_isMouseDown) {
                return;
            }

            _isMouseDown = false;
            if (!_isDragged) {
                // not dragged
            } else {
                GetParent().HandleItemDrag(this);
            }

            MoveToBottom();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            if (e.Handled) {
                return;
            }

            MoveToTop();

            base.OnMouseLeftButtonDown(e);
            e.Handled = true;
            _isMouseDown = true;
            _isDragged = false;
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs e) {
            _isDragged = true;
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

        private void MoveToTop() {
            GetParent().MoveToTop();
            Panel.SetZIndex(this, Panel.GetZIndex(GetParent()));
        }

        public void MoveToBottom() {
            GetParent().MoveToBottom();
            Panel.SetZIndex(this, Panel.GetZIndex(GetParent()));
        }

        public GridControl GetParent() {
            return ((GridControl) Parent);
        }
    }
}