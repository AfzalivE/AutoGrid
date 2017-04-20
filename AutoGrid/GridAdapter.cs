using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AutoGrid {
    public class GridAdapter : Canvas {
        private List<MyItem> _items { get; set; }
        public Grid Grid;
        private Tuple<int, MyItem> _itemForRemoval;

        static GridAdapter() {
//            DefaultStyleKeyProperty.OverrideMetadata(typeof(GridAdapter), new FrameworkPropertyMetadata(typeof(GridAdapter)));
        }

        public GridAdapter() {
            _items = new List<MyItem>();
            Grid = new Grid(1, 1);
        }

        public void SetItems(List<MyItem> items) {
            _items.Clear();
            for (var i = 0; i < items.Count; i++) {
                Console.WriteLine("Added item {0}", i);
                _items.Add(items[i]);
            }
            OnItemsChanged();
        }

        private void OnItemsChanged() {
            Grid = Grid.Recalculate(_items.Count);
            RecalculateItemSize();

            Console.WriteLine("ItemsChanged");

            this.Children.Clear();

            foreach (MyItem t in _items) {
                this.Children.Add(t);
            }
        }

        private void RecalculateItemSize() {
            double parentWidth = RenderSize.Width;
            double parentHeight = RenderSize.Height;
//            Console.WriteLine("{0} x {1}", parentWidth, parentHeight);
            Point origin = TransformToAncestor(this).Transform(new Point(0, 0));
            double x = origin.X;
            double y = origin.Y;
            // figure out width and height per item
            double widthPerItem = parentWidth / Grid.Cols;
            double heightPerItem = parentHeight / Grid.Rows;

            IEnumerator<MyItem> e = _items.GetEnumerator();

            for (int row = 0; row < Grid.Rows; row++) {
                for (int col = 0; col < Grid.Cols; col++) {
                    if (!e.MoveNext()) {
                        e.Dispose();
                        break;
                    }

                    MyItem item = e.Current;
                    if (item == null) {
                        Console.WriteLine("Item was null");
                        continue;
                    }

                    int left = (int) (col * widthPerItem + x);
                    int top = (int) (row * heightPerItem + y);
                    int right = (int) ((col + 1) * widthPerItem + x);
                    int bottom = (int) ((row + 1) * heightPerItem + y);

                    item.SetContainerRect(left, top, right, bottom, true);
                }
            }
        }

        public void Add(MyItem item) {
            _items.Add(item);
            OnItemAdded();
        }

        private void OnItemAdded() {
            OnItemsChanged();
//            Grid = Grid.Recalculate(_items.Count);
//            RecalculateItemSize();
//            this.Children.Add(_items[_items.Count - 1]);
//
//            Console.WriteLine("Item added");
        }

        public void Remove(int position) {
            MyItem itemForRemoval = _items[position];
            itemForRemoval.Hide((sender, args) => {
                _items.RemoveAt(position);
                OnItemRemoved(position);
            });
        }

        public void MaybeRemove(int position) {
            _itemForRemoval = new Tuple<int, MyItem>(position, _items[position]);
            Remove(position);
        }

        public void DontRemove() {
            if (_itemForRemoval == null) {
                return;
            }

            _items.Insert(_itemForRemoval.Item1, _itemForRemoval.Item2);
            _itemForRemoval.Item2.Show();
            OnItemsChanged();
            _itemForRemoval = null;
        }

        private void OnItemRemoved(int position) {
            OnItemsChanged();
//            Grid = Grid.Recalculate(_items.Count);
//            this.Children.RemoveAt(position);
//            RecalculateItemSize();
//
//            Console.WriteLine("ItemsRemoved");
        }

        public void Remove(MyItem item) {
            int index = _items.IndexOf(item);
            Remove(index);
        }

        public void MaybeRemove(MyItem item) {
            int index = _items.IndexOf(item);
            MaybeRemove(index);
        }

        public List<MyItem> GetItems() {
            return _items;
        }

        protected override Size MeasureOverride(Size constraint) {
            Console.WriteLine("Measure");
            return base.MeasureOverride(constraint);
        }
    }
}