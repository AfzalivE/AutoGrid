using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AutoGrid.GridImpl {
    public class GridControl : IGridControl {
        private int _defaultZIndex;

        public GridControl() {
            _defaultZIndex = Panel.GetZIndex(this);
        }

        public override void SetItems(List<IGridItem> items) {
            Items.Clear();
            foreach (IGridItem t in items) {
                Items.Add(t);
            }

            OnItemsChanged();
        }

        public override void OnItemsChanged() {
            // calculate future item sizes

            RecalculateGrid(Items.Count);
            for (var i = 0; i < Items.Count; i++) {
                Rect itemRect = Grid.GetItemRect(i);
                Items[i].SetSize(itemRect);
            }

            Children.Clear();

            Items.ForEach(item => {
                Children.Add(item);
            });
        }

        public void RecalculateGrid(int itemsCount) {
            Grid.Recalculate(itemsCount, RenderSize.Width, RenderSize.Height, TransformToAncestor(this).Transform(new Point(0, 0)));
        }

        public override void Add(IGridItem gridItem) {
            Items.Add(gridItem);
            Children.Add(gridItem);
        }

        public override void Remove() {
            throw new System.NotImplementedException();
        }

        public override void AddFrom(IGridControl gridControl, IGridItem item) {
            throw new System.NotImplementedException();
        }

        public override void SetAllItemSizes() {
            throw new System.NotImplementedException();
        }

        public void HandleItemDrag(IGridItem gridItem) {
            GetParent().HandleItemDrag(gridItem);
        }

        public void MoveToTop() {
            Panel.SetZIndex(this, 999);
        }

        public void MoveToBottom() {
            Panel.SetZIndex(this, _defaultZIndex);
        }

        public GridContainer GetParent() {
            return (GridContainer) Parent;
        }
    }
}