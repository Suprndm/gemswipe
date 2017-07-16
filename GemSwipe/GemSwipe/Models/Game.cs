using System;
using System.Collections.Generic;
using System.Linq;

namespace GemSwipe.Models
{
    public class Game: IGameState
    {
        private Board _board;
        public int Height { get; }
        public int Width { get; }

        public event Action<Gem> NewCell;
        public event Action Blocked;

        public Game(int height, int width)
        {
  
            Height = height;
            Width = width;
        }


        public void InitGame()
        {
            _board = BuildBoardFromString("1 0 0 1-0 1 1 0-0 0 1 0-1 0 1 1");
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
              //  PopGem();
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


        public IBoardState GetBoardState()
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
