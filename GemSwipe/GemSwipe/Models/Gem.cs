using System;

namespace GemSwipe.Models
{
    public class Gem
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public float FluidX { get; set; }
        public float FluidY { get; set; }

        public int Size { get; private set; }
        public int FluidSize { get; set; }

        public int TargetX { get; private set; }
        public int TargetY { get; private set; }

        private bool _willLevelUp;
        private bool _willDie;
        private bool _isDead;

        public Gem(int x, int y)
        {
            X = x;
            FluidX = X;

            Y = y;
            FluidY = Y;
            Size = 1;
        }

        public void LevelUp()
        {
            _willLevelUp = true;
        }

        public void Die()
        {
            _willDie = true;
        }

        public bool CanMerge()
        {
            return !_willLevelUp && !_willDie;
        }

        public void Resolve()
        {
            if (_willLevelUp)
            {
                Size++;
                _willLevelUp = false;
            }

            X = TargetX;
            Y = TargetY;
        }

        public void UpdatePosition()
        {
            FluidX += (float)((X - FluidX) * 0.4);
            if (Math.Abs(X - FluidX) < .01)
            {
                FluidX = X;
            }

            FluidY += (float)((Y - FluidY) * 0.4);
            if (Math.Abs(Y - FluidY) < .01)
            {
                FluidY = Y;
            }

            if (X == FluidX && Y == FluidY)
            {
                FluidSize = Size;
                _isDead = _willDie;
            }
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public bool WillDie()
        {
            return _willDie;
        }

        public void Move(int x, int y)
        {
            TargetX = x;
            TargetY = y;
        }


        public void SetSize(int size)
        {
            Size = size;
        }
    }
}