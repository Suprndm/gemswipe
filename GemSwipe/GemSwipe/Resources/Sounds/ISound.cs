using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Resources.Sounds
{
    public interface ISound
    {
        Task Play(bool loop=false);
        Task Pause();
        Task Stop();
        void Balance(float left);
        void Volume(float volume);
    }
}
