using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.SkiaEngine;

namespace GemSwipe.Game.Containers
{
    public class CenteredContainer:ContainerBase
    {
        public CenteredContainer(float x, float y, float height, float width) : base(x, y, height, width)
        {
        }

        public override void AddContent(ISkiaView skiaView)
        {
            skiaView.Y = Height / 2;
            skiaView.X = Width / 2;

            base.AddContent(skiaView);
        }
    }
}
