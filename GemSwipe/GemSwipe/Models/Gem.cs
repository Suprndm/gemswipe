using System;

namespace GemSwipe.Models
{
    public class Gem
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public int Size { get; private set; }
        public Guid Id { get; }

        public int TargetX { get; private set; }
        public int TargetY { get; private set; }

        private bool _willLevelUp;
        private bool _willDie;
        private bool _isDead;

        public Gem(int x, int y)
        {
            X = x;
            Y = y;
            Size = 1;

            Id = Guid.NewGuid();
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

            _isDead = _willDie;
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