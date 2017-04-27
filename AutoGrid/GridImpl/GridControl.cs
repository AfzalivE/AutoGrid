using System;
using System.Collections.Generic;
using System.Windows;

namespace AutoGrid.GridImpl {
    public class GridControl : IGridControl {

        public override void SetItems(List<IGridItem> items) {
            Items.Clear();
            foreach (IGridItem t in items) {
                Items.Add(t);
            }

            OnItemsChanged();
        }

        public override void OnItemsChanged() {
            // calculate future item sizes

            Grid.Recalculate(Items.Count, RenderSize.Width, RenderSize.Height, TransformToAncestor(this).Transform(new Point(0, 0)));
            for (var i = 0; i < Items.Count; i++) {
                Rect itemRect = Grid.GetItemRect(i);
                Items[i].SetSize(itemRect);
            }

            Children.Clear();

            Items.ForEach(item => {
                Children.Add(item);
            });
        }

        public override void Add() {
            throw new System.NotImplementedException();
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
    }
}