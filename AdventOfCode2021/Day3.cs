using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day3
    {
        public void Start()
        {
            /* decode binary numbers
             * generate 2 new binary numbers (gamma rate and epsilon rate)
             * power consumption = gamma rate * epsilon rate
             * 
             * Gamma rate = concat of most common bit in each corresponding position
             * Afterwards, convert the gamme rate from binary to decimal
             * 
             * Epsilon rate = least common bit is used (binary NOT)
             * Afterwards, convert the epsilon rate from binary to decimal
             */

            string[] fileLines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day3Input.txt");

            List<string> lines = new List<string>(fileLines);
            // 12 digits per line in input file
            int numDigitsPerLine = fileLines[0].Length;

            // 1000 lines total
            int totalNumLines = lines.Count;

            int[] digitCount = GetDigitCounts(lines, numDigitsPerLine);

            char[] gammaBinary = new char[numDigitsPerLine];
            char[] epsilonBinary = new char[numDigitsPerLine];

            for (int digitIndex = 0; digitIndex < numDigitsPerLine; digitIndex++)
            {
                if (digitCount[digitIndex] >= totalNumLines / 2)
                {
                    gammaBinary[digitIndex] = '1';
                    epsilonBinary[digitIndex] = '0';
                }
                else
                {
                    gammaBinary[digitIndex] = '0';
                    epsilonBinary[digitIndex] = '1';
                }
            }

            // convert binary to decimal
            int gamma = Convert.ToInt32(new string(gammaBinary), 2);
            int epsilon = Convert.ToInt32(new string(epsilonBinary), 2);

            Console.WriteLine($"gamma * epsilon: {gamma * epsilon}");
            Part2(lines);
        }

        /// <summary>
        /// Returns array of ints representing the number of 1's in that index of the original codes '0001011001'
        /// </summary>
        public int[] GetDigitCounts(List<string> lines, int numDigitsPerLine)
        {
            // 12 digits per line
            int[] digitCount = new int[numDigitsPerLine];

            // populates digitCounts
            foreach (var line in lines)
            {
                for (int digitIndex = 0; digitIndex < numDigitsPerLine; digitIndex++)
                {
                    int digit = line[digitIndex] - '0'; // converts from char to int
                    digitCount[digitIndex] += digit;
                }
            }

            return digitCount;
        }

        public void Part2(List<string> lines)
        {
            /* life support rating = oxygen generator rating * co2 scrubber rating
             * o2 rating
             * 
             * Looking at just the first bit, keep only numbers selected by the bit criteria
             * Only one number left? STOP, this is the rating value you are searchign for.
             * Otherwise repeat the prcoess considering the next bit to the right.
             * 
             * o2 rating:
             * bit criteria = most common value (0 or 1) in current bit position.
             *      - if a tie, keep 1 and discard 0's.
             */

            int o2 = GetO2(lines);
            int co2 = GetCO2(lines);

            Console.WriteLine($"o2: {o2}, co2: {co2}, o2 * co2: {o2 * co2}");
        }

        public int GetO2(List<string> lines)
        {
            int numDigitsPerLine = lines[0].Length;
            int index = 0;
            while (lines.Count > 1)
            {
                int totalNumLines = lines.Count;
                int[] digitCount = GetDigitCounts(lines, numDigitsPerLine);

                if (digitCount[index] >= (float)totalNumLines / 2)
                {
                    // keep only the lines that start with 1
                    lines = GetLines(lines, '1', index);
                }
                else
                {
                    // keep only the lines that start with 0
                    lines = GetLines(lines, '0', index);
                }
                index++;
            }

            return Convert.ToInt32(lines[0], 2);
        }

        public int GetCO2(List<string> lines)
        {
            int numDigitsPerLine = lines[0].Length;
            int index = 0;
            while (lines.Count > 1)
            {
                int totalNumLines = lines.Count;
                int[] digitCount = GetDigitCounts(lines, numDigitsPerLine);

                if (digitCount[index] >= (float)totalNumLines / 2)
                {
                    // keep only the lines that start with 0
                    lines = GetLines(lines, '0', index);
                }
                else
                {
                    // keep only the lines that start with 1
                    lines = GetLines(lines, '1', index);
                }
                index++;
            }

            return Convert.ToInt32(lines[0], 2);
        }

        /// <summary>
        /// Returns list of lines where the line starts with 0 or 1 based on startsWith param
        /// </summary>
        public List<string> GetLines(List<string> originalLines, char startsWith, int index)
        {
            List<string> newLines = new List<string>();

            foreach(var line in originalLines)
            {
                if (line[index] == startsWith)
                {
                    newLines.Add(line);
                }
            }
            return newLines;
        }


    }
}
