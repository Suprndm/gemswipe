using GemSwipe.Game.Models.BoardModel.Gems;
using GemSwipe.Game.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GemSwipe.Game.Models.BoardModel.Cells
{
    public class TeleportationCell : Cell
    {
        public string PortalId { get; set; }
        public TeleportationCell ExitCell;
        public TeleportationCell EntryCell;
        private IList<IGem> _alreadyTransportedGem;
        private bool _isEntry;
        private Gem _gem;
        private BlackholeGem _blackholeGem;
        private bool _isTeleporting;


        public TeleportationCell(int boardX, int boardY, Board board, string portalId, bool isEntry = true) : base(boardX, boardY, board)
        {
            _isTeleporting = false;
            PortalId = portalId;
            _isEntry = isEntry;
            _alreadyTransportedGem = new List<IGem>();
            if (isEntry)
            {
                SetOtherPortal(board);
            }
            float radius = board.GetGemSize();
            _gem = new Gem(boardX, boardY, 5, board.ToGemX(boardX), board.ToGemY(boardY), radius, new Random(), board);
            board.AddChild(_gem);

            radius = board.GetGemSize();
            _blackholeGem = new BlackholeGem(boardX, boardY, 0, board.ToGemX(boardX), board.ToGemY(boardY), radius, new Random(), board);
            board.AddChild(_blackholeGem);
        }

        public TeleportationCell FindOtherPortal(Board board)
        {
            return (TeleportationCell)board.CellsList.FirstOrDefault(p => p is TeleportationCell && ((TeleportationCell)p).PortalId == PortalId);
        }

        public void SetOtherPortal(Board board)
        {
            if (_isEntry)
            {
                ExitCell = FindOtherPortal(board);
                if (ExitCell != null)
                {
                    ExitCell.EntryCell = this;
                }
            }
            else
            {
                EntryCell = FindOtherPortal(board);
                if (EntryCell != null)
                {
                    EntryCell.ExitCell = this;
                }
            }
        }

        public void FindExit(Board board)
        {
            ExitCell = (TeleportationCell)board.CellsList.FirstOrDefault(p => p is TeleportationCell && ((TeleportationCell)p).PortalId == PortalId);
            if (ExitCell != null)
            {
                ExitCell.EntryCell = this;
            }
        }

        public void FindEntry(Board board)
        {
            EntryCell = (TeleportationCell)board.CellsList.FirstOrDefault(p => p is TeleportationCell && ((TeleportationCell)p).PortalId == PortalId);
            if (EntryCell != null)
            {
                EntryCell.ExitCell = this;
            }
        }

        //public override ICell GetTargetCell(Direction direction)
        //{
        //    if (_isEntry)
        //    {
        //        return ExitCell;
        //    }
        //    else
        //    {
        //        return base.GetTargetCell(direction);
        //    }
        //}

        //public override void Reinitialize()
        //{
        //    _alreadyTransportedGem = new List<IGem>();
        //    base.Reinitialize();
        //}

        //public override Task PickGem(IGem gem)
        //{
        //    if (_isEntry)
        //    {
        //        return gem.PerformAction(() => gem.Move(IndexX, IndexY, true));
        //    }
        //    else
        //    {
        //        return base.PickGem(gem);
        //    }
        //}

        ////public override Task PickGem(IGem gem)
        ////{
        ////    if (_isEntry)
        ////    {
        ////        return gem.PerformAction(() => gem.Move(IndexX, IndexY, true));
        ////    }
        ////    else
        ////    {
        ////        if (senderCell == EntryCell)
        ////        {

        ////        }
        ////    }
        ////}

        //public override bool CanProcess(IGem gem)
        //{
        //    return !_isTeleporting && base.CanProcess(gem);
        //}

        //public override Task TryReceiveGem(IGem gem, Direction direction, ICell senderCell)
        //{
        //    if (_isEntry)
        //    {
        //        if (_alreadyTransportedGem.SingleOrDefault(p => p == gem) == null)
        //        {
        //            _alreadyTransportedGem.Add(gem);
        //            return base.TryReceiveGem(gem, direction, senderCell);
        //        }
        //        else
        //        {
        //            return ReturnGem(gem, senderCell);
        //        }
        //    }
        //    else
        //    {
        //        if (senderCell == EntryCell)
        //        {
        //            return TryTeleport(gem, direction, senderCell);
        //        }
        //        else
        //        {
        //            return base.TryReceiveGem(gem, direction, senderCell);
        //        }
        //    }
        //}

        //public Task TryTeleport(IGem gem, Direction direction, ICell senderCell)
        //{
        //    if (AssignedGem == null)
        //    {
        //        senderCell.DetachGemBase();
        //        senderCell.Reactivate();

        //        Assign(gem);
        //        Reactivate();
        //        return Teleport(gem);
        //    }
        //    else
        //    {
        //        if (gem.CanCollide(AssignedGem))
        //        {
        //            senderCell.DetachGemBase();
        //            senderCell.Reactivate();

        //            Reactivate();
        //            Teleport(gem);
        //            return HandleCollisionWithAttachedGem(gem);
        //        }
        //        else
        //        {
        //            return ReturnGem(gem, senderCell);
        //        }
        //    }
        //}

        public Task IsTeleporting(bool isTeleporting)
        {
             _isTeleporting = isTeleporting;
            return Task.Delay(0);
        }

        public Task Teleport(IGem gem)
        {

            return gem.PerformAction(() => EntryCell.IsTeleporting(true), ()=>Miniaturize((GemBase)gem), () => gem.Move(IndexX, IndexY), () => IncreaseSize((GemBase)gem),()=>EntryCell.IsTeleporting(false));
        }

        public Task IncreaseSize(GemBase gem)
        {
            int animationLenght = 600;
            Gem gemToTeleport = (gem as Gem);
            gemToTeleport.Animate("IncreaseGemSize", p => gemToTeleport.Radius = (float)p, gemToTeleport.Radius, gemToTeleport.Radius * 2, 4, (uint)animationLenght, Easing.SinInOut);
            //gemToTeleport.Animate("MiniaturizeHeight", p => gemToTeleport.Height = (float)p, gemToTeleport.Height, gemToTeleport.Height / 2, 4, (uint)animationLenght, Easing.SinInOut);
            return Task.Delay(animationLenght);
        }

        public Task Miniaturize(GemBase gem)
        {
            int animationLenght = 600;
            Gem gemToTeleport = (gem as Gem);
            gemToTeleport.Animate("MiniaturizeWidth", p => gemToTeleport.Radius = (float)p, gemToTeleport.Radius, gemToTeleport.Radius / 2, 4, (uint)animationLenght, Easing.SinInOut);
            //gemToTeleport.Animate("MiniaturizeHeight", p => gemToTeleport.Height = (float)p, gemToTeleport.Height, gemToTeleport.Height / 2, 4, (uint)animationLenght, Easing.SinInOut);
            return Task.Delay(animationLenght);
        }
    }
}
