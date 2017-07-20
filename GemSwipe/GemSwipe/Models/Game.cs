using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Xamarin.Forms;

namespace GemSwipe.Models
{
    public class Game
    {
        private Board _board;
        private CountDown _countDown;
        public event Action<GameLostData> Lost;

        public Game()
        {
            _countDown = new CountDown(15);
            _countDown.Zero += _countDown_Zero;
        }

        private void _countDown_Zero()
        {
            Lost?.Invoke(new GameLostData {MaxLevel = 5, Score = 65494});
        }

        public void Start()
        {
            _countDown.Start();
        }

        public BoardSetup SetupBoard()
        {
            _board = BuildBoardFromString("1 0 0 1-0 1 1 0-0 0 1 0-1 0 1 1");
            return new BoardSetup
            {
                Columns = _board.Width,
                Rows = _board.Height,
                Gems = _board.Gems
            };
        }

        public SwipeResult Swipe(Direction direction)
        {
            var swipeResult = _board.Swipe(direction);
            swipeResult.IsWon = CheckWin();

            return swipeResult;
        }

        public double SecondsTillLose()
        {
            return _countDown.RemainingSeconds();
        }

        private bool CheckWin()
        {
            return _board.Gems.Count == 1;
        }

        public Board GetBoard()
        {
            return _board;
        }

        private Board BuildBoardFromString(string boardString)
        {
            var rows = boardString.Split('-');
            var height = rows.Length;
            var width = rows[0].Split(' ').Length;
            var boardCells = new List<Cell>();
            for (int j = 0; j < height; j++)
            {
                var cells = rows[j].Split(' ');
                for (int i = 0; i < width; i++)
                {
                    var gemSize = int.Parse(cells[i]);
                    var newCell = new Cell(i, j);
                    boardCells.Add(newCell);
                    if (gemSize > 0)
                    {
                        var newGem = new Gem(i, j);
                        newGem.SetSize(gemSize);
                        newCell.AttachGem(newGem);
                    }
                }
            }

            return new Board(boardCells);
        }
    }
}