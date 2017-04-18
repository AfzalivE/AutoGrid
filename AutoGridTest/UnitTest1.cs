using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using AutoGrid;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grid = AutoGrid.Grid;

namespace AutoGridTest {
    [TestClass]
    public class UnitTest1 {
        private GridAdapter _gridAdapter;

        [TestInitialize]
        public void setup() {
            _gridAdapter = new GridAdapter();
            _gridAdapter.RenderSize = new Size(500, 500);
        }

        [TestMethod]
        public void TestSetItemsGrid() {
            List<MyItem> items = new List<MyItem>();
            for (int i = 0; i < 12; i++) {
                items.Add(new MyItem());
            }

            _gridAdapter.SetItems(items);

            // 12 items means grid should be 3 x 4
            Grid adapterGrid = _gridAdapter.Grid;

            Assert.AreEqual(adapterGrid, new Grid(3, 4));
        }

        [TestMethod]
        public void TestSetItemsLayout() {
            List<MyItem> items = new List<MyItem>();
            for (int i = 0; i < 12; i++) {
                items.Add(new MyItem());
            }

            _gridAdapter.SetItems(items);

            // 12 items means grid should be 3 x 4
            Assert.AreEqual(_gridAdapter.Grid, new Grid(3, 4));
            List<MyItem> myItems = _gridAdapter.GetItems();

            // so each item's width = 500/4 cols = 125
            // height = 500/3 rows = 166.666667

            myItems.ForEach(item => {
                Assert.AreEqual(item.Width, 125);
                Assert.AreEqual(166.5, item.Height, 0.5);
            });

            for (int row = 0; row < 3; row++) {
                for (int col = 0; col < 4; col++) {
                    MyItem myItem = items[col + row];

//                    Canvas.GetLeft(myItem)
                }
            }
        }
    }
}