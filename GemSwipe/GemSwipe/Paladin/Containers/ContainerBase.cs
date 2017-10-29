using GemSwipe.Paladin.Core;

namespace GemSwipe.Paladin.Containers
{
    public abstract class ContainerBase:SkiaView
    {
        protected ContainerBase(float x, float y, float height, float width) : base(x, y, height, width)
        {
            
        }

        public virtual void AddContent(ISkiaView skiaView)
        {             
            AddChild(skiaView);
        }

        protected override void Draw()
        {
            
        }
    }
}
