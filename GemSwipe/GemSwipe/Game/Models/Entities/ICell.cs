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

        bool MustActivate(IGem gem);
        Task Activate(IGem gem);
        bool IsEmpty();

        bool CanHandle(IGem gem);
        Task Handle(IGem gem,ICell senderCell=null);

        //int IndexX { get; set; }
        //int IndexY { get; set; }
        void Assign(IGem gem);
        void UnassignGem();
    }
}
