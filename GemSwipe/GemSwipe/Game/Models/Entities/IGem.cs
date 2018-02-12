using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.Entities
{
    public interface IGem
    {
        //bool IsBusy { get; set; }
        void Dispose();
        void Reactivate();
        Task ValidateHandling();
        bool HasBeenHandled();
        bool CanPerform();
        bool HasCompletedPerformance();
        bool CanCollide(IGem gem);
        bool CanActivate();
        Task TryResolveSwipe(Direction direction);
        Task Collide(IGem gem);
        Task GoTo(ICell cell);
        void Attach(ICell cell);
        void DetachCell();

        int IndexX { get; set; }
        int IndexY { get; set; }

        Task PerformAction(params Func<Task>[] actions);
        Task Die();
        Task Move(int x, int y);
    }
}
