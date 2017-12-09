using GemSwipe.Game.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.BoardModel.Cells
{
    public class PipeCell : CellBase
    {
        public ICell ExitCell;

        public PipeCell(int boardX, int boardY, Board board, string portalId) : base(boardX, boardY, board)
        {
        }

        public void SetExit(ICell cell)
        {
            ExitCell = cell; 
        }

        public override ICell GetTargetCell(Direction direction)
        {
            return ExitCell;
        }
    }
}
