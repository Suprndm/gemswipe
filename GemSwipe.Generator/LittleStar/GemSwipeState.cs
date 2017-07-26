using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Models;
using LittleStar;

namespace GemSwipe.Generator.LittleStar
{
    public class GemSwipeState : IGameState
    {
        public Board Board { get; set; }
    }
}
