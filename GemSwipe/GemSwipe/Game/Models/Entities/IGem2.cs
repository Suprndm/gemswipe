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
        void ValidateHandling2();
        bool HasBeenHandled2();
        bool CanPerform2();
        bool CanCollide2(IGem2 gem2);
        bool CanActivate2();
        Task TryResolveSwipe2(Direction direction);
        Task Collide2(IGem2 gem2);
        Task GoTo2(ICell2 cell2);
    }
}
