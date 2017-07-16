using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace GemSwipe.GameEngine
{
    public interface IGLView
    {
        void Refresh(SKCanvas canvas);
        void MoveTo(float x, float y);
        void Dispose();

        float X { get; }
        float Y { get; }
        float Height { get; }
        float Width { get; }
    }
}
