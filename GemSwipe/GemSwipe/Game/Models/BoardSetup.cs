using GemSwipe.Data.LevelData;

namespace GemSwipe.Game.Models
{
    public struct BoardSetup
    {
        public BoardSetup(int levelId, int rows, int columns, string setupString)
        {
            LevelId = levelId;
            Rows = rows;
            Columns = columns;
            SetupString = setupString;
        }

        public BoardSetup(LevelData levelDataConfig)
        {
            LevelId = levelDataConfig.Id;
            Rows = levelDataConfig.Rows;
            Columns = levelDataConfig.Columns;
            SetupString = levelDataConfig.BoardSetupString;
        }

        public int LevelId { get; set; }
        public int Rows { get; }
        public int Columns { get; }
        public string SetupString { get; }
    }
}
