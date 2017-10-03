namespace GemSwipe.Game.Models
using GemSwipe.Data.Level;
{
    public class TransitionFloorSetup
    {
        public TransitionFloorSetup(int floor, string quote, string title)
        {
            Floor = floor;
            Quote = quote;
            Title = title;
        }

        public TransitionFloorSetup(int floor, LevelConfiguration levelconfig)
        {
            Floor = floor;
            Quote = levelconfig.Story;
            Title = levelconfig.Title;
        }

        public string Title { get; }
        public string Quote { get; }
        public int Floor { get; }

    }
}
