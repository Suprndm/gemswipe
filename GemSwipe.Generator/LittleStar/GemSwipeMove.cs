using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Models;
using LittleStar;

namespace GemSwipe.Generator.LittleStar
{
    public class GemSwipeMove : IMove
    {
        public Direction Direction { get; set; }
        public override string ToString()
        {
            return Direction.ToString();
        }
    }
}
