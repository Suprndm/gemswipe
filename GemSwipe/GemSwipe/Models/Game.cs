using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GemSwipe.Models
{
    public class Game
    {
        public event Action Blocked;
        public event Action<int> Fusion;

        private Board _board;

        public Game(int height, int width)
        {
            _board = new Board(height, width);
            Height = height;
            Width = width;
            Gems = new List<Gem>();
        }

        public void InitGame()
        {
            _board.Pop();
            _board.Pop();
        }

        public int Height { get; }
        public int Width { get; }

        public IList<Gem> Gems { get; }

      

   
    }
}
