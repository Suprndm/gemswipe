using System.Collections.Generic;
using GemSwipe.Game.Models;
using GemSwipe.Paladin.Behaviors;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Paladin.Core
{
    public interface ISkiaView
    {
        float X { get; set; }
        float Y { get; set; }

        SKRect GetHitbox(); 

        float Scale { get; }

        float Height { get; set; }
        float Width { get; set; }

        decimal ZIndex { get; set; }
        decimal VisualTreeDepth { get; set; }
        bool ToDispose { get; }

        bool IsVisible { get;}
        bool IsEnabled { get;}
        float Opacity { get; }

        ISkiaView Parent { get; set; }

        void DeclareTappable(ISkiaView child);
        void DeclarePannable(ISkiaView child);

        IList<ISkiaView> Tappables { get; }
        IList<ISkiaView> Pannables { get; }

        void AddChild(ISkiaView child);
        void RemoveChild(ISkiaView child);
        void Render();

        void AddBehavior(IBehavior behavior);
        void RemoveBehavior(IBehavior behavior);

        void SetCanvas(SKCanvas canvas);

        bool HitTheBox(Point p);

        void InvokeDown();
        void InvokeUp();
        void InvokePan(Point p);
        void InvokeDragOut();
        void InvokeSwipe(Direction direction);

        void Dispose();

    }
}
