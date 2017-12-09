using GemSwipe.Paladin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.Entities
{
    public abstract class GemBase2 : SkiaView, IGem2
    {
        protected const int MovementAnimationMs = 600;
        public int IndexX { get; set; }
        public int IndexY { get; set; }
        public ICell2 AnchorCell;
        private bool _hasBeenHandled;
        private bool _isPerformingAction;
        protected Board _board;
 
        public GemBase2(int boardX, int boardY, int size, Board board) : base(0, 0, 0, 0)
        {
            IndexX = boardX;
            IndexY = boardY;
            _board = board;
        }

        public GemBase2(int boardX, int boardY, int size, float x, float y, float radius, Random randomizer, Board board) : base(x, y, radius * 2, radius * 2)
        {
            IndexX = boardX;
            IndexY = boardY;
            _board = board;
        }


        public Task TryResolveSwipe(Direction direction)
        {
            if (CanActivate())
            {
                return ResolveSwipe(direction);
            }
            else
            {
                return Task.Delay(0);
            }
        }

        public bool CanActivate()
        {
            return !HasBeenHandled() && CanPerform();
        }

        public bool HasBeenHandled()
        {
            return _hasBeenHandled;
        }

        public bool CanPerform()
        {
            return !_isPerformingAction;
        }

        public void ValidateHandling()
        {
            _hasBeenHandled = true;
        }

        public virtual Task ResolveSwipe(Direction direction)
        {
            if (AnchorCell.MustActivate(this))
            {
                return AnchorCell.Activate(this);
            }
            else
            {
                return Activate(direction);
            }
        }

        public virtual Task Activate(Direction direction)
        {
            ICell2 targetCell = GetTargetCell(direction);
            if (targetCell == null)
            {
                ValidateHandling();
                return Task.Delay(0);
            }
            else
            {
                if (targetCell.CanHandle(this))
                {
                    return targetCell.Handle(this);
                }
                else
                {
                    return Task.Delay(0);
                }
            }
        }

        public virtual ICell2 GetTargetCell(Direction direction)
        {
            int targetX = -1;
            int targetY = -1;

            bool targetIsNull = true;
            switch (direction)
            {
                case Direction.Top:
                    targetX = IndexX;
                    targetY = IndexY - 1;
                    if (targetY >= 0)
                    {
                        targetIsNull = false;
                    }
                    break;
                case Direction.Bottom:
                    targetX = IndexX;
                    targetY = IndexY + 1;
                    if (targetY <= _board.NbOfRows - 1)
                    {
                        targetIsNull = false;
                    }
                    break;
                case Direction.Left:
                    targetX = IndexX - 1;
                    targetY = IndexY;
                    if (targetX >= 0)
                    {
                        targetIsNull = false;
                    }
                    break;
                case Direction.Right:
                    targetX = IndexX + 1;
                    targetY = IndexY;
                    if (targetX <= _board.NbOfColumns - 1)
                    {
                        targetIsNull = false;
                    }
                    break;
            }
            if (targetIsNull)
            {
                return null;
            }
            else
            {
                return _board.CellsList2.SingleOrDefault(p => p.IndexX == targetX && p.IndexY == targetY);
            }
        }

      
        public virtual bool CanCollide(IGem2 gem2)
        {
            return false;
        }

        public virtual Task Collide(IGem2 gem2)
        {
            return Task.Delay(0);
        }

        public Task GoTo(ICell2 cell2)
        {
            return Task.Delay(0);
        }
    }
}
