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

        private bool _resolved;
        protected Board _board;
        protected IList<Action<int>> _animationsStack;
        //public Gem GetAttachedGem<T>()
        //{
        //    return _attachedGem;

        //    IList<Func<T>> listOfFunct = new List<Func<T>>();
        //    IList<Action<int>> listOfActions = new List<Action<int>>();
        //    Func<T> func = () => { return 1; };
        //    listOfFunct.Add(func);
        //    listOfFunct[0].Invoke();
        //}


        public GemBase(int boardX, int boardY, int size, Board board) : base(0, 0, 0, 0)
        {
            _board = board;
            _resolved = false;
            _animationsStack = new List<Action<int>>();
        }

        public GemBase(int boardX, int boardY, int size, float x, float y, float radius, Random randomizer, Board board) : base(x, y, radius * 2, radius * 2)
        {
            _board = board;
            _resolved = false;
            _animationsStack = new List<Action<int>>();
        }

        public void Reinitialize()
        {
            _resolved = false;
            _animationsStack = new List<Action<int>>();
        }

        public bool CanAct(Direction direction)
        {
            return !_resolved;
        }

        public bool IsResolved()
        {
            return _resolved;
        }

        public void ValidateResolution()
        {
            _resolved = true;
        }

        public virtual bool CanCollide(IGem gem)
        {
            return false;
        }

        public virtual void CollideInto(IGem gem)
        {
            //DieTo(_board.ToGemX(gem.IndexX, this), _board.ToGemY(gem.IndexY, this));
        }

        public virtual void Move(int x, int y)
        {
            TargetX = x;
            TargetY = y;
           
            if (_resolved)
            {
                IndexX = x;
                IndexY = y;
                _animationsStack.Add((p) => MoveTo(_board.ToGemX(TargetX, this), _board.ToGemY(TargetY, this)));
            }
        }

        public Task MoveTo(float x, float y, int animationLenght=MovementAnimationMs)
        {
            var oldX = _x;
            var oldY = _y;

            var newX = x;
            var newY = y;
            if (Canvas != null)
            {
                this.Animate("moveX", p => _x = (float)p, oldX, newX, 4, (uint)animationLenght, Easing.CubicOut);
                this.Animate("moveY", p => _y = (float)p, oldY, newY, 8, (uint)animationLenght, Easing.CubicOut);
            }
            return Task.Delay(animationLenght);

        }

        public async virtual void Die()
        {
           
            if (Canvas != null)
            {
                await Task.Delay(MovementAnimationMs / 2);
                this.Animate("fade", p => _opacity = (float)p, 1, 0, 4, MovementAnimationMs / 2, Easing.CubicOut);
                await Task.Delay(MovementAnimationMs / 2);
            }
        }

        public async void DieTo(float x, float y)
        {
            //    var deadGems = Gems.Where(gem => gem.IsDead()).ToList();
            //    foreach (var deadGem in deadGems)
            //    {
            //        Gems.Remove(deadGem);
            //    }

            MoveTo(x, y);

            //if (Canvas != null)
            //{
            //    await Task.Delay(MovementAnimationMs / 2);
            //    this.Animate("fade", p => _opacity = (float)p, 1, 0, 4, MovementAnimationMs / 2, Easing.CubicOut);
            //    await Task.Delay(MovementAnimationMs / 2);
            //}
            Dispose();
        }

        public async virtual void RunAnimation()
        {
            if (_animationsStack.Count > 0)
            {
                foreach (Action<int> animation in _animationsStack)
                {
                    animation.Invoke(_animationsStack.Count);
                    await Task.Delay(MovementAnimationMs);
                }
            }
        }
    }
}
