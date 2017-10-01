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

        public LevelConfiguration(string jsonLevelData)
        {
            LevelConfiguration lvl = JsonConvert.DeserializeObject<LevelConfiguration>(jsonLevelData);
        }

        public LevelConfiguration(int id, string boardSetupString, int nbOfMoves, string story, string title)
        {
            Id = id;
            BoardSetupString = boardSetupString;
            NbOfMovesToSolve = nbOfMoves;
            Story = story;
            Title = title;
        }
    }
}
