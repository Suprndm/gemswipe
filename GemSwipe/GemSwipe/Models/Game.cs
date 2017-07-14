using System;
using System.Collections.Generic;

namespace GemSwipe.Models
{
    public class Game
    {

        private Board _board;
        public event Action<Gem> NewCell;
        public event Action Blocked;

        public Game(int height, int width)
        {
            _board = new Board(height, width);
            Height = height;
            Width = width;
            Gems = new List<Gem>();
        }

        public void InitGame()
        {
            PopGem();
            PopGem();
        }

        public void Swipe(Direction direction)
        {
            _board.Swipe(direction);
            if (_board.IsFull())
            {
                Blocked?.Invoke();
            }
            else
            {
                PopGem();
            }
        }

        public Board GetBoard()
        {
            return _board;
        }

        private void PopGem()
        {
            var gem = _board.Pop();
            NewCell?.Invoke(gem);
        }

        public int Height { get; }
        public int Width { get; }

        public IList<Gem> Gems { get; }


    }
}
