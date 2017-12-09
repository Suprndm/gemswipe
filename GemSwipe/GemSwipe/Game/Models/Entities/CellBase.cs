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
        public IGem AttachedGem;
        private CellBase _targetCell;
        private Board _board;

        private bool _hasHandledGem;
        public bool Resolved { get; set; }

        public int IndexX { get; set; }
        public int IndexY { get; set; }

        public CellBase(int boardX, int boardY, Board board)
        {
            IndexX = boardX;
            IndexY = boardY;
            _board = board;
            AttachedGem = null;
        }

        public virtual Task ResolveSwipe(Direction direction)
        {
            if (CanActivate())
            {
                return TryHandleGem(AttachedGem, direction);
            }
            else return Task.Delay(0);
        }

        public virtual void Reinitialize()
        {
            _hasHandledGem = false;
        }

        public virtual void Reactivate()
        {
            _hasHandledGem = false;
        }

        public virtual void AttachGem(IGem gem)
        {
            AttachedGem = gem;

        }

        public virtual void DetachGemBase()
        {
            AttachedGem = null;
        }


        public bool MustActivate()
        {
            if (AttachedGem == null)
            {
                return false;
            }
            else
            {
                return !_hasHandledGem;
            }
        }

        public virtual bool CanActivate()
        {
            if (AttachedGem == null)
            {
                return false;
            }
            else
            {
                return !_hasHandledGem && AttachedGem.CanPerform();
            }
        }

        public virtual ICell GetTargetCell(Direction direction)
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
                return _board.CellsList.SingleOrDefault(p => p.IndexX == targetX && p.IndexY == targetY);
            }
        }

        public virtual Task TryHandleGem(IGem gem, Direction direction)
        {

            ICell targetCell = GetTargetCell(direction);
            if (targetCell == null)
            {
                ValidateGemHandling();
                return Task.Delay(0);
            }
            else
            {
                if (targetCell.CanProcess(gem))
                {
                    return targetCell.TryReceiveGem(gem, direction, this);
                }
                else
                {
                    return Task.Delay(0);
                }
            }
        }

        public virtual void ValidateGemHandling()
        {
            _hasHandledGem = true;
        }


        public virtual bool CanProcess(IGem gem)
        {
            if (AttachedGem == null)
            {
                return true;
            }
            else
            {
                return _hasHandledGem && AttachedGem.CanPerform() && gem.CanPerform();
            }

        }

        public virtual Task TryReceiveGem(IGem gem, Direction direction, ICell senderCell)
        {
            if (AttachedGem == null)
            {
                senderCell.DetachGemBase();
                senderCell.Reactivate();

                AttachGem(gem);
                Reactivate();
                return PickGem(gem);
            }
            else
            {
                if (gem.CanCollide(AttachedGem))
                {
                    senderCell.DetachGemBase();
                    senderCell.Reactivate();

                    Reactivate();
                    PickGem(gem);
                    return HandleCollisionWithAttachedGem(gem);
                }
                else
                {
                    return ReturnGem(gem, senderCell);
                }
            }
        }

        public virtual Task PickGem(IGem gem)
        {
            return gem.Move(IndexX, IndexY, true);
        }

        public virtual Task HandleCollisionWithAttachedGem(IGem gem)
        {
            return gem.PerformAction(() => gem.CollideInto(AttachedGem));
        }

        public virtual Task ReturnGem(IGem gem, ICell senderCell)
        {
            senderCell.AttachGem(gem);
            senderCell.ValidateGemHandling();
            return Task.Delay(0);
            //return gem.Move(senderCell.IndexX, senderCell.IndexY, true);
        }


    }
}
