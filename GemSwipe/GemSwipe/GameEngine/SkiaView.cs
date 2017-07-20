using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace GemSwipe.GameEngine
{
    public abstract class SkiaView : IAnimatable, ISkiaView, IDisposable
    {
        protected float _x;
        public float X
        {
            get
            {
                if (Parent != null) return _x + Parent.X;
                return _x;
            }
            protected set => _x = value;
        }

        protected float _y;
        public float Y
        {
            get
            {
                if (Parent != null) return _y + Parent.Y;
                return _y;
            }
            protected set => _y = value;
        }

        public float Height { get; protected set; }
        public float Width { get; protected set; }
        public int ZIndex { get; set; }
        public bool ToDispose { get; protected set; }
        public SKCanvas Canvas { get; protected set; }
        private readonly IList<ISkiaView> _children;
        public ISkiaView Parent { get; set; }

        public void AddChild(ISkiaView child, int zindex = 0)
        {
            child.ZIndex = 0;
            _children.Add(child);
            child.Parent = this;
        }

        public void RemoveChild(ISkiaView child)
        {
            child.Dispose();
            _children.Remove(child);
        }

        protected abstract void Draw();

        public void Render()
        {
            Draw();

            foreach (var child in _children.OrderBy(child => child.ZIndex))
            {
                if (child.ToDispose)
                    RemoveChild(child);
                else
                    child.Render();
            }
        }

        protected SkiaView(SKCanvas canvas, float x, float y, float height, float width)
        {
            _x = x;
            _y = y;
            Height = height;
            Width = width;
            Canvas = canvas;
            _children = new List<ISkiaView>();
        }

        public virtual void Dispose()
        {
            ToDispose = true;
            foreach (var child in _children)
            {
                child.Dispose();
            }

            _children.Clear();
        }

        public void BatchBegin()
        {
        }

        public void BatchCommit()
        {
        }
    }
}
