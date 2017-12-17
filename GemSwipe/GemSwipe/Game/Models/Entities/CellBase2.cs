using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.Entities
{
    public class CellBase2 : ICell2
    {
        public int IndexX2 { get; set; }
        public int IndexY2 { get; set; }
        public IGem2 AttachedGem2;
        private Board _board2;

        public CellBase2(int boardX, int boardY, Board board)
        {
            IndexX2 = boardX;
            IndexY2 = boardY;
            _board2 = board;
            AttachedGem2 = null;
        }

        public void AttachGem2(IGem2 gem2)
        {
            AttachedGem2 = gem2;
        }

        public bool MustActivate2(IGem2 gem2)
        {
            return false;
        }

        public Task Activate2(IGem2 gem2)
        {
            return Task.Delay(0);
        }
        public bool IsEmpty2()
        {
            return AttachedGem2 == null;
        }

        public bool CanHandle2(IGem2 gem2)
        {
            if (IsEmpty2())
            {
                return true;
            }
            else
            {
                return AttachedGem2.CanPerform2() && AttachedGem2.HasBeenHandled2();
            }
        }

        public Task Handle2(IGem2 gem2)
        {
            if (IsEmpty2())
            {
                return Pick2(gem2);
            }
            else
            {
                if (gem2.CanCollide2(AttachedGem2))
                {
                    return gem2.Collide2(AttachedGem2);
                }
                else
                {
                    gem2.ValidateHandling2();
                    return Task.Delay(0);
                }
            }
        }

        public Task Pick2(IGem2 gem2)
        {
            return gem2.GoTo2(this);
        }
    }
}
