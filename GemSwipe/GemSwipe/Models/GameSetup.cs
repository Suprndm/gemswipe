using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Models
{
    public class GameSetup
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public IList<Gem> Gems { get; set; }
    }
}
