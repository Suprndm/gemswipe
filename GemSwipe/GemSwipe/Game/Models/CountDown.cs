using System;
using Xamarin.Forms;

namespace GemSwipe.Game.Models
{
    public class CountDown : IDisposable
    {
        private double _secondsToGo;
        private bool _isRunning;
        private double _pastSeconds;
        private bool _isDisposed;

        private const int MilisecondPerTick = 100;

        public event Action Zero;

        public CountDown(int seconds)
        {
            Reset(seconds);

            Device.StartTimer(TimeSpan.FromMilliseconds(MilisecondPerTick), () =>
            {
                if (_isRunning)
                {
                    _pastSeconds += (double)MilisecondPerTick / 1000;

                    if (_secondsToGo - _pastSeconds <= 0)
                    {
                        Zero?.Invoke();
                        Stop();
                    }
                }

                return !_isDisposed;
            });
        }

        public void Start()
        {
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;

        }

        public double RemainingSeconds()
        {
            return _secondsToGo - _pastSeconds;
        }

        public void Reset(int to)
        {
            _secondsToGo = to;
            _pastSeconds = 0;
        }

        public void AddMoreTime(int seconds)
        {
            _secondsToGo += seconds;
        }

        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}
