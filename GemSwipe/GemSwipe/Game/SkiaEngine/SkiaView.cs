using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.SkiaEngine
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
                if (Parent != null) return _y * Scale + (1 - _scale) * _height / 2 + Parent.Y;
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

        protected float _opacity;
        public float Opacity
        {
            get
            {
                if (_opacity < 0)
                {
                    return 0;
                }
                else if (_opacity > 1)
                {
                    return 1;
                }
                else
                {

                    if (Parent != null)
                    {
                        return Parent.Opacity * _opacity;
                    }
                    else
                    {
                        return _opacity;
                    }
                }
            }
            protected set => _opacity = value;
        }

        public int ZIndex { get; set; }
        public bool ToDispose { get; protected set; }

        public bool IsVisible
        {
            get
            {
                if (Parent != null) return _isVisible && Parent.IsVisible;
                return _isVisible;
            }
            protected set => _isVisible = value;
        }

        public IList<ISkiaView> Tappables { get; }

        public SKCanvas Canvas { get; protected set; }
        private readonly IList<ISkiaView> _children;
        private bool _tappable;
        private bool _isVisible;
        public ISkiaView Parent { get; set; }
        protected SKColor BackgroundColor { get; set; }

        public void DeclareTappable(ISkiaView child)
        {
            if (Parent != null)
            {
                Parent.DeclareTappable(child);
            }
            else
            {
                Tappables.Add(child);
            }
        }

        public void AddChild(ISkiaView child, int zindex = 0)
        {
            child.ZIndex = zindex;
            _children.Add(child);
            child.Parent = this;

            foreach (var tappable in child.Tappables)
            {
                DeclareTappable(tappable);
            }

            child.Tappables.Clear();
        }

        public void RemoveChild(ISkiaView child)
        {
            _children.Remove(child);
        }

        protected abstract void Draw();

        public void Render()
        {
            if (!IsVisible) return;

            Draw();

            foreach (var child in _children.OrderBy(child => child.ZIndex).ToList())
            {
                if (child.ToDispose)
                    RemoveChild(child);
                else
                    child.Render();
            }
        }

        protected void DrawHitbox()
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = new SKColor(255,255,255,50);
                paint.Style = SKPaintStyle.Fill;
                Canvas.DrawRect(
                  GetHitbox(),
                    paint);
            }
        }

        public virtual SKRect GetHitbox()
        {
            return SKRect.Create(X, Y, Width, Height);
        }

        public void DetectTap(Point p)
        {
            // Clear Tappables 
            foreach (var child in Tappables.Where(child => child.ToDispose).ToList())
            {
                Tappables.Remove(child);
            }

            // Detect
            foreach (var tappable in Tappables)
            {
                var hitbox = tappable.GetHitbox();
                if (tappable.IsVisible && p.X >= hitbox.Left && p.Y >= hitbox.Top && p.X <= hitbox.Right && p.Y <= hitbox.Bottom)
                {
                    tappable.Tap();
                    return;
                }
            }
        }

        public void Tap()
        {
            Tapped?.Invoke();
        }

        public event Action Tapped;

        protected SkiaView(SKCanvas canvas, float x, float y, float height, float width)
        {
            _isVisible = true;

            _x = x;
            _y = y;

            Height = height;
            Width = width;

            Tappables = new List<ISkiaView>();
            _children = new List<ISkiaView>();

            BackgroundColor = new SKColor(255, 255, 255, 0);

            Canvas = canvas;

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
