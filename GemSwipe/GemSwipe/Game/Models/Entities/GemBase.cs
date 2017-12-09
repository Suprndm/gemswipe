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
        public int TargetX { get; set; }
        public int TargetY { get; set; }

        public ICell AnchorCell;
        private bool _hasBeenHandled;
        private bool _isPerformingAction;
        protected Board _board;

        public GemBase(int boardX, int boardY, int size, Board board) : base(0, 0, 0, 0)
        {
            _board = board;
        }

        public GemBase(int boardX, int boardY, int size, float x, float y, float radius, Random randomizer, Board board) : base(x, y, radius * 2, radius * 2)
        {
            _board = board;
        }


        public virtual bool CanPerform()
        {
            return !_isPerformingAction;
        }

        public virtual bool CanCollide(IGem gem)
        {
            return false;
        }

        public virtual Task CollideInto(IGem gem)
        {
            return Task.Delay(0);
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

        public Task Move(int x, int y, bool animationActivator = false)
        {
            IndexX = x;
            IndexY = y;
            if (animationActivator)
            {
                return MoveTo(_board.ToGemX(x), _board.ToGemY(y));
            }
            else
            {
                return Task.Delay(0);
            }
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
            Dispose();
        }

        public virtual void Reinitialize()
        {
        }


    }
}
