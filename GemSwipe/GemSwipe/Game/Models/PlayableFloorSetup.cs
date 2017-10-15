using GemSwipe.Data.LevelData;

namespace GemSwipe.Game.Models
{
    public class PlayableFloorSetup
    {
        public PlayableFloorSetup(BoardSetup boardSetup, int floor, bool isFinal, string title)
        {
            BoardSetup = boardSetup;
            Floor = floor;
            IsFinal = isFinal;
            Title = title;
        }

       public PlayableFloorSetup(LevelData levelDataConfig)
        {

        }

        public string Title { get; }
        public BoardSetup BoardSetup { get; }
        public int Floor { get; }
        public bool IsFinal { get; }

    }
}
