using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day2
    {
        public void Start()
        {
            /* "forward X" increases horizontal position by X units
             * "down X" increases depth by X units
             * "up X" decreases depth by X units 
             */

            int horizontalPos = 0;
            int depth = 0;

            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day2Input.txt");

            foreach(var line in lines)
            {
                string[] direction = line.Split(' ');
                int amount = int.Parse(direction[1]);
                switch (direction[0])
                {
                    case "forward":
                        horizontalPos += amount;
                        break;
                    case "down":
                        depth += amount;
                        break;
                    case "up":
                        depth -= amount;
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine($"horizontal pos: {horizontalPos}\ndepth: {depth}");
            Console.WriteLine($"multiplied together: {horizontalPos * depth}");

            this.Part2(lines);
        }

        public void Part2(string[] lines)
        {
            /*
             * "down X" increases your aim by X units
             * "up X" decreases aim by X units
             * "forward X" does 2 things: 
             *      - increases horizontal position by X units
             *      - increases depth by your aim multiplied by X
             */

            int aim = 0;
            int horizontalPos = 0;
            int depth = 0;

            foreach (var line in lines)
            {
                string[] direction = line.Split(' ');
                int amount = int.Parse(direction[1]);
                switch (direction[0])
                {
                    case "forward":
                        horizontalPos += amount;
                        depth += aim * amount;
                        break;
                    case "down":
                        aim += amount;
                        break;
                    case "up":
                        aim -= amount;
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine($"PART 2 horizontal pos: {horizontalPos}\ndepth: {depth}");
            Console.WriteLine($"PART 2 multiplied together: {horizontalPos * depth}");
        }
    }
}
