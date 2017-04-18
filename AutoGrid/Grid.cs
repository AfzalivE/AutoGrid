using System;

namespace AutoGrid {
    public class Grid {
        public readonly int Rows;
        public readonly int Cols;

        public Grid(int rows, int cols) {
            this.Rows = rows;
            this.Cols = cols;
        }

        public static Grid Recalculate(int count) {
            //  handle zero case
            int cols = 1;
            int rows = 1;
            if (count == 0) {
                return new Grid(cols, rows);
            }

            cols = (int) Math.Ceiling(Math.Sqrt(count));
            rows = count > cols * (cols - 1) ? cols : cols - 1;

            Console.WriteLine("For {2} = Grid: {0} x {1}", rows, cols, count);

            return new Grid(rows, cols);
        }

        public override bool Equals(object obj) {
            Grid grid = (Grid) obj;
            return grid != null && Rows == grid.Rows && Cols == grid.Cols;
        }
    }
}