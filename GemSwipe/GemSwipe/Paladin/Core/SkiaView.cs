using System;
using System.Collections.Generic;
using System.Linq;
using GemSwipe.Game.Models;
using GemSwipe.Game.Models.Entities;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Paladin.Core
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

        public bool IsEnabled
        {
            get
            {
                if (Parent != null)
                    return _isEnabled && Parent.IsEnabled;
                return _isEnabled;
            }
            protected set => _isEnabled = value;
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
            set => _opacity = value;
        }

        private decimal _visualTreeDepth;
        public decimal VisualTreeDepth
        {
            get
            {
                if (Parent != null)
                {
                    return  0.1m * Parent.VisualTreeDepth;
                }
                return _visualTreeDepth;
            }
            set { _visualTreeDepth = value; }
        }

        private decimal _zIndex;
        public decimal ZIndex
        {
            get
            {
                if (Parent != null)
                {
                    return _zIndex * 0.1m * Parent.VisualTreeDepth + Parent.ZIndex;
                }
                return _zIndex;
            }
            set { _zIndex = value; }
        }

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
        private IList<ISkiaView> _children;
        private bool _tappable;
        private bool _isVisible;
        private bool _isEnabled;
        public ISkiaView Parent { get; set; }
        protected SKColor BackgroundColor { get; set; }

        public void AddChild(ISkiaView child)
        {
            lock (_children)
            {
                decimal zindex = 0.4m;
                child.SetCanvas(Canvas);

                if ( _children.Count > 0)
                {
                    zindex = 1-1m/(_children.Count + 1);
                }
               
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
            var children = _children.ToList();
            foreach (var child in children)
            {
                child.SetCanvas(canvas);
            }
        }

        #region TapEvents


        public IList<ISkiaView> Tappables { get; private set; }

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

        public void InvokePan(Point p)
        {
            Pan?.Invoke(p);
        }

        public void InvokeDragOut()
        {
            DragOut?.Invoke();
        }

        public void InvokeSwipe(Direction direction)
        {
            Swippe?.Invoke(direction);
        }

        public event Action Down;
        public event Action Up;
        public event Action<Point> Pan;
        public event Action DragOut;
        public event Action<Direction> Swippe;

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

        protected SkiaView()
        {
            _x = 0;
            _y = 0;
            Height = SkiaRoot.ScreenHeight;
            Width = SkiaRoot.ScreenWidth;

            Initialize();
        }


        protected SkiaView(float x, float y, float height, float width)
        {
            _x = x;
            _y = y;
            Height = height;
            Width = width;

            Initialize();
        }

        private void Initialize()
        {
            _opacity = 1;
            _visualTreeDepth = 1;
            _isVisible = true;
            _isEnabled = true;

            _zIndex = 1;

            Tappables = new List<ISkiaView>();
            _children = new List<ISkiaView>();

            Scale = 1;
        }

        public virtual void Dispose()
        {
            lock (_children)
            {
                ToDispose = true;
                foreach (var child in _children)
                {
                    child.Dispose();
                }

                _children.Clear();
            }
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
