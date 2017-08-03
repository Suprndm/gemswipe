using GemSwipe.GameEngine;

namespace GemSwipe.Models
{
    public class Cell
    {
        public int X { get;}
        public int Y { get;}

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        private Gem _attachedGem;

        public bool IsEmpty()
        {
            return _attachedGem == null;
        }

        public void AttachGem(Gem gem)
        {
            _attachedGem = gem;
            gem.Move(X,Y);
        }

        public void DetachGem()
        {
            _attachedGem = null;
        }

        public Gem GetAttachedGem()
        {
            return _attachedGem;
        }
    }
}
