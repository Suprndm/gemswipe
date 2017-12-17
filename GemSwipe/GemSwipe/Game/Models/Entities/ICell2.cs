using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.Entities
{
    public interface ICell2
    {
        int IndexX2 { get; set; }
        int IndexY2 { get; set; }

        bool MustActivate2(IGem2 gem2);
        Task Activate2(IGem2 gem2);
        bool IsEmpty2();

        bool CanHandle2(IGem2 gem2);
        Task Handle2(IGem2 gem2);

        //void AttachGem(IGem gem);
        //bool CanProcess(IGem gem);
        //void DetachGemBase();
        //void Reactivate();
        //void ValidateGemHandling();
        //Task TryReceiveGem(IGem gem, Direction direction, ICell senderCell);
    }
}
