using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.Entities
{
    public interface ICell2
    {
        int IndexX { get; set; }
        int IndexY { get; set; }

        bool MustActivate(IGem2 gem2);
        Task Activate(IGem2 gem2);
        bool IsEmpty();

        bool CanHandle(IGem2 gem2);
        Task Handle(IGem2 gem2);

        //void AttachGem(IGem gem);
        //bool CanProcess(IGem gem);
        //void DetachGemBase();
        //void Reactivate();
        //void ValidateGemHandling();
        //Task TryReceiveGem(IGem gem, Direction direction, ICell senderCell);
    }
}
