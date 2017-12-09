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

        public override ICell GetTargetCell(Direction direction)
        {
            return null;
        }


        public override void ValidateGemHandling()
        {
            DetachGemBase();
        }

        public override Task PickGem(IGem gem)
        {
            base.PickGem(gem);
            Logger.Log(AttachedGem.ToString());

            return gem.PerformAction(() => _blackholeGem.Swallow(), () => gem.Die());
        }

    }
}
