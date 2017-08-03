using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine;

namespace GemSwipe.Models
{
    public class SwipeResult
    {
        public bool IsBlocked { get; set; }
        public bool BoardWon { get; set; }
        public bool GameFinished { get; set; }

        public IList<Gem> MovedGems { get; set; }
        public IList<Gem> FusedGems { get; set; }
        public IList<Gem> DeadGems { get; set; }
    }
}
