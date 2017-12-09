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

        //bool IsResolved();
        bool CanPerform();
        bool CanCollide(IGem targetGem);
        Task CollideInto(IGem targetGem);
        Task PerformAction(params Func<Task>[] actions);
        Task Die();
        //void ValidateResolution();
        Task Move(int x, int y, bool activation = false);
        void Dispose();
    }
}
