using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AutoGrid {
    public class MyThumb : Thumb {

        static MyThumb() {

        }

        public MyThumb() {
            DragDelta += OnDragDelta;
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs e) {
            Console.WriteLine("Mouse is dragging");
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            Canvas.SetRight(this, Canvas.GetRight(this) + e.HorizontalChange);
            Canvas.SetBottom(this, Canvas.GetBottom(this) + e.VerticalChange);
        }
    }
}