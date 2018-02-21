using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Services.Sound
{
    public class AudioTrack : ISound
    {
        public float Volume
        {
            get
            {
                return (float)_audioStreamPlayer.Volume;
            }
            set
            {
                float volume;
                if (value < 0)
                {
                    volume = 0;
                }
                else if (value > 1)
                {
                    volume = 1;
                }
                else
                {
                    volume = value;
                }
                _audioStreamPlayer.Volume = volume;
            }
        }

        private Stream _audioStream;
        private Plugin.SimpleAudioPlayer.Abstractions.ISimpleAudioPlayer _audioStreamPlayer;

        public AudioTrack(string audioName)
        {
            string path = $"Resources/Sounds/{audioName}";
            _audioStream = SoundService.Instance.LoadStream(path);
            _audioStreamPlayer = SoundService.Instance.CreatePlayer();
            _audioStreamPlayer.Load(_audioStream);
        }

        public Task Play(bool loop = false)
        {
            try
            {
                _audioStreamPlayer.Loop = loop;
                _audioStreamPlayer.Play();
                _audioStreamPlayer.PlaybackEnded += (e, s) => Stop();
                return Task.Delay(0);
            }
            catch (NotImplementedException e)
            {
                throw e;
            }
        }

        public Task Pause()
        {
            _audioStreamPlayer.Pause();
            return Task.Delay(0);
        }
        
        public Task Stop()
        {
            _audioStreamPlayer.Stop();
            _audioStreamPlayer.Dispose();
            _audioStream.Dispose();
            return Task.Delay(0);
        }

        public async Task FadeOut(int fadingDurationMs)
        {
            if (fadingDurationMs != 0)
            {
                float baseVolume = Volume;
                float fadingSpeed = baseVolume / fadingDurationMs;

                int numberOfIntervals = 100;
                float fadingInterval = fadingDurationMs / numberOfIntervals;
                for (int i = 0; i < numberOfIntervals; i++)
                {
                    await Task.Delay((int)fadingInterval);
                    Volume -= fadingSpeed * fadingInterval;
                }
                Volume = 0;
            }
            else
            {
                Volume = 0;
            }
            _audioStreamPlayer.PlaybackEnded += (e, s) => Stop();
        }
    }
}
