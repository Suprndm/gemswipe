using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace GemSwipe.GameEngine
{
    public class BoardFactoryView:SkiaView
    {
        public BoardFactoryView(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
        }

        protected override void Draw()
        {
            throw new NotImplementedException();
        }
    }
}
