using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Events;
using GemSwipe.Game.Models;
using GemSwipe.Game.Models.Entities;

namespace GemSwipe.Generator
{
    public class Evaluator
    {
        public double ComputeDifficulty(LevelData levelData)
        {
            var board = new Board(levelData.BoardSetupString);

            int initialGemsScore = board.Gems.Sum(g => GemsNeeded(g.Size));

            int gemsNeeded = levelData.Objectives.Sum(kvp => kvp.Value * GemsNeeded(kvp.Key));

            int eventsModifier = levelData.Events.Sum(e => GemsModifier(e));

            var ratio = ((double) gemsNeeded / (initialGemsScore + eventsModifier));

            return ratio;
        }


        private int GemsNeeded(int size)
        {
            return (int)Math.Pow(2, size - 1);
        }

        private int GemsModifier(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Empty:
                    return 0;
                    break;
                case EventType.Energy:
                    return 1;
                    break;
                case EventType.Explosion:
                    return 0;
                    break;
                case EventType.Blackhole:
                    return 0;
                    break;
                case EventType.WhiteHole:
                    return 0;
                    break;
                case EventType.TimeWarp:
                    return 0;
                    break;
                case EventType.Tempest:
                    return 4;
                    break;
            }

            return 0;
        }
    }
}
