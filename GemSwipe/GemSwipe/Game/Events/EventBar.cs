using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Containers;
using GemSwipe.Game.Entities;
using GemSwipe.Game.SkiaEngine;
using Xamarin.Forms;

namespace GemSwipe.Game.Events
{
    public class EventBar:SkiaView
    {
        private IList<IEvent> _events;
        private float _eventWidth;
        private float _eventMargin;
        private Container _eventContainer;
        private EventCounter _eventCounter;

        public EventBar(float x, float y, float height, float width) : base(x, y, height, width)
        {
            _y = -Height;
            _eventWidth = width / 10;
            _eventMargin = _eventWidth * 1f;

            _eventContainer = new Container();
            _eventContainer.X = Width;
            AddChild(_eventContainer);

            var counterWidth = height;
            _eventCounter = new EventCounter(width - counterWidth, 0, Height, counterWidth);
            AddChild(_eventCounter);
        }



        public async Task Initialize(IList<EventType> eventTypes)
        {
            _events = new List<IEvent>();
            for (int i = 0; i < eventTypes.Count; i++)
            {
                var eventType = eventTypes[i];
                var newEvent = BuildEvent(eventType, i);
                _events.Add(newEvent);
                _eventContainer.AddContent(newEvent);
            }

            _eventCounter.UpdateCount(_events.Count);

            this.Animate("eventBarTranslation", p => _y = (float)p, _y, 0, 4, 1000, Easing.SpringOut);
            await Task.Delay(800);

            this.Animate("eventContainerTranslation", p => _eventContainer.X = (float)p, _eventContainer.X, 0, 1, 2000, Easing.SpringOut);

            await Task.Delay(1800);

            _events.First().Warmup();
        }

        public async Task<bool> ActivateNextEventEvent(Board board)
        {
            var eventToActivate = _events.First();
            var succeeded = await eventToActivate.Activate(board);

            _events.RemoveAt(0);
            _eventCounter.UpdateCount(_events.Count);

            Slide();


            return succeeded;
        }

        public int GetEventsCount()
        {
            return _events.Count;
        }

        private async Task Slide()
        {

            if (_events.Count > 0)
            {
                await _events.First().Warmup();
            }

            this.Animate("eventContainerTranslation", p => _eventContainer.X = (float)p, _eventContainer.X, _eventContainer.X-(_eventMargin+ _eventWidth), 1, 1000, Easing.SpringOut);
            await Task.Delay(1000);
        }

        protected override void Draw()
        {
            DrawHitbox();
        }

        IEvent BuildEvent(EventType type, int index)
        {
            var posX = (_eventMargin + _eventWidth ) * (1 + index) - _eventMargin*0.75f;
            var posY = Height / 2;
            IEvent newEvent = null;
            switch (type)
            {
                case EventType.Empty:
                    newEvent = new EmptyEvent(posX, posY, _eventWidth, _eventWidth);
                    break;
                case EventType.Energy:
                    newEvent = new EnergyEvent(posX, posY, _eventWidth, _eventWidth);
                    break;
                case EventType.Explosion:
                    newEvent = new EmptyEvent(posX, posY, _eventWidth, _eventWidth);
                    break;
                case EventType.BlackHole:
                    newEvent = new EmptyEvent(posX, posY, _eventWidth, _eventWidth);
                    break;
                case EventType.WhiteHole:
                    newEvent = new EmptyEvent(posX, posY, _eventWidth, _eventWidth);
                    break;
                case EventType.TimeWarp:
                    newEvent = new EmptyEvent(posX, posY, _eventWidth, _eventWidth);
                    break;
                case EventType.Tempest:
                    newEvent = new TempestEvent(posX, posY, _eventWidth, _eventWidth);
                    break;
            }
            return newEvent;
        }
    }
}
