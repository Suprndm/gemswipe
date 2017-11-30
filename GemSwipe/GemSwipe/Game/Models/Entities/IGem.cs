using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.Entities
{
    public interface IGem
    {
        int IndexX { get; set; }
        int IndexY { get; set; }

        bool IsResolved();
        bool CanCollide(IGem targetGem);
        void CollideInto(IGem targetGem);
        void ValidateResolution();
        void Move(int x, int y);
    }
}
