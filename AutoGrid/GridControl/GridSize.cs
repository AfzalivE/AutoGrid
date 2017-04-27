using System;
using System.Windows;

namespace AutoGrid {
    public class GridSize {
        public int Rows;
        public int Cols;
        private double _widthPerItem;
        private double _heightPerItem;
        private double _originX;
        private double _originY;

        public GridSize(int rows, int cols) {
            this.Rows = rows;
            this.Cols = cols;
        }

        public void Recalculate(int count, double width, double height, Point origin) {
            //  handle zero case
            int cols = 1;
            int rows = 1;
            if (count == 0) {
                Cols = cols;
                Rows = rows;
                return;
            }

            cols = (int) Math.Ceiling(Math.Sqrt(count));
            rows = count > cols * (cols - 1) ? cols : cols - 1;

            Console.WriteLine("For {2} = Grid: {0} x {1}", rows, cols, count);

            Cols = cols;
            Rows = rows;

            RecalculateItemSize(width, height, origin);
        }

        private void RecalculateItemSize(double width, double height, Point origin) {
            _originX = origin.X;
            _originY = origin.Y;
            // figure out width and height per item
            _widthPerItem = width / Cols;
            _heightPerItem = height / Rows;
        }

        public Rect GetItemRect(int position) {
            int row = position / Cols;
            int col = position  - row * Cols;

            Console.WriteLine("row: {0}, col: {1}", row, col);

            int left = (int) (col * _widthPerItem + _originX);
            int top = (int) (row * _heightPerItem + _originY);
            int right = (int) ((col + 1) * _widthPerItem + _originX);
            int bottom = (int) ((row + 1) * _heightPerItem + _originY);

            // x, y, width, height
            return new Rect(left, top, right - left, bottom - top);
        }
    }
}