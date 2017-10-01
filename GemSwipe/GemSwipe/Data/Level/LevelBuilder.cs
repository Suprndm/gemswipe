using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GemSwipe.Data.Level
{

    public static class LevelBuilder
    {
        public static LevelConfiguration JSONDeserializedLevel(string json)
        {
            return JsonConvert.DeserializeObject<LevelConfiguration>(json);
        }

        public static void JSONWriteLevel(LevelConfiguration levelConfig)
        {
            
        }
    }
}
