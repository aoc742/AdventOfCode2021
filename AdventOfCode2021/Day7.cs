using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day7
    {
        public void Start()
        {
            // crab submarines can only move horizontally.
            // given list of horizontal positions of each crab
            // crab submarines have limited fuel.
            // goal: make all horizontal positions match, while spending
            // as little fuel as possible
            // each unit move over costs 1 unit of fuel.

            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day7Input.txt");
            string[] separator = { "," };
            string[] numbers = lines[0].Split(separator, StringSplitOptions.RemoveEmptyEntries);

            List<int> positions = numbers.ToList().Select(x => int.Parse(x)).ToList();

            Part1(positions);
            Part2(positions);
        }

        public void Part1(List<int> positions)
        {
            int maxPosition = positions.Max();

            int bestPosition = -1;
            int bestFuelCost = int.MaxValue;
            for (int i = 0; i < maxPosition; i++)
            {
                // estimate cost of moving all submarines to the "i" position
                int fuelCost = 0;
                positions.ForEach(position => fuelCost += Math.Abs(position - i));

                if (fuelCost < bestFuelCost)
                {
                    bestPosition = i;
                    bestFuelCost = fuelCost;
                }
            }

            Console.WriteLine($"Best position: {bestPosition}, fuel cost: {bestFuelCost}");
        }

        public void Part2(List<int> positions)
        {
            int maxPosition = positions.Max();

            int bestPosition = -1;
            int bestFuelCost = int.MaxValue;
            for (int i = 0; i < maxPosition; i++)
            {
                // estimate cost of moving all submarines to the "i" position
                int totalFuelCost = 0;
                foreach(var position in positions)
                {
                    totalFuelCost += GetFuelCostOfMove(position, i);
                }

                if (totalFuelCost < bestFuelCost)
                {
                    bestPosition = i;
                    bestFuelCost = totalFuelCost;
                }
            }

            Console.WriteLine($"Best position: {bestPosition}, fuel cost: {bestFuelCost}");

        }

        public int GetFuelCostOfMove(int start, int end)
        {
            int numTerms = Math.Abs(start - end) + 1;

            int firstTerm = 1;
            int lastTerm = Math.Abs(start - end) - 1;
            int result = numTerms * (firstTerm + lastTerm) / 2;

            return result;
        }
    }
}
