using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.Entities
{
    public class CellBase2 : ICell2
    {
        public int IndexX { get; set; }
        public int IndexY { get; set; }
        public IGem2 AttachedGem;
        private Board _board;

        public CellBase2(int boardX, int boardY, Board board)
        {
            IndexX = boardX;
            IndexY = boardY;
            _board = board;
            AttachedGem = null;
        }

        public void AttachGem(IGem2 gem2)
        {
            AttachedGem = gem2;
        }

        public bool MustActivate(IGem2 gem2)
        {
            return false;
        }

        public Task Activate(IGem2 gem2)
        {
            return Task.Delay(0);
        }
        public bool IsEmpty()
        {
            return AttachedGem == null;
        }

        public bool CanHandle(IGem2 gem2)
        {
            if (IsEmpty())
            {
                return true;
            }
            else
            {
                return AttachedGem.CanPerform() && AttachedGem.HasBeenHandled();
            }
        }

        public Task Handle(IGem2 gem2)
        {
            if (IsEmpty())
            {
                return Pick(gem2);
            }
            else
            {
                if (gem2.CanCollide(AttachedGem))
                {
                    return gem2.Collide(AttachedGem);
                }
                else
                {
                    gem2.ValidateHandling();
                    return Task.Delay(0);
                }
            }
        }

        public Task Pick(IGem2 gem2)
        {
            return gem2.GoTo(this);
        }
    }
}
