﻿using GemSwipe.Game.Models.BoardModel.Gems;
using GemSwipe.Game.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.BoardModel.Cells
{
    public class BlockingCell : Cell
    {
        public BlockingCell(int boardX, int boardY, Board board, Random randomizer) : base(boardX, boardY, board)
        {
            float radius = board.GetGemSize();
            IsBlocked = true;
            //BlackholeGem _blackholeGem = new BlackholeGem(boardX, boardY, 0, board.ToGemX(boardX), board.ToGemY(boardY), radius, randomizer, board);
            //board.AddChild(_blackholeGem);
        }

        public override Task Handle(IGem gem, ICell senderCell)
        {
            return ReturnToSender(gem, senderCell);
        }
    }
}
