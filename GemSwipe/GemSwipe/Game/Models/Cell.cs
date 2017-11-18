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
            SetModifier(gemType);
        }

        public Cell(int x, int y, Gem gem)
        {
            X = x;
            Y = y;
            if (gem != null)
            {
                SetModifier(gem.Type);
            }
        }

        public void SetModifier(GemType gemType)
        {
            switch (gemType)
            {
                default:
                    IsBlocked = false;
                    Modifier = CellModifier.Base;
                    break;
                case GemType.Blocking:
                    IsBlocked = true;
                    Modifier = CellModifier.Blocked;
                    break;
                case GemType.Teleportation:
                    IsBlocked = true;
                    Modifier = CellModifier.Teleporter;
                    break;
            }
        }

        private Gem _attachedGem;

        public bool IsEmpty()
        {
            return _attachedGem == null && !IsBlocked;
        }

        public void AttachGem(Gem gem)
        {
            if (gem != null)
            {
                _attachedGem = gem;
                SetModifier(gem.Type);
                gem.Move(X, Y);
            }
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
