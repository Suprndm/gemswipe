using System.Collections.Generic;
using GemSwipe.Game.Entities;

namespace GemSwipe.Game.Models
{
    public class SwipeResult
    {
        public bool IsBlocked { get; set; }
        public bool BoardWon { get; set; }
        public bool GameFinished { get; set; }
        public bool IsFull { get; set; }

        public IList<Gem> MovedGems { get; set; }
        public IList<Gem> FusedGems { get; set; }
        public IList<Gem> DeadGems { get; set; }
    }
}
