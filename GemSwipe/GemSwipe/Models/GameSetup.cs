using System.Collections.Generic;

namespace GemSwipe.Models
{
    public class GameSetup
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public IList<Gem> Gems { get; set; }
    }
}
