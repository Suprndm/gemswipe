using GemSwipe.Game.Models.BoardModel;
using GemSwipe.Game.Models.Entities;

namespace GemSwipe.Game.Models
{
    public class Cell
    {
        public int X { get; }
        public int Y { get; }

        public bool IsBlocked { get; set; }
        public CellModifier Modifier { get; set; }

        public Cell(int x, int y, bool isBlocked = false)
        {
            X = x;
            Y = y;
            IsBlocked = isBlocked;
        }

        public Cell(int x, int y, GemType gemType)
        {
            X = x;
            Y = y;
            Modifier = SetModifier(gemType);
        }

        public CellModifier SetModifier(GemType gemType)
        {
            switch (gemType)
            {
                default:
                    IsBlocked = false;
                    return CellModifier.Base;
                case GemType.Blocking:
                    IsBlocked = true;
                    return CellModifier.Blocked;
            }
        }

        private Gem _attachedGem;

        public bool IsEmpty()
        {
            return _attachedGem == null && !IsBlocked;
        }

        public void AttachGem(Gem gem)
        {
            _attachedGem = gem;
            gem.Move(X, Y);
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
