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
    public abstract class SkiaView : IAnimatable, ISkiaView
    {
        public float X { get; protected set; }
        public float Y { get; protected set; }
        public float Height { get; protected set; }
        public float Width { get; protected set; }
        public int ZIndex { get; set; }
        public bool ToDispose { get; protected set; }
        public SKCanvas Canvas { get; protected set; }
        public IList<ISkiaView> Children { get; protected set; }
        public ISkiaView Parent { get; set; }

        public void AddChild(ISkiaView child)
        {
            Children.Add(child);
            child.Parent = this;
        }

        public void RemoveChild(ISkiaView child)
        {
            child.Dispose();
            Children.Remove(child);
        }

        protected abstract void Draw();

        public void Render()
        {
            Draw();

            foreach (var child in Children.OrderByDescending(child => child.ZIndex))
            {
                if (child.ToDispose)
                    RemoveChild(child);
                else
                    child.Render();
            }
        }

        protected SkiaView(SKCanvas canvas, float x, float y, float height, float width)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
            Canvas = canvas;
            Children = new List<ISkiaView>();
        }

        public virtual void Dispose()
        {
            ToDispose = true;
            foreach (var child in Children)
            {
                child.Dispose();
            }

            Children.Clear();
        }

        public void BatchBegin()
        {
        }

        public void BatchCommit()
        {
        }
    }
}
