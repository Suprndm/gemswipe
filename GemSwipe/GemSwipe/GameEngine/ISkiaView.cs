using System.Collections.Generic;

namespace GemSwipe.GameEngine
{
    public interface ISkiaView
    {
        float X { get; }
        float Y { get; }
        float Height { get; }
        float Width { get; }
        int ZIndex { get; set; }
        bool ToDispose { get; }

        ISkiaView Parent { get; set; }

        void AddChild(ISkiaView child, int zindex);
        void RemoveChild(ISkiaView child);
        void Render();
        void Dispose();

    }
}
