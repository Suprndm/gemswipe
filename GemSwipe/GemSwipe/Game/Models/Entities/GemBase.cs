using GemSwipe.Paladin.Core;
using GemSwipe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GemSwipe.Game.Models.Entities
{
    public abstract class GemBase : SkiaView, IGem
    {
        protected const int MovementAnimationMs = 600;
        public int IndexX { get; set; }
        public int IndexY { get; set; }
        public ICell AttachedCell;
        private bool _hasBeenHandled;
        private bool _isPerformingAction = false;
        protected Board _board;

        public GemBase(int boardX, int boardY, int size, Board board) : base(0, 0, 0, 0)
        {
            IndexX = boardX;
            IndexY = boardY;
            _board = board;
            if (_board != null)
            {
                AttachedCell = _board.CellsList.SingleOrDefault(p => p.IndexX == IndexX && p.IndexY == IndexY);
            }
        }

        public GemBase(int boardX, int boardY, int size, float x, float y, float radius, Random randomizer, Board board) : base(x, y, radius * 2, radius * 2)
        {
            IndexX = boardX;
            IndexY = boardY;
            _board = board;
            if (_board != null)
            {
                AttachedCell = _board.CellsList.SingleOrDefault(p => p.IndexX == IndexX && p.IndexY == IndexY);
            }
        }

        #region Gem Model handling

        public virtual void Reactivate()
        {
            _hasBeenHandled = false;
        }

        public void Attach(ICell cell)
        {
            AttachedCell = cell;
        }

        public void DetachCell()
        {
            AttachedCell = null;
        }

        public bool CanActivate()
        {
            return !HasBeenHandled() && CanPerform();
        }

        public bool HasBeenHandled()
        {
            return _hasBeenHandled;
        }

        public Task ValidateHandling()
        {
            _hasBeenHandled = true;
            return Task.Delay(0);
        }

        public bool CanPerform()
        {
            return !_isPerformingAction;
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

        public virtual Task ResolveSwipe(Direction direction)
        {
            if (AttachedCell.MustActivate(this))
            {
                return AttachedCell.Activate(this);
            }
            else
            {
                return SelfActivate(direction);
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

        public virtual Task SelfActivate(Direction direction)
        {
            ICell targetCell = GetTargetCell(direction);
            if (targetCell == null)
            {
                return ValidateHandling();
            }
            else
            {
                if (targetCell.CanHandle(this))
                {
                    return targetCell.Handle(this, AttachedCell);
                }
                else
                {
                    return Task.Delay(0);
                }
            }
        }

        public virtual bool CanCollide(IGem gem)
        {
            return false;
        }

        public virtual Task Collide(IGem gem)
        {
            return Task.Delay(0);
        }

        #endregion

        #region Animation

        public Task GoTo(ICell cell)
        {
            IndexX = cell.IndexX;
            IndexY = cell.IndexY;
            AttachedCell = cell;
            return Move(cell.IndexX, cell.IndexY);

        }

        public async Task PerformAction(params Func<Task>[] actions)
        {
            _isPerformingAction = true;
            for (int i = 0; i < actions.Length; i++)
            {
                await actions[i].Invoke();
            }
            _isPerformingAction = false;
        }

        public virtual Task Move(int x, int y)
        {
            return MoveTo(_board.ToGemX(x), _board.ToGemY(y));
        }

        public Task MoveTo(float x, float y, int animationLenght = MovementAnimationMs)
        {
            var oldX = _x;
            var oldY = _y;

            var newX = x;
            var newY = y;
            if (Canvas != null)
            {
                this.Animate("moveX", p => _x = (float)p, oldX, newX, 4, (uint)animationLenght, Easing.SinInOut);
                this.Animate("moveY", p => _y = (float)p, oldY, newY, 8, (uint)animationLenght, Easing.SinInOut);
            }
            return Task.Delay(animationLenght);

        }

        public async virtual Task Die()
        {
            if (Canvas != null)
            {
                await Task.Delay(MovementAnimationMs / 2);
                this.Animate("fade", p => _opacity = (float)p, 1, 0, 4, MovementAnimationMs / 2, Easing.SinInOut);
                await Task.Delay(MovementAnimationMs / 2);
            }
            Clear();
        }

        public void Clear()
        {
            _board.Gems.Remove(this);
            Dispose();
        }
        #endregion
    }
}
