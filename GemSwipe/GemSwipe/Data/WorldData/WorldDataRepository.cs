using System.Linq;

namespace GemSwipe.Data.LevelData
{
    public class WorldDataRepository:RepositoryBase<WorldData>
    {
        public int GetWorldIdByLevelId(int levelId)
        {
            return Data.First(w => w.LevelIds.Contains(levelId)).Id;
        }
    }
}
