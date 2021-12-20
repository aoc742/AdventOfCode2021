using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day8
    {
        public void Start()
        {
            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day8Input.txt");
            string[] separators = { "|" };
            List<string[]> assortedLines = lines.ToList().Select(line => line.Split(separators, StringSplitOptions.RemoveEmptyEntries)).ToList();

            //List<string[]> inputLines = assortedLines.Where((c, i) => i % 2 == 0).ToList();
            //List<string[]> outputLines = assortedLines.Where((c, i) => i % 2 == 1).ToList();
            List<string[]> inputLines = new List<string[]>();
            List<string[]> outputLines = new List<string[]>();

            string [] separators2 = {" "};
            foreach(var line in assortedLines)
            {
                inputLines.Add(line[0].Split(separators2, StringSplitOptions.RemoveEmptyEntries));
                outputLines.Add(line[1].Split(separators2, StringSplitOptions.RemoveEmptyEntries));
            }

            List<Test> tests = new List<Test>();
            for (int i = 0; i < inputLines.Count; i++)
            {
                tests.Add(new Test(inputLines[i], outputLines[i]));
            }

            //Part1(tests);
            Part2(tests);
        }

        public void Part1(List<Test> tests)
        {
            int count1478 = 0;
            foreach (var test in tests)
            {
                count1478 += test.Find1478s();
            }

            Console.WriteLine($"Part 1: 1478 Count: {count1478}");
        }

        public void Part2(List<Test> tests)
        {
            int count = 0;
            foreach (var test in tests)
            {
                test.Decode();
                count += test.GetOutput();
            }

            Console.WriteLine($"Part 2: Count: {count}");
        }

        internal class Test
        {
            public List<string> Inputs { get; set; }
            public List<string> Outputs { get; set;}

            private string _decodedZeroSignal;
            private string _decodedOneSignal;
            private string _decodedTwoSignal;
            private string _decodedThreeSignal;
            private string _decodedFourSignal;
            private string _decodedFiveSignal;
            private string _decodedSixSignal;
            private string _decodedSevenSignal;
            private string _decodedEightSignal;
            private string _decodedNineSignal;
                

            public Test(string[] inputs, string[] outputs)
            {
                this.Inputs = inputs.ToList();
                this.Outputs = outputs.ToList();
            }

            /// <summary>
            /// Returns the list of Outputs that are either 1, 4, 7, or 8
            /// </summary>
            /// <returns></returns>
            public int Find1478s()
            {
                int count = 0;
                foreach(var output in this.Outputs)
                {
                    if (output.Length == 2 || // 1 uses two segments
                        output.Length == 3 || // 7 uses three segments
                        output.Length == 4 || // 4 uses four segments
                        output.Length == 7)   // 8 uses seven segments
                    {
                        count++;
                    }
                }
                return count;
            }

            /// <summary>
            /// Decodes the 10 signals to map a-g to the 8 segments
            /// 
            ///   aaaa
            ///  b    c
            ///  b    c
            ///   dddd
            ///  e    f
            ///  e    f
            ///   gggg
            ///   
            /// Where a-g can be mapped in any order
            /// </summary>
            public void Decode()
            {
                /* We'll represent the a-g segments with 0-6 like so
                 *    0000
                 *   1    2
                 *   1    2
                 *    3333
                 *   4    5
                 *   4    5
                 *    6666
                 *    
                 * 
                 */
                // each number contains the below segments in its signal
                // "signal      = list of segments
                //string one      = "25";
                //string seven    = "025";
                //string four     = "1235";
                //string two      = "02346";
                //string three    = "02356";
                //string five     = "01356";
                //string zero     = "012456";
                //string six      = "013456";
                //string nine     = "012356";
                //string eight    = "0123456";

                string oneSignal = this.Inputs.First(input => input.Length == 2);
                string sevenSignal = this.Inputs.First(input => input.Length == 3);
                string fourSignal = this.Inputs.First(input => input.Length == 4);
                string eightSignal = this.Inputs.First(input => input.Length == 7);

                string _2or5Segment = oneSignal;                                                                         // 2,5 = b, e
                char zeroSegment = sevenSignal.First(number => !oneSignal.Contains(number));                // 0   = d
                string _1or3Segment = string.Concat(fourSignal.Where(number => !sevenSignal.Contains(number)));    // 1,3 = c, g

                string partialKnowns = string.Concat(_2or5Segment, zeroSegment, _1or3Segment);
                List<string> fiveLengthSignals = this.Inputs.Where(input => input.Length == 5).ToList();
                string twoSignal = fiveLengthSignals.First(signal =>
                {
                    int sharedCount = 0;
                    foreach(char num in signal)
                    {
                        if (partialKnowns.Contains(num))
                        {
                            sharedCount++;
                        }
                    }
                    return sharedCount != 4 ? true : false;
                });
                List<string> _3or5Signals = fiveLengthSignals.Where(signal => signal != twoSignal).ToList();

                char fourSegment = twoSignal.First(segment => !_3or5Signals[0].Contains(segment) && !_3or5Signals[1].Contains(segment));

                // Get 6 segment
                List<char> _3or5temps = new List<char>();
                _3or5temps.Add(_3or5Signals[0].First(digit => !_3or5Signals[1].Contains(digit)));
                _3or5temps.Add(_3or5Signals[1].First(digit => !_3or5Signals[0].Contains(digit)));
                int tempIndex = _3or5temps.FindIndex(digit => oneSignal.Contains(digit)); 
                string threeSignal = _3or5Signals[tempIndex];
                string fiveSignal = _3or5Signals[tempIndex == 0 ? 1 : 0];

                // the number in oneSignal that fiveSignal does not contain
                char twoSegment = oneSignal.First(digit => !fiveSignal.Contains(digit));
                // the number in fiveSignal that threeSignal does not contain
                char oneSegment = fiveSignal.First(digit => !threeSignal.Contains(digit));

                char sixSegment = threeSignal.First(digit => !sevenSignal.Contains(digit) && !fourSignal.Contains(digit));

                char fiveSegment = oneSignal.First(digit => digit != twoSegment);

                List<string> sixLengthSignals = this.Inputs.Where(input => input.Length == 6).ToList();
                string zeroSignal = sixLengthSignals.First(signal => signal.Contains(zeroSegment) &&
                                                                     signal.Contains(oneSegment) &&
                                                                     signal.Contains(twoSegment) &&
                                                                     signal.Contains(fourSegment) &&
                                                                     signal.Contains(fiveSegment) &&
                                                                     signal.Contains(sixSegment));

                char threeSegment = fourSignal.First(digit => digit != oneSegment &&
                                                              digit != twoSegment &&
                                                              digit != fiveSegment);

                string sixSignal = sixLengthSignals.First(signal => signal.Contains(zeroSegment) &&
                                                                     signal.Contains(oneSegment) &&
                                                                     signal.Contains(threeSegment) &&
                                                                     signal.Contains(fourSegment) &&
                                                                     signal.Contains(fiveSegment) &&
                                                                     signal.Contains(sixSegment));

                string nineSignal = sixLengthSignals.First(signal => signal != zeroSignal && signal != sixSignal);

                this._decodedZeroSignal = this.Alphabetize(zeroSignal);
                this._decodedOneSignal = this.Alphabetize(oneSignal);
                this._decodedTwoSignal = this.Alphabetize(twoSignal);
                this._decodedThreeSignal = this.Alphabetize(threeSignal);
                this._decodedFourSignal = this.Alphabetize(fourSignal);
                this._decodedFiveSignal = this.Alphabetize(fiveSignal);
                this._decodedSixSignal = this.Alphabetize(sixSignal);
                this._decodedSevenSignal = this.Alphabetize(sevenSignal);
                this._decodedEightSignal = this.Alphabetize(eightSignal);
                this._decodedNineSignal = this.Alphabetize(nineSignal);
            }

            private string Alphabetize(string outOfOrder)
            {
                char[] newArray = outOfOrder.ToCharArray();
                Array.Sort(newArray);
                return new string(newArray);
            }

            /// <summary>
            /// From an encoded string "abcd" input,
            /// returns the decoded number in string format "3"
            /// </summary>
            private string GetSingleDecodedNumber(string output)
            {
                if (output == this._decodedZeroSignal) return "0";
                else if (output == this._decodedOneSignal) return "1";
                else if (output == this._decodedTwoSignal) return "2";
                else if (output == this._decodedThreeSignal) return "3";
                else if (output == this._decodedFourSignal) return "4";
                else if (output == this._decodedFiveSignal) return "5";
                else if (output == this._decodedSixSignal) return "6";
                else if (output == this._decodedSevenSignal) return "7";
                else if (output == this._decodedEightSignal) return "8";
                else if (output == this._decodedNineSignal) return "9";
                else throw new Exception("Incorrect value somehow");
            }

            /// <summary>
            /// Returns the 4-digit decoded output number
            /// </summary>
            public int GetOutput()
            {
                string fourDigitString = "";
                foreach(var output in this.Outputs)
                {
                    string result = GetSingleDecodedNumber(this.Alphabetize(output));
                    fourDigitString += result;
                }

                return int.Parse(fourDigitString);
            }
        }
    }
}
