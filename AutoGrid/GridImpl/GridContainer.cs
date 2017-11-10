using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AutoGrid.GridImpl {
    public class GridContainer : UniformGrid {
        public void HandleItemDrag(IGridItem gridItem) {
            MoveItem(gridItem);
        }

        private void MoveItem(IGridItem gridItem) {
            GridControl targetGrid = GetTargetGrid();
            // TODO check if current parent and targetGrid are the same
            // if yes, drop to the original position

            // TODO relayout existing items to accomodate the new one

            int newItemCount = targetGrid.Items.Count + 1;
            Console.WriteLine("new item count: {0}", newItemCount);
            targetGrid.RecalculateGrid(newItemCount);

            Rect itemRect = targetGrid.Grid.GetItemRect(newItemCount - 1);

            double top = itemRect.Top;
            double left = itemRect.Left;

            Vector offset = VisualTreeHelper.GetOffset(gridItem);

            double oldLeft = !Double.IsNaN(Canvas.GetLeft(gridItem)) ? Canvas.GetLeft(gridItem) : offset.X;
            double oldTop = !Double.IsNaN(Canvas.GetTop(gridItem)) ? Canvas.GetTop(gridItem) : offset.Y;

            Point gridItemPt = gridItem.TranslatePoint(new Point(left, top), targetGrid);
            Point gridItemEnds = gridItem.TranslatePoint(new Point(itemRect.Right, itemRect.Bottom), targetGrid);

            gridItem.RenderTransform = gridItem.TranslateTransform;

            DoubleAnimation xAnim = new DoubleAnimation(oldLeft - gridItemPt.X + left * 2, TimeSpan.FromMilliseconds(1000)) {
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
                BeginTime = TimeSpan.FromMilliseconds(0),
                FillBehavior = FillBehavior.Stop
            };

            DoubleAnimation yAnim = new DoubleAnimation(oldTop - gridItemPt.Y + top * 2, TimeSpan.FromMilliseconds(1000)) {
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
                BeginTime = TimeSpan.FromMilliseconds(0),
                FillBehavior = FillBehavior.Stop
            };

            Storyboard.SetTarget(xAnim, gridItem);
            Storyboard.SetTargetProperty(xAnim, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTarget(yAnim, gridItem);
            Storyboard.SetTargetProperty(yAnim, new PropertyPath("(Canvas.Top)"));

            gridItem.RenderTransform = gridItem.ScaleTransform;


            // TODO fix this sizing issue
            DoubleAnimation wAnim = new DoubleAnimation() {
                To = (itemRect.Right - itemRect.Left) / gridItem.ActualWidth,
                Duration = new Duration(TimeSpan.FromMilliseconds(1000)),
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
            };

            DoubleAnimation hAnim = new DoubleAnimation() {
                To = (itemRect.Bottom - itemRect.Top) / gridItem.ActualHeight,
                Duration = new Duration(TimeSpan.FromMilliseconds(1000)),
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
            };

            Storyboard.SetTarget(hAnim, gridItem);
            Storyboard.SetTargetProperty(hAnim, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(wAnim, gridItem);
            Storyboard.SetTargetProperty(wAnim, new PropertyPath("RenderTransform.ScaleY"));

            Storyboard moveStoryboard = new Storyboard();
            moveStoryboard.Children.Add(xAnim);
            moveStoryboard.Children.Add(yAnim);
//            moveStoryboard.Children.Add(wAnim);
//            moveStoryboard.Children.Add(hAnim);

            moveStoryboard.Completed += (sender, args) => {
                Console.WriteLine("Animation complete");
                Canvas.SetLeft(gridItem, left);
                Canvas.SetTop(gridItem, top);
                gridItem.Width = itemRect.Right - itemRect.Left;
                gridItem.Height = itemRect.Bottom - itemRect.Top;
                gridItem.GetParent().Children.Remove(gridItem);
                targetGrid.Add(gridItem);
            };

            moveStoryboard.Begin();
        }

        public GridControl GetTargetGrid() {
            return (GridControl) Children[CurrentMouseOverIndex];
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e) {
            Point position = e.GetPosition(this);

            double heightPerItem = ActualHeight / Rows;
            double widthPerItem = ActualWidth / Columns;

//            Console.WriteLine("Posi: left: {0}, top: {1}", position.X, position.Y);

            int col = (int) Math.Floor(position.X / widthPerItem);
            int row = (int) Math.Floor(position.Y / heightPerItem);

            // Check because when you're dragging an item
            // all weird stuff starts to happen
            if (col >= Columns || row >= Rows || col < 0 || row < 0) {
                return;
            }

            int index = col + row * Columns;

            ((GridControl) Children[index]).Background = Brushes.Blue;

            for (var i = 0; i < Children.Count; i++) {
                if (i != index) {
                    ((GridControl) Children[i]).Background = Brushes.Transparent;
                }
            }

//            Console.WriteLine("At Row: {0}, Col: {1}, index: {2}", row, col, index);

            CurrentMouseOverIndex = index;

            base.OnPreviewMouseMove(e);
        }

        public int CurrentMouseOverIndex { get; set; }
    }
}