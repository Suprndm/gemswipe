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
            _countDown = new CountDown(150);
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
            _board = new Board("1 1 2 1-0 1 2 1-0 0 2 2-1 0 1 1");
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

            if(swipeResult.IsWon) _countDown.AddMoreTime(8);

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
    }
}