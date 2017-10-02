namespace GemSwipe.Game.Models
{
    public class TransitionFloorSetup
    {
        public TransitionFloorSetup(int floor, string quote, string title)
        {
            Floor = floor;
            Quote = quote;
            Title = title;
        }

        public string Title { get; }
        public string Quote { get; }
        public int Floor { get; }

    }
}
