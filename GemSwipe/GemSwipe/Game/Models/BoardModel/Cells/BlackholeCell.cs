using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Models.BoardModel.Gems;
using GemSwipe.Game.Models.Entities;
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
            _blackholeGem = new BlackholeGem(boardX, boardY, 0, board.ToGemX(boardX, radius), board.ToGemY(boardY, radius), radius, randomizer, board);
            board.AddChild(_blackholeGem);
        }

        public override bool CanActivate()
        {
            return base.CanActivate();
        }

        public override bool CanProcess(IGem gem)
        {
            return base.CanProcess(gem);
        }

        public override ICell GetTargetCell(Direction direction)
        {
            return base.GetTargetCell(direction);
        }

        public override Task TryHandleGem(IGem gem, Direction direction)
        {
            return base.TryHandleGem(gem, direction);
        }

        public override Task TryReceiveGem(IGem gem, Direction direction, ICell senderCell)
        {
            //return base.TryReceiveGem(gem, direction, senderCell);
            senderCell.DetachGemBase();
            senderCell.Reinitialize();

            //AttachGem(gem);
            Reinitialize();
            _blackholeGem.Swallow();
            return gem.PerformAction(gem.Move(IndexX, IndexY, true),gem.Die());

        }


    }
}
