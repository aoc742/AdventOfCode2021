using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day6
    {
        public void Start()
        {
            // each Lanternfish creates a new lanternfish once every 7 days
            // each fish is labelled based on number of days until it creates a new lanternfish
            // new lanternfish need 2 extra days in its first cycle

            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day6Input.txt");
            string[] separator = { ","};
            string[] numbers = lines[0].Split(separator, StringSplitOptions.RemoveEmptyEntries);


            //Console.WriteLine($"Initial State: {string.Join(",", numbers)}");

            // Part 1 - 80 days
            Solve(numbers, 80);

            // Part 2 - 256 days
            Solve(numbers, 256);
        }

        public void Solve(string[] numbers, int days)
        {
            Queue<long> fishQ = new Queue<long>();

            // Input into queue number of fish with each unique age
            // Queue should have 9 numbers in it at all times
            for (int i = 0; i < 9; ++i)
            {
                int numFishOfAge = numbers.Where(number => int.Parse(number) == i).Count();
                fishQ.Enqueue(numFishOfAge);
            }

            int day = 1;
            while (day <= days)
            {
                long popped = fishQ.Dequeue();

                // each popped fish had an offspring
                // so we need to enqueue popped # fish
                // and also update index 6 in queue with popped # fish
                fishQ.Enqueue(popped);
                var tempArray = fishQ.ToArray();
                tempArray[6] += popped;
                fishQ = new Queue<long>(tempArray);

                ++day;
            }

            Console.WriteLine($"After {day-1} days: {fishQ.ToArray().Sum()} fish");
        }
    }
}
