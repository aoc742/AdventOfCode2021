using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day10
    {
        public void Start()
        {
            string[] fileLines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day10Input.txt");

            List<Line> lines = new List<Line>();
            foreach(var line in fileLines)
            {
                lines.Add(new Line(line));
            }

            List<char> illegalChars = new List<char>();
            List<Line> corruptedLines = new List<Line>();
            foreach(var line in lines)
            {
                if (line.IsCorrupted(out char illegal))
                {
                    illegalChars.Add(illegal);
                    corruptedLines.Add(line);
                }
            }

            // sum up illegal chars. ) = 3, ] = 57, } = 1197, > = 25137
            int illegalCharScore = 0;
            foreach(var c in illegalChars)
            {
                if (c == ')') illegalCharScore += 3;
                else if (c == ']') illegalCharScore += 57;
                else if (c == '}') illegalCharScore += 1197;
                else if (c == '>') illegalCharScore += 25137;
            }

            Console.WriteLine($"Part 1: Illegal char score: {illegalCharScore}");


            // Part 2
            // discard all corrupted lines. the remaining lines are "incomplete"
            foreach(var line in corruptedLines)
            {
                lines.Remove(line);
            }

            List<ulong> scores = new List<ulong>();
            foreach(var line in lines)
            {
                List<char> closingSequence = line.GetClosingCharSequence();
                scores.Add(this.GetScore(closingSequence));
            }

            ulong[] sorted = scores.ToArray();
            Array.Sort(sorted);
            Console.WriteLine($"Part 2 Score: {sorted[sorted.Length / 2]}");
        }

        private ulong GetScore(List<char> sequence)
        {
            ulong score = 0;
            foreach(char c in sequence)
            {
                score *= 5;
                if (c == ')') score += 1;
                else if (c == ']') score += 2;
                else if (c == '}') score += 3;
                else if (c == '>') score += 4;
            }
            return score;
        }

        private class Line
        {
            private string _line;
            private bool[] _isMatched;

            public Line(string line)
            {
                this._line = line;
                this._isMatched = new bool[line.Length]; // all false by default
            }

            public List<char> GetClosingCharSequence()
            {
                List<int> unmatchedIndices = new List<int>();
                for(int i = this._isMatched.Length-1; i >= 0; --i)
                {
                    if (!this._isMatched[i])
                    {
                        unmatchedIndices.Add(i);
                    }
                }

                List<char> closingChars = new List<char>();
                foreach(int index in unmatchedIndices)
                {
                    closingChars.Add(this.GetMatchingCloser(this._line[index]));
                }

                return closingChars;
            }

            /// <summary>
            /// A corrupted line is one where a chunk closes with the wrong character - 
            /// that is, where the characters it opens and closes with do not form one 
            /// of the four legal pairs listed above.
            /// </summary>
            public bool IsCorrupted(out char firstIncorrectClosingChar)
            {
                firstIncorrectClosingChar = ' ';

                // Get all closing chars
                List<int> closingIndices = new List<int>();
                for (int i = 0; i < this._line.Length; ++i)
                {
                    if (this.IsCloser(this._line[i]))
                    {
                        closingIndices.Add(i);
                    }
                }

                foreach(var closingIndex in closingIndices)
                {
                    for (int i = closingIndex - 1; i >= 0; --i)
                    {
                        if (this._isMatched[i])
                        {
                            // already previously matched
                            continue;
                        }

                        if (IsMatch(this._line[i], this._line[closingIndex]))
                        {
                            this._isMatched[i] = true;
                            this._isMatched[closingIndex] = true;
                            break;
                        }
                        else
                        {
                            // no match! corrupted
                            firstIncorrectClosingChar = this._line[closingIndex];
                            return true;
                        }
                    }
                }

                return false;
            }

            private bool IsOpener(char c)
            {
                if (c == '[' ||
                    c == '{' ||
                    c == '(' ||
                    c == '<')
                    return true;
                return false;
            }

            private bool IsCloser(char c)
            {
                if (c == ']' ||
                    c == '}' ||
                    c == ')' ||
                    c == '>')
                    return true;
                return false;
            }

            private bool IsMatch(char first, char second)
            {
                int one = (int)first;
                int two = (int)second;

                return one + two == 81 ||
                       one + two == 122 ||
                       one + two == 184 ||
                       one + two == 248;
            }

            private char GetMatchingCloser(char c)
            {
                if (c == '(') return ')';
                else if (c == '[') return ']';
                else if (c == '{') return '}';
                else if (c == '<') return '>';

                throw new Exception($"GetMatch: Invalid input {c}");
            }
        }
    }
}
