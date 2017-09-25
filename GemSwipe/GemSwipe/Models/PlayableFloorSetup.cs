using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine.Floors;

namespace GemSwipe.Models
{
    public class PlayableFloorSetup
    {
        public PlayableFloorSetup(BoardSetup boardSetup, int floor, bool isFinal, string title)
        {
            BoardSetup = boardSetup;
            Floor = floor;
            IsFinal = isFinal;
            Title = title;
        }

        public string Title { get; }
        public BoardSetup BoardSetup { get;}
        public int Floor { get;}
        public bool IsFinal { get;}

    }
}
