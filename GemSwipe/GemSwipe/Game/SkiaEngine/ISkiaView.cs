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

        int ZIndex { get; set; }
        bool ToDispose { get; }

        bool IsVisible { get;}
        float Opacity { get; }

        ISkiaView Parent { get; set; }

        void DeclareTappable(ISkiaView child);

        IList<ISkiaView> Tappables { get; }

        void AddChild(ISkiaView child, int zindex);
        void RemoveChild(ISkiaView child);
        void Render();

        void SetCanvas(SKCanvas canvas);

        void DetectTap(Point p);

        void Tap();
        event Action Tapped;

        void Dispose();

    }
}
