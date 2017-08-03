using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Models
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
