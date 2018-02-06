using System;
using System.Collections.Generic;
using System.Text;
using GemSwipe.Paladin.Core;

namespace GemSwipe.Paladin.Behaviors
{
    public interface IBehavior:IDisposable
    {
        bool IsDisposed();
        void Attach(SkiaView skiaView);
        void Detach();
        void Update();
    }
}
