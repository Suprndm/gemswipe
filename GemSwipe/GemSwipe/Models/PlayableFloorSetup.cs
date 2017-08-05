using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Models
{
    public class PlayableFloorSetup
    {
        public PlayableFloorSetup(BoardSetup boardSetup, int floor, bool isFinal)
        {
            BoardSetup = boardSetup;
            Floor = floor;
            IsFinal = isFinal;
        }

        public BoardSetup BoardSetup { get;}
        public int Floor { get;}
        public bool IsFinal { get;}

    }
}
