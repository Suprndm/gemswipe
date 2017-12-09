using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.Entities
{
    public interface ICell
    {
        int IndexX { get; set; }
        int IndexY { get; set; }
        void AttachGem(IGem gem);
        bool CanProcess(IGem gem);
        void DetachGemBase();
        void Reactivate();
        void ValidateGemHandling();
        Task TryReceiveGem(IGem gem, Direction direction, ICell senderCell);
    }
}
