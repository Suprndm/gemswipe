using GemSwipe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual void ResolveSwipe(Direction direction)
        {
            if (CanActivate())
            {
                TryHandleGem(AttachedGem, direction);
            }

        }

        public void Reinitialize()
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

        public virtual bool CanActivate()
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

        public virtual void TryHandleGem(IGem gem, Direction direction)
        {

            ICell targetCell = GetTargetCell(direction);
            if (targetCell == null)
            {
                gem.ValidateResolution();
                gem.Move(IndexX, IndexY);
                _hasHandledGem = true;
            }
            else
            {
                if (targetCell.CanProcess(gem))
                {
                    DetachGemBase();
                    targetCell.TryReceiveGem(gem, direction, this);
                    _hasHandledGem = true;
                }
            }
        }

        public virtual bool CanProcess(IGem gem)
        {
            if (AttachedGem == null)
            {
                return true;
            }
            else
            {
                return _hasHandledGem && AttachedGem.IsResolved();
            }
            //else
            //{
            //    return _hasHandledGem;
            //}
        }

        public virtual void TryReceiveGem(IGem gem, Direction direction, ICell senderCell)
        {
            if (AttachedGem == null)
            {
                AttachGem(gem);
                gem.Move(IndexX, IndexY);
                _hasHandledGem = false;
            }
            else
            {
                gem.ValidateResolution();
                if (gem.CanCollide(AttachedGem))
                {
                    gem.CollideInto(AttachedGem);
                    _hasHandledGem = false;

                }
                else
                {
                    ReturnGem(gem, senderCell);
                }
            }
        }

        public virtual void ReturnGem(IGem gem, ICell senderCell)
        {
            senderCell.AttachGem(gem);
            gem.Move(senderCell.IndexX, senderCell.IndexY);
        }


    }
}
