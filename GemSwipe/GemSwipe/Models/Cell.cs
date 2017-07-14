using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

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
        }

        public void DetachGem(Gem gem)
        {
            _attachedGem = null;
        }

        public Gem GetAttachedGem()
        {
            return _attachedGem;
        }
    }
}
