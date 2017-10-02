using System.Collections.Generic;

namespace GemSwipe.Game.Models
{
    public struct GameSetup
    {
        public GameSetup(int level, IList<BoardSetup> boardSetups)
        {
            Level = level;
            BoardSetups = boardSetups;
        }

        public int Level { get; }
        public IList<BoardSetup> BoardSetups { get;}
    }
}
