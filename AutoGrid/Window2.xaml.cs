using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AutoGrid.GridImpl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoGrid {
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window {
        private List<IGridItem> _items;

        public Window2() {
            InitializeComponent();
            Loaded += OnWindowLoaded;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e) {
            Tests.CheckTempSize(Temp, Width, Height);

            Setup();
        }

        private void Setup() {
            _items = new List<IGridItem>();
            for (int i = 0; i < 10; i++) {
                GridItem item = new GridItem {
                    Content = $"Str {i}"
                };

                _items.Add(item);
            }

            ItemGrid2.SetItems(_items);
        }
    }

    internal static class Tests {
        public static void CheckTempSize(Canvas temp, double width, double height) {
            Vector offset = VisualTreeHelper.GetOffset(temp);
            Assert.AreEqual(offset.X, 0);
            Assert.AreEqual(offset.Y, 0);

            Assert.IsTrue(temp.RenderSize.Width > 0);
            Assert.IsTrue(temp.RenderSize.Height > 0);

//            Console.WriteLine("left:{0}, top:{1}, right:{2}, bottom:{3}",
//                              offset.X, offset.Y, temp.RenderSize.Width, temp.RenderSize.Height);
        }
    }
}
