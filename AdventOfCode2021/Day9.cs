using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day9
    {
        private List<int> _map = new List<int>();
        private HashSet<int> _reachedIndices = new HashSet<int>(); // list of indices in _map that we've already been to before
        private int _numRows;
        private int _numCols;
        private List<int> lowPoints = new List<int>();

        public void Start()
        {
            // input file is a 2d grid of numbers.
            // you must find the low points - points lower
            // than its adjacent locations.

            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day9Input.txt");
            foreach (var line in lines)
            {
                foreach(char digit in line)
                {
                    this._map.Add(digit - '0'); // converts char to int
                }
            }

            this._numRows = lines.Length;
            this._numCols = lines[0].Length;

            for (int i = 0; i < this._map.Count(); ++i)
            {
                if (_reachedIndices.Contains(i))
                {
                    // don't care about an index we've already been to.
                    continue;
                }

                if (TryFindLowPoint(i, out int lowPointIndex))
                {
                    this.lowPoints.Add(lowPointIndex);
                }
            }

            // Calculate sum of risk levels
            // risk level of a low point = its height + 1
            int overallRiskLevel = 0;
            foreach (var lowPoint in lowPoints)
            {
                overallRiskLevel += this._map[lowPoint] + 1;
            }

            Console.WriteLine($"Part 1: Overall risk level: {overallRiskLevel}");

            // Part 2 - Find basins
            List<int> basinSizes = new List<int>();
            foreach(var lowPoint in lowPoints)
            {
                basinSizes.Add(GetBasinSize(lowPoint));
            }

            int first = basinSizes.Max();
            basinSizes.Remove(first);
            int second = basinSizes.Max();
            basinSizes.Remove(second);
            int third = basinSizes.Max();
            Console.WriteLine($"Part 2: product of 3 largest basin sizes: {first * second * third}");
        }

        /// <summary>
        /// Returns the size of a basin that includes the input index
        /// </summary>
        public int GetBasinSize(int index)
        {
            HashSet<int> basin = new HashSet<int>();
            basin.Add(index);

            overallVisits.Clear();
            GetBasinSizeRecurse(index, basin);
            return overallVisits.Count();
        }

        HashSet<int> overallVisits = new HashSet<int>();
        /// <summary>
        /// Recursively searches all adjacent nodes, searching for all adjacent nodes that 
        /// are not the number 9. Returns nothing, but populates overallVisits with the 
        /// number of indices that were searched (that did not equal 9), indicating the size of the basin.
        /// </summary>
        public void GetBasinSizeRecurse(int startingIndex, HashSet<int> visited)
        {
            // Get adjacent positions
            LinkedList<int> nodes = new LinkedList<int>();
            //LinkedList<string> nodes = graph.adjacentNodes(visited.Last());
            int newIndex;
            if (TryGoLeft(startingIndex, out newIndex))
            {
                nodes.AddLast(newIndex);
            }
            if (TryGoRight(startingIndex, out newIndex))
            {
                nodes.AddLast(newIndex);
            }
            if (TryGoUp(startingIndex, out newIndex))
            {
                nodes.AddLast(newIndex);
            }
            if (TryGoDown(startingIndex, out newIndex))
            {
                nodes.AddLast(newIndex);
            }

            // Examine each adjacent node
            List<int> toSearch = new List<int>();
            foreach (int node in nodes)
            {
                if (visited.Contains(node))
                {
                    continue;
                }

                // if node.Equals(endNode)
                if (this._map[node] != 9)
                {
                    //visited.Add(node);
                    toSearch.Add(node);
                }
            }

            foreach(int node in toSearch)
            {
                // If already visited, or node is an endNode
                if (visited.Contains(node) || this._map[node] == 9)
                {
                    continue;
                }

                visited.Add(node);

                GetBasinSize2(node, visited);
            }

            foreach(var visit in visited)
            {
                this.overallVisits.Add(visit);
            }
        }

        /// <summary>
        /// returns the index of the low point associated with the inputIndex
        /// </summary>
        public bool TryFindLowPoint(int index, out int lowPoint)
        {
            while (true)
            {
                // already searched this index? don't do it again.
                if (this._reachedIndices.Contains(index))
                {
                    lowPoint = index;
                    return false;
                }
                this._reachedIndices.Add(index);

                int newIndex;
                int nextIndexToTry = -1;
                bool atLowPoint = true;
                if (TryGoLeft(index, out newIndex))
                {
                    if (this._map[newIndex] <= this._map[index])
                    {
                        nextIndexToTry = newIndex;
                        atLowPoint = false;
                        //continue;
                    }
                    else
                    {
                        this._reachedIndices.Add(newIndex);
                    }
                }
                if (TryGoRight(index, out newIndex))
                {
                    if (this._map[newIndex] <= this._map[index])
                    {
                        nextIndexToTry = newIndex;
                        atLowPoint = false;
                    }
                    else
                    {
                        this._reachedIndices.Add(newIndex);
                    }
                }
                if (TryGoUp(index, out newIndex))
                {
                    if(this._map[newIndex] <= this._map[index])
                    {
                        nextIndexToTry = newIndex;
                        atLowPoint = false;
                    }
                    else
                    {
                        this._reachedIndices.Add(newIndex);
                    }
                }
                if (TryGoDown(index, out newIndex))
                {
                    if (this._map[newIndex] <= this._map[index])
                    {
                        nextIndexToTry = newIndex;
                        atLowPoint = false;
                    }
                    else
                    {
                        this._reachedIndices.Add(newIndex);
                    }
                }

                // if you searched all directions and can't find a lowerPoint, then you're at it, baby!
                if (atLowPoint)
                {
                    lowPoint = index;
                    return true;
                }
                index = nextIndexToTry;
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
            if (this.GetCol(inputIndex) == this._numCols-1)
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
            if (row== 0)
            {
                outputIndex= -1;
                return false;
            }
            outputIndex = this._numCols * (row-1) + col;
            return true;
        }

        private bool TryGoDown(int inputIndex, out int outputIndex)
        {
            int row = this.GetRow(inputIndex);
            int col = this.GetCol(inputIndex);
            if (row == this._numRows -1)
            {
                outputIndex = -1;
                return false;
            }
            outputIndex = this._numCols * (row + 1) + col;
            return true;
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
