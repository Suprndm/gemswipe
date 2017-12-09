using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.Entities
{
    public interface IGem2
    {
        void Dispose();
        void ValidateHandling();
        bool HasBeenHandled();
        bool CanPerform();
        bool CanCollide(IGem2 gem2);
        Task Collide(IGem2 gem2);
        Task GoTo(ICell2 cell2);
    }
}
