using System.Collections.Generic;
using GemSwipe.Game.Events;

namespace GemSwipe.Data.LevelData
{
    public class LevelData
    {
        public int Id { get; set; }
        public string BoardSetupString { get; set; }
        public int Moves { get; set; }
        public string Story { get; set; }
        public string Title { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public IDictionary<int, int> Objectives { get; set; }
        public IList<EventType> Events { get; set; }

        public LevelData()
        {
        }
    }
}
