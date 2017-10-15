namespace GemSwipe.Data.LevelData
{
    public class LevelData
    {
        public int Id { get; set; }
        public string BoardSetupString { get; set; }
        public int NbOfMovesToSolve { get; set; }
        public string Story { get; set; }
        public string Title { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public LevelData()
        {
        }
    }
}
