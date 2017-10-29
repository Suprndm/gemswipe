using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;

namespace GemSwipe.Game.Pages.Game
{
    public class ObjectivesView : SkiaView
    {
        private readonly IDictionary<int, ObjectiveView> _objectives;

        public ObjectivesView(IDictionary<int, int> objectives, bool inGame, float x, float y, float height) : base(x, y, height, height)
        {
            _objectives = new Dictionary<int, ObjectiveView>();
            var totalWidth = height * objectives.Count;
            Width = totalWidth;
            int count = 0;
            foreach (var objective in objectives)
            {
                var xPos = -totalWidth / 2 + height * count;
                var objectiveView = new ObjectiveView(objective, inGame, xPos, 0, height, height);

                _objectives.Add(objective.Key, objectiveView);

                AddChild(objectiveView);
                count++;
            }
        }

        public void UpdateObjective(int size, int count)
        {
            _objectives[size].UpdateCount(count);
        }

        protected override void Draw()
        {

        }
    }
}
