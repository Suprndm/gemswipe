using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Animations
{
    public class AnimationStack
    {
        private AnimationStack _parentStack;
        private IList<Action<AnimationArg>> _animationStack;
        private AnimationArg _animationArg;

        public void RunAnimation()
        {
            foreach (Action<AnimationArg> animation in _animationStack)
            {
                animation.Invoke(new AnimationArg());
            }
        }
    }
}
