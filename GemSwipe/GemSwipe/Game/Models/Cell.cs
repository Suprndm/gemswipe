using GemSwipe.Game.Models.BoardModel;
using GemSwipe.Game.Models.Entities;
using System;
using System.Collections.Generic;

namespace GemSwipe.Game.Models
{
    public class Cell : CellBase
    {

        public int X { get; }
        public int Y { get; }

        public bool IsBlocked { get; set; }

        public Cell(int x, int y, Board board, bool isBlocked = false) : base(x, y, board)
        {
            X = x;
            Y = y;
        }

        public Gem GetAttachedGem()
        {
            return (Gem)AssignedGem;
        }
    }
}
