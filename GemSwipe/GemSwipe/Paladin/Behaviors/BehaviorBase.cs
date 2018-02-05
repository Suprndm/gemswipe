using GemSwipe.Paladin.Core;
using Xamarin.Forms;

namespace GemSwipe.Paladin.Behaviors
{
    public abstract class BehaviorBase : IBehavior, IAnimatable
    {
        protected SkiaView View { get; private set; }

        private bool _isDisposed;

        public bool IsDisposed()
        {
            return _isDisposed;
        }

        public virtual void Attach(SkiaView skiaView)
        {
            View = skiaView;
        }

        public virtual void Detach()
        {
            View = null;
        }

        public void Update()
        {
            if (View == null || IsDisposed())
                return;

            OnUpdated();
        }

        public abstract void OnUpdated();

        public void BatchBegin()
        {
        }

        public void BatchCommit()
        {
        }

        public virtual void Dispose()
        {
            _isDisposed = true;
        }
    }
}
