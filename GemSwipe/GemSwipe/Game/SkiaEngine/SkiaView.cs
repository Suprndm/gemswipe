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
            set => _x = value;
        }

        protected float _y;
        public float Y
        {
            get
            {
                if (Parent != null) return _y * Scale + (1 - _scale) * _height / 2 + Parent.Y;
                return _y;
            }
            set => _y = value;
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


        public SKCanvas Canvas { get; protected set; }
        private readonly IList<ISkiaView> _children;
        private bool _tappable;
        private bool _isVisible;
        public ISkiaView Parent { get; set; }
        protected SKColor BackgroundColor { get; set; }


        public void AddChild(ISkiaView child, int zindex = 0)
        {
            lock (_children)
            {
                child.SetCanvas(Canvas);
                child.ZIndex = zindex;
                _children.Add(child);
                child.Parent = this;

                foreach (var tappable in child.Tappables)
                {
                    DeclareTappable(tappable);
                }

                child.Tappables.Clear();
            }
        }

        public void RemoveChild(ISkiaView child)
        {
                _children.Remove(child);
                child.SetCanvas(null);
        }

        protected abstract void Draw();

        public void Render()
        {
            if (!IsVisible) return;

            Draw();

            lock (_children)
            {
                var children = _children.ToList();

                foreach (var child in children.OrderBy(child => child.ZIndex).ToList())
                {
                    if (child.ToDispose)
                        RemoveChild(child);
                    else
                        child.Render();
                }
            }
        }

        public void SetCanvas(SKCanvas canvas)
        {
            Canvas = canvas;
            foreach (var child in _children)
            {
                child.SetCanvas(canvas);
            }
        }

        #region TapEvents


        public IList<ISkiaView> Tappables { get; }

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

        public void DetectDown(Point p)
        {
            // Clear Tappables 
            foreach (var child in Tappables.Where(child => child.ToDispose).ToList())
            {
                Tappables.Remove(child);
            }

            // Detect
            foreach (var tappable in Tappables)
            {
                if (tappable.HitTheBox(p))
                {
                    tappable.InvokeDown();
                    return;
                }
            }
        }

        public void DetectUp(Point p)
        {
            // Clear Tappables 
            foreach (var child in Tappables.Where(child => child.ToDispose).ToList())
            {
                Tappables.Remove(child);
            }

            // Detect
            foreach (var tappable in Tappables)
            {
                if (tappable.HitTheBox(p))
                {
                    tappable.InvokeUp();
                    return;
                }
            }
        }

        public bool HitTheBox(Point p)
        {
            var hitbox = GetHitbox();

            if (IsVisible && p.X >= hitbox.Left && p.Y >= hitbox.Top && p.X <= hitbox.Right && p.Y <= hitbox.Bottom)
            {
                return true;
            }

            return false;
        }

        public void InvokeDown()
        {
            Down?.Invoke();
        }

        public void InvokeUp()
        {
            Up?.Invoke();
        }

        public event Action Down;
        public event Action Up;

        #endregion

        protected void DrawHitbox()
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = new SKColor(255, 255, 255, 50);
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



        protected SkiaView(float x, float y, float height, float width)
        {
            _opacity = 1;
            _isVisible = true;

            _x = x;
            _y = y;

            Height = height;
            Width = width;

            Tappables = new List<ISkiaView>();
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

        protected SKColor CreateColor(SKColor color)
        {
            return CreateColor(color.Red, color.Green, color.Blue, (byte)(color.Alpha * Opacity));
        }

        protected SKColor CreateColor(byte r, byte g, byte b, byte a = 255)
        {
            return new SKColor(r, g, b, (byte)(a * Opacity));
        }
    }
}
