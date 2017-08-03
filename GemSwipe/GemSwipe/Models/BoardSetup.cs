using System.Collections.Generic;

namespace GemSwipe.Models
{
    public struct BoardSetup
    {
        public BoardSetup(int rows, int columns, string setupString)
        {
            Rows = rows;
            Columns = columns;
            SetupString = setupString;
        }

        public int Rows { get; }
        public int Columns { get; }
        public string SetupString { get; } 
    }
}
