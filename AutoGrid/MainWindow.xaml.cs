using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoGrid {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private List<MyItem> _items;

        public MainWindow() {
            InitializeComponent();

            Loaded += OnWindowLoaded;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e) {
//            ItemGrid.RenderSize = new Size(RenderSize.Width, RenderSize.Height);

            _items = new List<MyItem>();
            for (int i = 0; i < 10; i++) {
                MyItem myItem = new MyItem {
                    Content = $"Str {i}"
                };

                _items.Add(myItem);
            }

            ItemGrid.SetItems(_items);

            var items = new List<MyItem>();
            for (int i = 0; i < 10; i++) {
                MyItem myItem = new MyItem {
                    Content = $"Str {i}"
                };

                items.Add(myItem);
            }

            ItemGrid2.SetItems(items);

            //ItemGrid.Remove(0);
            //ItemGrid.Remove(3);
            //ItemGrid.Remove(2);
            //ItemGrid.Remove(5);
            //ItemGrid.Remove(0);
            //ItemGrid.Remove(0);
            //ItemGrid.Remove(0);
            //ItemGrid.Remove(0);

//            Console.WriteLine("Width: {0}, Height: {1}", Width, Height);
//            Console.WriteLine("Width: {0}, Height: {1}", ItemGrid.Width, ItemGrid.Height);
//
//            MovingItem.MoveObject(0.0, 0.0, new TimeSpan(0, 0, 0, 0, 400), 500);
//
//            MovingItem.ShrinkObject();
//            MovingItem.GrowObject();

            for (int i = 1; i < 3; i++) {
                manipulateGrid(i * 3000);
            }
        }

        public async Task manipulateGrid(int delay) {
            await Task.Delay(delay);
//            MainGrid.Children.Add(new Button());
//            MainGrid.Columns = 4;
            if (delay < 5000) {
                ItemGrid.DontRemove();
            } else {
                ItemGrid.Add(new MyItem {
                    Content = $"Str 321"
                });
//                ItemGrid.DontRemove();
//                ItemGrid.GetItems()[0].Show();
            }
        }
    }
}