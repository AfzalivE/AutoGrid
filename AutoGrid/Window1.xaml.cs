using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AutoGrid {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {
        public Window1() {
            InitializeComponent();

            Loaded += OnWindowLoaded;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e) {
            ShowPosition();

            Canvas.SetLeft(grandchild, 0);
            Canvas.SetTop(grandchild, 0);
            Canvas.SetRight(grandchild, 0);
            Canvas.SetBottom(grandchild, 0);

            Move(2000);

            ShowPosition();
        }

        private void MoveGc() {
//            Canvas.SetLeft(grandchild, 10);
//            Canvas.SetTop(grandchild, 10);

            double left = -10;
            double top = -10;
            Vector offset = VisualTreeHelper.GetOffset(grandchild);
            //            MainWindow currentMainWindow = (MainWindow) Application.Current.MainWindow;
            //            GridAdapter parent = (GridAdapter) currentMainWindow.GetTargetGrid(this);
            //            Console.WriteLine("Parent: {0}", parent.Children.Count);
            //            Point offset = TranslatePoint(new Point(0, 0), (UIElement) Parent);
            double oldLeft = !Double.IsNaN(Canvas.GetLeft(grandchild)) ? Canvas.GetLeft(grandchild): offset.X;
            double oldTop = !Double.IsNaN(Canvas.GetTop(grandchild)) ? Canvas.GetTop(grandchild) : offset.Y;

            Point gcChild1 = grandchild.TranslatePoint(new Point(left, top), child1);
            Console.WriteLine("Grandchild {0}, {1} from Child1", gcChild1.X, gcChild1.Y);

            //            Point transform = TransformToVisual((Visual) Parent).Transform(((Visual) Parent).TransformToAncestor((Visual) Parent).Transform(new Point()));

                        Console.WriteLine("Move: {0}, {1}", oldLeft, oldTop);

            RenderTransform = new TranslateTransform();

            DoubleAnimation xAnim = new DoubleAnimation(-gcChild1.X + oldLeft, TimeSpan.FromMilliseconds(1000)) {
                EasingFunction = new PowerEase {EasingMode = EasingMode.EaseOut},
                BeginTime = TimeSpan.FromMilliseconds(500),
                FillBehavior = FillBehavior.Stop
            };

            DoubleAnimation yAnim = new DoubleAnimation(-gcChild1.Y + oldTop, TimeSpan.FromMilliseconds(1000)) {
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut },
                BeginTime = TimeSpan.FromMilliseconds(500),
                FillBehavior = FillBehavior.Stop
            };

            Storyboard.SetTarget(xAnim, grandchild);
            Storyboard.SetTargetProperty(xAnim, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTarget(yAnim, grandchild);
            Storyboard.SetTargetProperty(yAnim, new PropertyPath("(Canvas.Top)"));

            Storyboard moveStoryboard = new Storyboard();
            moveStoryboard.Children.Add(xAnim);
            moveStoryboard.Children.Add(yAnim);

            moveStoryboard.Completed += (sender, args) => {
                Console.WriteLine("Animation complete");
                Canvas.SetLeft(grandchild, -left);
                Canvas.SetTop(grandchild, -top);
                child2.Children.RemoveAt(0);
                child1.Children.Add(grandchild);

            };

            moveStoryboard.Begin();
        }

        public async Task Move(int delay) {
            await Task.Delay(delay);

            MoveGc();

        }

        private void ShowPosition() {
            Point translatePoint = father.TranslatePoint(new Point(0, 0), (UIElement) Parent);
            Console.WriteLine("Father {0}, {1}", translatePoint.X, translatePoint.Y);

            Point child1Father = child1.TranslatePoint(new Point(0, 0), (UIElement) Parent);
            Console.WriteLine("Child1 {0}, {1} from Father", child1Father.X, child1Father.Y);

            Point child2Father = child2.TranslatePoint(new Point(0, 0), (UIElement) Parent);
            Console.WriteLine("Child2 from Parent {0}, {1}", child2Father.X, child2Father.Y);

            Point child2Child1 = child2.TranslatePoint(new Point(0, 0), (UIElement) child1);
            Console.WriteLine("Child2 {0}, {1} from Child1", child2Child1.X, child2Child1.Y);

            Point gcChild1 = grandchild.TranslatePoint(new Point(0, 0), child1);
            Console.WriteLine("Grandchild {0}, {1} from Child1", gcChild1.X, gcChild1.Y);

            Point gcChild2 = grandchild.TranslatePoint(new Point(0, 0), child2);
            Console.WriteLine("Grandchild {0}, {1} from Child2", gcChild2.X, gcChild2.Y);
        }
    }
}