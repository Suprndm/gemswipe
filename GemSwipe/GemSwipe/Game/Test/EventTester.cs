using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Events;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Game.Popups;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.UIElements.Buttons;
using Newtonsoft.Json;

namespace GemSwipe.Game.Test
{
    class EventTester : SkiaView
    {
        private EventBar _eventBar;
        public EventTester()
        {
            var popButton = new TextButton(Width / 2, 9.5f * Height / 10, Height / 40, "Pop !");
            AddChild(popButton);

            popButton.Activated += PopButton_Activated;

            var objectives = new Dictionary<int, int>();
            objectives.Add(2, 8);
            objectives.Add(4, 5);
            objectives.Add(6, 1);
            objectives.Add(8, 3);

            var events = new List<EventType>()
            {
                EventType.Empty,
                EventType.Energy,
                EventType.Empty,
                EventType.Energy,
                EventType.Empty,
                EventType.Energy,
                EventType.Empty,
                EventType.Energy,
                EventType.Empty,
                EventType.Energy,
            };


            var levelData = new LevelData()
            {
                BoardSetupString = "",
                Columns = 4,
                Rows = 4,
                Id = 1,
                Objectives = objectives,
                Events = events
            };

            var json = JsonConvert.SerializeObject(levelData);
            _eventBar = new EventBar(0, 0, 0.1f * Height, Width);
            AddChild(_eventBar);

            _eventBar.Initialize(levelData.Events);

            var objectivesView = new ObjectivesView(objectives, false, Width / 2, 0.2f * Height, 0.1f * Height);

            AddChild(objectivesView);

        }

        private void PopButton_Activated()
        {
            _eventBar.ActivateNextEventEvent(null);
        }


        protected override void Draw()
        {
        }
    }
}
