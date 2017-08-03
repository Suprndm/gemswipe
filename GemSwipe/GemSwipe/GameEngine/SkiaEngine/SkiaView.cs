using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.GameEngine.SkiaEngine
{
    public abstract class SkiaView : IAnimatable, ISkiaView, IDisposable
    {
        protected float _scale;
        public float Scale
        {
            get
            {
                if (Parent != null) return _scale * Parent.Scale;
                return _scale;
            }
            protected set => _scale = value;
        }


        protected float _x;
        public float X
        {
            get
            {
                if (Parent != null) return _x * Scale + (1 - _scale) * _width / 2 + Parent.X;
                return _x;
            }
            protected set => _x = value;
        }

        protected float _y;
        public float Y
        {
            get
            {
                if (Parent != null) return _y * Scale +(1-_scale)*_height/2 + Parent.Y;
                return _y;
            }
            protected set => _y = value;
        }

        protected float _width;
        public float Width
        {
            get
            {
                if (Parent != null) return _width * Scale;
                return _width;
            }
            protected set => _width = value;
        }

        protected float _height;
        public float Height
        {
            get
            {
                if (Parent != null) return _height * Scale;
                return _height;
            }
            protected set => _height = value;
        }

        public int ZIndex { get; set; }
        public bool ToDispose { get; protected set; }
        public SKCanvas Canvas { get; protected set; }
        private readonly IList<ISkiaView> _children;
        public ISkiaView Parent { get; set; }

        public void AddChild(ISkiaView child, int zindex = 0)
        {
            child.ZIndex = zindex;
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

            foreach (var child in _children.OrderBy(child => child.ZIndex).ToList())
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

            Scale = 1;
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
