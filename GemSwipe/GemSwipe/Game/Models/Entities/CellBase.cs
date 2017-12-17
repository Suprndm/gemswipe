using GemSwipe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GemSwipe.Game.Models.Entities
{
    public class CellBase : ICell
    {
        public int IndexX { get; set; }
        public int IndexY { get; set; }
        public IGem AssignedGem;
        private Board _board;

        public CellBase(int boardX, int boardY, Board board)
        {
            IndexX = boardX;
            IndexY = boardY;
            _board = board;
            AssignedGem = null;
        }

        public void Assign(IGem gem)
        {
            AssignedGem = gem;
        }

        public void UnassignGem()
        {
            AssignedGem = null;
        }


        public virtual bool MustActivate(IGem gem)
        {
            return false;
        }

        public Task Activate(IGem gem)
        {
            return Task.Delay(0);
        }

        public bool IsEmpty()
        {
            return AssignedGem == null;
        }

        public bool CanHandle(IGem gem)
        {
            if (IsEmpty())
            {
                return true;
            }
            else
            {
                return AssignedGem.CanPerform() && AssignedGem.HasBeenHandled();
                //return AssignedGem.CanPerform() && AssignedGem.HasBeenHandled();
            }
        }

        public Task Handle(IGem gem, ICell senderCell = null)
        {
            if (IsEmpty())
            {
                senderCell?.UnassignGem();
                Assign(gem);
                gem.Attach(this);
                return Pick(gem);
            }
            else if (gem.CanCollide(AssignedGem))
            {
                senderCell?.UnassignGem();
                AssignedGem.Reactivate();
                //Pick(gem);
                return gem.Collide(AssignedGem);
            }
            else
            {
                return ReturnToSender(gem, senderCell);
            }
        }

        public Task ReturnToSender(IGem gem, ICell senderCell)
        {
            gem.ValidateHandling();
            return Task.Delay(0);
        }

        public Task Pick(IGem gem)
        {
            return gem.GoTo(this);
        }
    }
}
