using System.Collections.Generic;
using GemSwipe.Game.Events;

namespace GemSwipe.Data.LevelData
{
    public class WorldData
    {
        public int Id { get; set; }
        public string Story { get; set; }
        public string Title { get; set; }
        public IList<int> LevelIds { get; set; }

        public WorldData()
        {

        }
    }
}
