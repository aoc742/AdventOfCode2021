using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day13
    {
        public void Start()
        {
            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day13Input.txt");
            string[] separators = { " " };
            List<string[]> assortedLines = lines.ToList().Select(line => line.Split(separators, StringSplitOptions.RemoveEmptyEntries)).ToList();

            List<List<int>> markedPositions = new List<List<int>>(); // x coord, y coord
            List<Tuple<bool, int>> folds = new List<Tuple<bool, int>>(); // true if fold along y, else x ; int = fold index
            foreach(var assortedLine in assortedLines)
            {
                if (assortedLine.Count() == 1)
                {
                    string[] xy = assortedLine[0].Split(',');
                    List<int> position = new List<int>();
                    position.Add(int.Parse(xy[0])); // x index
                    position.Add(int.Parse(xy[1])); // y index
                    markedPositions.Add(position);
                }
                else if (assortedLine.Count() == 3)
                {
                    string[] fold = assortedLine[2].Split('=');
                    folds.Add(new Tuple<bool, int>(fold[0].Contains("y"), int.Parse(fold[1])));
                }
            }

            foreach(var fold in folds)
            {
                int leftMost = 0;
                int topMost = 0;

                if (fold.Item1)
                {
                    leftMost = 0;
                    // fold along y
                    foreach(var position in markedPositions)
                    {
                        // if y index > fold location
                        if (position[1] > fold.Item2)
                        {
                            position[1] -= (position[1] - fold.Item2) * 2;

                            if (position[1] > fold.Item2)
                            {
                                // going into the negatives
                                position[1] -= fold.Item2;
                            }
                        }

                        if (position[1] < topMost)
                        {
                            topMost = position[1];
                        }
                    }
                }
                else
                {
                    topMost = 0;

                    // fold along x
                    foreach(var position in markedPositions)
                    {
                        // if y index > fold location
                        if (position[0] > fold.Item2)
                        {
                            position[0] -= (position[0] - fold.Item2) * 2;

                            if (position[0] > fold.Item2)
                            {
                                // going into the negatives
                                position[0] -= fold.Item2;
                            }
                        }

                        if (position[0] < leftMost)
                        {
                            leftMost = position[0];
                        }
                    }
                }

                // reformat all markedPositions to reset topleft most item to 0,0
                if (topMost < 0)
                {
                    foreach(var position in markedPositions)
                    {
                        position[1] -= topMost;
                    }
                }
                if (leftMost < 0)
                {
                    foreach(var position in markedPositions)
                    {
                        position[0] -= leftMost;
                    }
                }

                List<string> markedPositionStrs = new List<string>();
                foreach (var position in markedPositions)
                {
                    markedPositionStrs.Add($"{position[0]},{position[1]}");
                }
                markedPositionStrs = markedPositionStrs.Distinct().ToList();
                // convert back to ints
                markedPositions.Clear();
                foreach(var str in markedPositionStrs)
                {
                    string[] xy = str.Split(',');
                    List<int> position = new List<int>();
                    position.Add(int.Parse(xy[0])); // x index
                    position.Add(int.Parse(xy[1])); // y index
                    markedPositions.Add(position);
                }

                // Part 1 is the total count after a single fold
                Console.WriteLine($"Number of marked positions: {markedPositions.Count()}");

            }

            // Part 2: plot the grid and read out the numbers
            string[][] map = new string[6][];
            for(int i = 0; i < map.Length; i++)
            {
                map[i] = new string[50];
                for(int j = 0; j < map[i].Length; j++)
                {
                    map[i][j] = ".";
                }
            }

            foreach(var position in markedPositions)
            {
                map[position[1]][position[0]] = "#";
            }

            foreach(var line in map)
            {
                Console.WriteLine(string.Join("", line));
            }
        }
    }
}
