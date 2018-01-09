using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Models.BoardModel.Gems;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.BoardModel.Cells
{
    public class BlackholeCell : Cell
    {
        private BlackholeGem _blackholeGem;

        public BlackholeCell(int boardX, int boardY, Board board, Random randomizer) : base(boardX, boardY, board)
        {
            float radius = board.GetGemSize();
            _blackholeGem = new BlackholeGem(boardX, boardY, 0, board.ToGemX(boardX), board.ToGemY(boardY), radius, randomizer, board);
            board.AddChild(_blackholeGem);
        }

        public async override Task Pick(IGem gem)
        {
            base.Pick(gem);
            await gem.PerformAction(() => _blackholeGem.Swallow(), () => gem.Die());
            UnassignGem();
        }

    }
}
