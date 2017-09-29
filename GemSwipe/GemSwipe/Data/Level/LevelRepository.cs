using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Data.Level
{
    public class LevelRepository
    {
        private IDictionary<int,LevelConfiguration> _listOfLevelsConfigurations;

        public LevelConfiguration GetLevelConfigurationById(int id)
        {
            return _listOfLevelsConfigurations[id];
        }

        public void BuildListOfLevels(string json)
        {
            
        }

    }
}
