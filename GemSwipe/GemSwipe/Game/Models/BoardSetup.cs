namespace GemSwipe.Game.Models
{
    public struct BoardSetup
    {
        public BoardSetup(int levelId, int rows, int columns, string setupString, int moves)
        {
            LevelId = levelId;
            Rows = rows;
            Columns = columns;
            SetupString = setupString;
            Moves = moves;
        }

        public int LevelId { get; set; }
        public int Rows { get; }
        public int Columns { get; }
        public string SetupString { get; }
        public int Moves { get;}
    }
}
