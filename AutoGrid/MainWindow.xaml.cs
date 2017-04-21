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

        public void StoreInTemp(MyItem item) {
            Temp.Children.Add(item);
        }

        public void RetrieveFromTemp() {
            if (Temp.Children.Count > 0) {
                MyItem tempChild = (MyItem) Temp.Children[0];
                Temp.Children.Remove(tempChild);
                ((GridAdapter) MainGrid.Children[CurrentMouseOverIndex]).Add(tempChild);
            }
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e) {
            Point position = e.GetPosition(this);

            double heightPerItem = MainGrid.ActualHeight / MainGrid.Rows;
            double widthPerItem = MainGrid.ActualWidth / MainGrid.Columns;

//            Console.WriteLine("Posi: left: {0}, top: {1}", position.X, position.Y);

            int col = (int) Math.Floor(position.X / widthPerItem);
            int row = (int) Math.Floor(position.Y / heightPerItem);

            // Check because when you're dragging an item
            // all weird stuff starts to happen
            if (col >= MainGrid.Columns || row >= MainGrid.Rows || col < 0 || row < 0) {
                return;
            }

            int index = col + row * MainGrid.Columns;

            ((GridAdapter) MainGrid.Children[index]).Background = Brushes.Blue;

            for (var i = 0; i < MainGrid.Children.Count; i++) {
                if (i != index) {
                    ((GridAdapter) MainGrid.Children[i]).Background = Brushes.Transparent;
                }
            }

//            Console.WriteLine("At Row: {0}, Col: {1}", row, col);

            CurrentMouseOverIndex = index;

            base.OnPreviewMouseMove(e);
        }

        public int CurrentMouseOverIndex { get; set; }

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

//            for (var j = 1; j < MainGrid.Children.Count; j++) {
//                var items = new List<MyItem>();
//                for (int i = 0; i < 10; i++) {
//                    MyItem myItem = new MyItem {
//                        Content = $"Str {i}"
//                    };
//
//                    items.Add(myItem);
//                }
//
//                ((GridAdapter) MainGrid.Children[j]).SetItems(items);
//            }

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

//            for (int i = 1; i < 3; i++) {
//                manipulateGrid(i * 3000);
//            }
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