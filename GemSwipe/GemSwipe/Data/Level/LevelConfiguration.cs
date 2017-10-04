using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace GemSwipe.Data.Level
{
    public class LevelConfiguration
    {
        public int Id { get; set; }
        public string BoardSetupString { get; set; }
        public int NbOfMovesToSolve { get; set; }
        public string Story { get; set; }
        public string Title { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public LevelConfiguration()
        {

        }
    }
}
