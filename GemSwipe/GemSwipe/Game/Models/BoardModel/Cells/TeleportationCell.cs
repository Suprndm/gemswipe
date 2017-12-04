using GemSwipe.Game.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.BoardModel.Cells
{
    public class TeleportationCell : Cell
    {
        public string PortalId { get; set; }
        public TeleportationCell ExitGem;
        private bool _hasBeenUsed;
        private bool _portalIsOpened;

        public TeleportationCell(int boardX, int boardY, Board board, string portalId) : base(boardX, boardY, board)
        {
            PortalId = portalId;
            _portalIsOpened = true;
            _hasBeenUsed = false;
            FindExit(board);

            float radius = board.GetGemSize();
            Gem gem = new Gem(boardX, boardY, 5, board.ToGemX(boardX, radius), board.ToGemY(boardY, radius), radius, new Random(), board);
            board.AddChild(gem);
        }

        public void FindExit(Board board)
        {
            ExitGem = (TeleportationCell)board.CellsList.FirstOrDefault(p => p is TeleportationCell && ((TeleportationCell)p).PortalId == PortalId);
            if (ExitGem != null)
            {
                ExitGem.ExitGem = this;
            }
        }

        public override ICell GetTargetCell(Direction direction)
        {
            if (_portalIsOpened)
            {
                ExitGem.ClosePortal();
                return ExitGem;
            }
            else
            {
                OpenPortal();
                return base.GetTargetCell(direction);
            }
        }

        public bool IsOpened()
        {
            return _portalIsOpened;
        }

        public void OpenPortal()
        {
            _portalIsOpened = true;
        }

        public void ClosePortal()
        {
            _portalIsOpened = false;
        }

        public void SwitchPortalState()
        {
            _portalIsOpened = !_portalIsOpened;
        }

        public override Task TryReceiveGem(IGem gem, Direction direction, ICell senderCell)
        {
            if (AttachedGem == null)
            {
                senderCell.DetachGemBase();
                senderCell.Reinitialize();

                AttachGem(gem);
                Reinitialize();
                return gem.PerformAction(gem.Move(IndexX, IndexY, true));
            }
            else
            {
                if (gem.CanCollide(AttachedGem))
                {
                    senderCell.DetachGemBase();
                    senderCell.Reinitialize();
                    Reinitialize();
                    gem.Move(IndexX, IndexY, true);
                    return gem.CollideInto(AttachedGem);
                }
                else
                {
                    return ReturnGem(gem, senderCell);
                }
            }
        }
    }
}
