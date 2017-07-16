using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace GemSwipe.GameEngine
{
    public abstract class GLViewBase : IGLView
    {

        public float X { get; protected set; }
        public float Y { get; protected set; }
        public float Height { get; protected set; }
        public float Width { get; protected set; }


        protected GLViewBase(float x, float y, float height, float width)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }

        public abstract void UpdateState();
        public abstract void Draw(SKCanvas canvas);

        public void Refresh(SKCanvas canvas)
        {
            UpdateState();
            Draw(canvas);
        }

        public abstract void MoveTo(float x, float y);

        public abstract void Dispose();

    }
}
