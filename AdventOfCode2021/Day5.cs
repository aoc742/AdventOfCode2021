using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day5
    {
        private int _numCols = 100000;

        public void Start()
        {
            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day5Input.txt");

            Part1(lines);
            Part2(lines);
        }

        public void Part1(string[] lines)
        {
            Dictionary<int, int> points = new Dictionary<int, int>();

            int numPointsWithAtLeast2Lines = 0;

            foreach (var line in lines)
            {
                string[] separators = { ",", " -> " };
                string[] parts = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                int x0 = int.Parse(parts[0]);
                int y0 = int.Parse(parts[1]);
                int x1 = int.Parse(parts[2]);
                int y1 = int.Parse(parts[3]);

                // Part 1
                //only consider horizontal and vertical lines for now
                if (x0 == x1 || y0 == y1)
                {
                    // get list of points between x0,y0 and x1,y1
                    for (int x = Math.Min(x0, x1); x <= Math.Max(x0, x1); x++)
                    {
                        for (int y = Math.Min(y0, y1); y <= Math.Max(y0, y1); y++)
                        {
                            int index = this.GetIndex(y, x);
                            if (points.ContainsKey(index))
                            {
                                // already in the set, meaning it now has 2
                                ++points[index];

                                if (points[index] == 2)
                                {
                                    ++numPointsWithAtLeast2Lines;
                                }
                            }
                            else
                            {
                                points[index] = 1;
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Part 1: Number points with two or more lines: {numPointsWithAtLeast2Lines}");
        }

        public void Part2(string[] lines)
        {
            Dictionary<int, int> points = new Dictionary<int, int>();

            int numPointsWithAtLeast2Lines = 0;

            foreach (var line in lines)
            {
                string[] separators = { ",", " -> " };
                string[] parts = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                int x0 = int.Parse(parts[0]);
                int y0 = int.Parse(parts[1]);
                int x1 = int.Parse(parts[2]);
                int y1 = int.Parse(parts[3]);

                // Part 2
                List<int> indices = this.GetPointsInLine(x0, y0, x1, y1);
                foreach (int index in indices)
                {
                    if (points.ContainsKey(index))
                    {
                        // already in the set, meaning it now has 2
                        ++points[index];

                        if (points[index] == 2)
                        {
                            ++numPointsWithAtLeast2Lines;
                        }
                    }
                    else
                    {
                        points[index] = 1;
                    }
                }
            }

            Console.WriteLine($"Part 2: Number points with two or more lines: {numPointsWithAtLeast2Lines}");
        }

        /// <summary>
        /// Returns list of indices representing points that fall along a line.
        /// index values returned correspond to index position in 2D array
        /// </summary>
        public List<int> GetPointsInLine(int x0, int y0, int x1, int y1)
        {
            List<int> points = new List<int>();

            // get vertical or horizontal lines
            if (x0 == x1 || y0 == y1)
            {
                // get list of points between x0,y0 and x1,y1
                for (int x = Math.Min(x0, x1); x <= Math.Max(x0, x1); x++)
                {
                    for (int y = Math.Min(y0, y1); y <= Math.Max(y0, y1); y++)
                    {
                        points.Add(this.GetIndex(y, x));
                    }
                }
            }
            else
            {
                // get diagonal lines
                //int slope = Math.Min(x0, x1) == x0 ? (y1 - y0) / (x1 - x0) : (y0 - y1) / (x0 - x1);

                int slope = (y1 - y0) / (x1 - x0);
                int b = y0 - (slope * x0);

                for (int x = Math.Min(x0, x1); x <= Math.Max(x0, x1); x++)
                {
                    int y = slope * x + b;
                    points.Add(this.GetIndex(y, x));
                }
            }


            //throw new NotImplementedException();

            return points;
        }

        public int GetIndex(int y, int x)
        {
            return this._numCols * y + x;
        }
    }
}
