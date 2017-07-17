using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Models
{
    public class GameUpdate
    {
        public bool IsBlocked { get; set; }
        public bool IsWon { get; set; }

        public IList<Gem> MovedGems { get; set; }
        public IList<Gem> FusedGems { get; set; }
        public IList<Gem> DeadGems { get; set; }
    }
}
