using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Services.Sound
{
    public interface ISound
    {
        Task Play(bool loop = false);
        Task Pause();
        Task FadeOut(int fadingDurationMs);
        Task Stop();
        float Volume { get; set; }
    }
}
