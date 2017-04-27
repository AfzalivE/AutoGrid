using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutoGrid {
    public abstract class IGridControl : Canvas {
        protected List<IGridItem> Items;
        protected GridSize Grid;

        public IGridControl() {
            Items = new List<IGridItem>();
            Grid = new GridSize(1, 1);

            Background = Brushes.Transparent;
        }

        // to set a list of items at once
        public abstract void SetItems(List<IGridItem> items);

        // when all items in a list are set at once
        public abstract void OnItemsChanged();

        // when a new item is added, which wasn't present in another GridControl before
        public abstract void Add();

        // when an item is removed completely
        // i.e. when it has been moved to another GridControl, or deleted
        public abstract void Remove();

        // when an item is added from another GridControl
        // do position calculations
        // and remove this item from the other GridControl when appropriate
        public abstract void AddFrom(IGridControl gridControl, IGridItem item);

        // recalculate and set position for all items
        public abstract void SetAllItemSizes();
    }
}