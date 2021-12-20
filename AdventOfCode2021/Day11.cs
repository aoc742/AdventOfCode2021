using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day11
    {
        private List<int> _grid = new List<int>();
        private List<bool> _flashed = new List<bool>();
        private int _numRows;
        private int _numCols;
        private ulong _numFlashes = 0;
        private int _day = 1;
        private bool _allFlashedSameDay;

        /// <summary>
        /// Scenario:
        /// 100 octopuses arranged in 10x10 grid.
        /// Each represented by a number 0-9, which is its energy level
        /// Each slowly gains energy over time (0-9 nrg levels) and
        /// flashes brightly for a moment when its energy is full, then resets back to 0.
        /// 
        /// Goal: Identify how many total flashes occur after 100 steps
        /// </summary>
        public void Start()
        {
            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day11Input.txt");
            foreach(string line in lines)
            {
                foreach(char c in line)
                {
                    this._grid.Add(c - '0'); // converts char to int
                    this._flashed.Add(false);
                }
            }

            this._numRows = lines.Length;
            this._numCols = lines[0].Length;

            this._day = 1;
            while (this._day <= 100)
            {
                this.Step();
                this._day++;
            }

            Console.WriteLine($"Part 1: Num flashes after {this._day - 1} days: {this._numFlashes}");

            // Part 2 - Keep going until all octopus flash on the same day.
            while (true)
            {
                this.Step();
                this._day++;

                if (this._allFlashedSameDay)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// During a single step, the following occurs:
        /// 1. energy level of each octopus += 1
        /// 2. Any octopus w/ an nrg level > 9 FLASHES
        ///     2a. All adjacent octopuses nrg += 1 (including diagonal)
        ///     2b. If an adjacent octopus nrg level > 9: also FLASHES
        ///     2c. Continue process as long as new octopuses keep flashing
        ///     (octopus can only flash once per step)
        /// 3. Any octopus that flashed during this step has its energy set to 0
        /// </summary>
        private void Step()
        {
            // 1. energy level++
            // also reset flash states before beginning step
            for(int i = 0; i < this._grid.Count(); i++)
            {
                this._grid[i] += 1;
                this._flashed[i] = false;
            }

            // 2. update flashes
            int index = 0;
            while (index < this._grid.Count())
            {
                if (this._grid[index] > 9 && !this._flashed[index])
                {
                    this._flashed[index] = true;
                    this._numFlashes++;
                    List<int> adjacents = this.GetAdjacentIndices(index);
                    foreach(int adjacent in adjacents)
                    {
                        this._grid[adjacent] += 1;
                    }
                    index = 0; // restart back at start with the new numbers. slow, but easy and effective.
                }
                else
                {
                    index++;
                }
            }

            // 3. Reset flashed octopus to 0
            this._allFlashedSameDay = true;
            for (int i = 0; i < this._grid.Count(); ++i)
            {
                if (this._flashed[i])
                {
                    this._grid[i] = 0;
                }
                else
                {
                    this._allFlashedSameDay = false;
                }
            }

            if (this._allFlashedSameDay)
            {
                Console.WriteLine($"All flashed on day {this._day}");
                return;
            }
        }

        private bool TryGoLeft(int inputIndex, out int outputIndex)
        {
            if (this.GetCol(inputIndex) == 0)
            {
                outputIndex = -1;
                return false;
            }

            outputIndex = inputIndex - 1;
            return true;

        }

        private bool TryGoRight(int inputIndex, out int outputIndex)
        {
            if (this.GetCol(inputIndex) == this._numCols - 1)
            {
                outputIndex = -1;
                return false;
            }
            outputIndex = inputIndex + 1;
            return true;
        }

        private bool TryGoUp(int inputIndex, out int outputIndex)
        {
            int row = this.GetRow(inputIndex);
            int col = this.GetCol(inputIndex);
            if (row == 0)
            {
                outputIndex = -1;
                return false;
            }
            outputIndex = this._numCols * (row - 1) + col;
            return true;
        }

        private bool TryGoDown(int inputIndex, out int outputIndex)
        {
            int row = this.GetRow(inputIndex);
            int col = this.GetCol(inputIndex);
            if (row == this._numRows - 1)
            {
                outputIndex = -1;
                return false;
            }
            outputIndex = this._numCols * (row + 1) + col;
            return true;
        }

        private List<int> GetAdjacentIndices(int inputIndex)
        {
            // Get adjacent positions
            List<int> nodes = new List<int>();

            int leftIndex, rightIndex, upIndex, downIndex;
            bool left = false, right = false, up = false, down = false;
            if (TryGoLeft(inputIndex, out leftIndex))
            {
                nodes.Add(leftIndex);
                left = true;
            }
            if (TryGoRight(inputIndex, out rightIndex))
            {
                nodes.Add(rightIndex);
                right = true;
            }
            if (TryGoUp(inputIndex, out upIndex))
            {
                nodes.Add(upIndex);
                up = true;
            }
            if (TryGoDown(inputIndex, out downIndex))
            {
                nodes.Add(downIndex);
                down = true;
            }

            if (up && left) nodes.Add(upIndex - 1);
            if (up && right) nodes.Add(upIndex + 1);
            if (down && left) nodes.Add(downIndex - 1);
            if (down && right) nodes.Add(downIndex + 1);

            return nodes;
        }

        private int GetCol(int index)
        {
            return index % this._numCols;
        }

        private int GetRow(int index)
        {
            return index / this._numCols;
        }

        private int GetIndex(int row, int col)
        {
            return this._numCols * row + col;
        }
    }
}
