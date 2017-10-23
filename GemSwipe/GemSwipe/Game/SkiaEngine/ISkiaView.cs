using System;
using System.Collections.Generic;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.SkiaEngine
{
    public interface ISkiaView
    {
        float X { get; }
        float Y { get; }

        SKRect GetHitbox(); 

        float Scale { get; }

        float Height { get; }
        float Width { get; }

        decimal ZIndex { get; set; }
        decimal VisualTreeDepth { get; set; }
        bool ToDispose { get; }

        bool IsVisible { get;}
        bool IsEnabled { get;}
        float Opacity { get; }

        ISkiaView Parent { get; set; }

        void DeclareTappable(ISkiaView child);

        IList<ISkiaView> Tappables { get; }

        void AddChild(ISkiaView child);
        void RemoveChild(ISkiaView child);
        void Render();

        void SetCanvas(SKCanvas canvas);

        bool HitTheBox(Point p);

        void InvokeDown();
        void InvokeUp();
        void InvokePan(Point p);
        void InvokeDragOut();

        void Dispose();

    }
}
