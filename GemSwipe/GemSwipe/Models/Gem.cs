
namespace GemSwipe.Models
{
    public class Gem
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Size { get; private set; }

        public int TargetX { get; private set; }
        public int TargetY { get; private set; }

        private bool _willLevelUp;
        private bool _willDie;

        public Gem(int x, int y)
        {
            X = x;
            Y = y;

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
        }

        public void Move(int x, int y)
        {
            TargetX = x;
            TargetY = y;
        }

    }
}
