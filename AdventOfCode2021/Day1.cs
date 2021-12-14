using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day1
    {
        public void Start()
        {
            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day1Input.txt");

            int increasedCount = -1;
            int last = -1;

            foreach (string line in lines)
            {
                int current = int.Parse(line);
                if (current > last)
                {
                    ++increasedCount;
                }

                last = current;
            }

            Console.WriteLine($"Total measurements that are larger than previous: {increasedCount}");

            Part2();
        }

        public void Part2()
        {
            // Compares previous in a 3-measurement sliding window 

            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day1Input.txt");

            int increasedCount = -1;
            int last = -1;

            for(int i = 0; i < lines.Length-2; ++i)
            {
                int one = int.Parse(lines[i]);
                int two = int.Parse(lines[i + 1]);
                int three = int.Parse(lines[i + 2]);
                int current = one + two + three;
                if (current > last)
                {
                    ++increasedCount;
                }

                last = current;
            }

            Console.WriteLine($"Total using 3-measurement sliding window: {increasedCount}");
        }
    }
}
