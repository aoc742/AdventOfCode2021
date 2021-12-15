using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day4
    {
        public void Start()
        {
            /* Bingo
             * 5x5 grid, numbers chosen at random.
             * 5 in a row or column win (diagonals DON'T count) 
             */

            string text = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day4Input.txt");

            string[] separator = { "\r\n\r\n" };
            string[] boardStrs = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            // TODO: calculate numRows,numCols from the text, instead of hardcode
            int numCols = 5;
            int numRows = 5;

            string[] callouts = boardStrs[0].Split(',');
            List<Board> boards = boardStrs.Skip(1).Select((board, index) => new Board(board, index, numRows, numCols)).ToList();

            List<int> previousWinners = new List<int>(); // index of previous winners
            for(int i = 0; i < callouts.Length; i++)
            {
                string nextCallout = callouts[i];
                boards.ForEach(board => board.UpdateBoard(nextCallout));

                List<Board> winners = boards.FindAll(board => board.IsWinner && !previousWinners.Contains(board.GetId()));
                foreach(Board board in winners)
                {
                    int boardIndex = board.GetId();
                    previousWinners.Add(boardIndex);
                    Board winningBoard = boards[boardIndex];

                    int finalScore = winningBoard.CalculateScore();
                    Console.WriteLine($"Winning board: {boardIndex}, score: {finalScore}");
                }
            }
        }

        private class Board
        {
            private int _id;
            private string[] _board;
            private bool[] _found; // true if number in same position in _board has been called out, else false
            private int _numRows;
            private int _numCols;

            private int _winningCalloutNumber;

            public bool IsWinner { get; set; } = false;

            public Board(string board, int id, int numRows, int numCols)
            {
                this._id = id;
                this._numCols = numCols;
                this._numRows = numRows;

                string[] separators = {" ", "\r\n" };
                this._board = board.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                this._found = new bool[this._board.Length];
            }

            public int GetId()
            {
                return this._id;
            }

            /// <summary>
            /// Updates current state of the board based on a newly called out Bingo number
            /// </summary>
            public void UpdateBoard(string callout)
            {
                // check if number exists in my board
                if (this._board.Contains(callout))
                {
                    int index = Array.FindIndex(this._board, (position) => { return position == callout; });
                    if (index != -1)
                    {
                        this._found[index] = true;

                        // check for a win (5 in a row)
                        if (this.CheckForWin(index))
                        {
                            this.IsWinner = true;
                        }
                    }
                }
            }

            public int GetCol(int index)
            {
                return index % this._numCols;
            }

            public int GetRow(int index)
            {
                return index / this._numCols;
            }

            public int GetIndex(int row, int col)
            {
                return this._numCols * row + col;
            }

            /// <summary>
            /// Checks board state to see if the board has 5 in a row (vertical or horizontal)
            /// </summary>
            public bool CheckForWin(int indexOfLastCallout)
            {
                // note: we only need to check the row/col of the most recent callout
                int col = this.GetCol(indexOfLastCallout);
                int row = this.GetRow(indexOfLastCallout);

                // win by column
                bool winByColumn = true;
                for(int i = 0; i < this._numRows; i++)
                {
                    int index = this.GetIndex(i, col);
                    if (!this._found[index])
                    {
                        winByColumn = false;
                        break;
                    }
                }

                // win by row
                bool winByRow = true;
                for(int i = 0; i < this._numCols; i++)
                {
                    int index = this.GetIndex(row, i);
                    if (!this._found[index])
                    {
                        winByRow = false;
                        break;
                    }
                }

                if (winByColumn || winByRow)
                {
                    this._winningCalloutNumber = int.Parse(this._board[indexOfLastCallout]);
                    return true;
                }
                return false;
            }

            public int GetSumOfAllUnmarkedNumbers()
            {
                int count = 0;
                for(int i = 0; i < this._found.Length; i++)
                {
                    if (!this._found[i])
                    {
                        count += int.Parse(this._board[i]);
                    }
                }
                return count;
            }

            /// <summary>
            /// sum of all unmarked numbers multiplied by last called number when the board won
            /// </summary>
            public int CalculateScore()
            {
                return this.GetSumOfAllUnmarkedNumbers() * this._winningCalloutNumber;
            }
        }
    }
}
