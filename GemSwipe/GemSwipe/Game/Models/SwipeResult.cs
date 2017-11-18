using System.Collections.Generic;
using GemSwipe.Game.Models.Entities;

namespace GemSwipe.Game.Models
{
    public class SwipeResult
    {
        public bool IsBlocked { get; set; }
        public bool IsFull { get; set; }

        public IList<Gem> MovedGems { get; set; }
        public IList<Gem> FusedGems { get; set; }
        public IList<Gem> DeadGems { get; set; }
        public IList<Gem> TeleporterGems { get; set; }

        public SwipeResult()
        {
            MovedGems = new List<Gem>();
            FusedGems = new List<Gem>();
            DeadGems = new List<Gem>();
            TeleporterGems = new List<Gem>();
        }
    }
}
