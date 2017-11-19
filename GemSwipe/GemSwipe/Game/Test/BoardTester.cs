using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Game.Events;
using GemSwipe.Game.Models;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.UIElements.Buttons;

namespace GemSwipe.Game.Test
{
    public class BoardTester : SkiaView
    {
        public BoardTester()
        {
            var levelDataRepository = new LevelDataRepository();
            var board = new Board(new BoardSetup(levelDataRepository.Get(1)));

            AddChild(board);

        }

        protected override void Draw()
        {
        }
    }
}
