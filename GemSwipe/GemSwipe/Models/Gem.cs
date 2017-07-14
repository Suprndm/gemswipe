
using Xamarin.Forms;

namespace GemSwipe.Models
{
    public class Gem
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Size { get; private set; }

        private bool _willLevelUp;
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

        public void Resolve()
        {
            if (_willLevelUp)
            {
                Size++;
                _willLevelUp = false;
            }
        }

    }
}
