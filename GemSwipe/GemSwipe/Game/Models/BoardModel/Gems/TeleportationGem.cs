using GemSwipe.Game.Models.Entities;
using GemSwipe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.BoardModel.Gems
{
    public class TeleportationGem : Gem
    {
        public string PortalId { get; set; }
        public TeleportationGem ExitGem;
        public IList<Gem> GemToTeleport;

        private Board _board;
        private IList<Cell> _topLane;
        private IList<Cell> _bottomLane;
        private IList<Cell> _leftLane;
        private IList<Cell> _rightLane;

        private bool _hasBeenUsed;



        public TeleportationGem(int boardX, int boardY, int size, Board board) : base(boardX, boardY, size,board)
        {
            Type = GemType.Teleportation;
            _board = board;

            _topLane = new List<Cell>();
            _bottomLane = new List<Cell>();
            _leftLane = new List<Cell>();
            _rightLane = new List<Cell>();

            GemToTeleport = new List<Gem>();
            _hasBeenUsed = false;
        }

        public TeleportationGem(Board board, string portalId, int boardX, int boardY, int size, float x, float y, float radius, Random randomizer) : base(boardX, boardY, size, x, y, radius, randomizer,board)
        {
            Type = GemType.Teleportation;
            PortalId = portalId;
            _board = board;

            _topLane = new List<Cell>();
            _bottomLane = new List<Cell>();
            _leftLane = new List<Cell>();
            _rightLane = new List<Cell>();

            GemToTeleport = new List<Gem>();
            _hasBeenUsed = false;
        }

        public void FindExit()
        {
            //ExitGem = _board.TeleportationGems.FirstOrDefault(p => p.PortalId == PortalId);
            ExitGem.ExitGem = this;
        }

        public void BuildExitLanes()
        {
            //top lane

            int boardMinIndexY = 0;
            if (IndexY > boardMinIndexY)
            {
                int indexY = IndexY - 1;
                Cell cell = _board.Cells[IndexX, indexY];

                while (!cell.IsBlocked && indexY >= boardMinIndexY)
                {
                    cell = _board.Cells[IndexX, indexY];
                    _topLane.Add(cell);

                    indexY--;
                }
            }

            //bottom lane

            int boardMaxIndexY = _board.NbOfRows - 1;
            if (IndexY < boardMaxIndexY)
            {
                int indexY = IndexY + 1;
                Cell cell = _board.Cells[IndexX, indexY];
                while (!cell.IsBlocked && indexY <= boardMaxIndexY)
                {
                    cell = _board.Cells[IndexX, indexY];
                    _bottomLane.Add(cell);

                    indexY++;

                }
            }

            //left lane

            int boardMinIndexX = 0;
            if (IndexX > boardMinIndexX)
            {
                int indexX = IndexX - 1;
                Cell cell = _board.Cells[indexX, IndexY];

                while (!cell.IsBlocked && indexX >= boardMinIndexX)
                {
                    cell = _board.Cells[indexX, IndexY];
                    _leftLane.Add(cell);

                    indexX--;
                }
            }

            //right lane

            int boardMaxIndexX = _board.NbOfColumns - 1;
            if (IndexX < boardMaxIndexX)
            {
                int indexX = IndexX + 1;
                Cell cell = _board.Cells[indexX, IndexY];

                while (!cell.IsBlocked && indexX <= boardMaxIndexX)
                {
                    cell = _board.Cells[indexX, IndexY];
                    _rightLane.Add(cell);

                    indexX++;
                }
            }
        }

        //public override void GoAlongLane(IList<Cell> cellsLane, Direction direction, SwipeResult swipeResult)
        //{
        //    _board.Cells[BoardX, BoardY].AttachGem(this);
        //}

        public void SetExitGem(TeleportationGem exitGem)
        {
            ExitGem = exitGem;
        }

        public bool ExitHasSpace(Direction direction)
        {
            return ExitGem.LaneHasSpace(direction);
        }

        public bool LaneHasSpace(Direction direction)
        {
            switch (direction)
            {
                default:
                    return false;
                case Direction.Top:
                    if (_topLane.Count > 0)
                    {
                        return _topLane[0].IsEmpty();
                    }
                    else
                    {
                        return false;
                    }
                case Direction.Bottom:
                    if (_bottomLane.Count > 0)
                    {
                        return _bottomLane[0].IsEmpty();
                    }
                    else
                    {
                        return false;
                    }
                case Direction.Left:
                    if (_leftLane.Count > 0)
                    {
                        return _leftLane[0].IsEmpty();
                    }
                    else
                    {
                        return false;
                    }
                case Direction.Right:
                    if (_rightLane.Count > 0)
                    {
                        return _rightLane[0].IsEmpty();
                    }
                    else
                    {
                        return false;
                    }
            }
        }

        //public void SendGemToLane(Gem gem, Direction direction, SwipeResult swipeResult)
        //{
        //    IList<Cell> cellsLane = new List<Cell>();
        //    switch (direction)
        //    {
        //        case Direction.Top:
        //            cellsLane = SelectFreeCells(_topLane);
        //            break;
        //        case Direction.Bottom:
        //            cellsLane = SelectFreeCells(_bottomLane);
        //            break;
        //        case Direction.Left:
        //            cellsLane = SelectFreeCells(_leftLane);
        //            break;
        //        case Direction.Right:
        //            cellsLane = SelectFreeCells(_rightLane);
        //            break;
        //    }
        //    gem.GoAlongLane(cellsLane, direction, swipeResult);
        //    var movedGem = swipeResult.MovedGems;
        //    movedGem.Remove(gem);
        //}

        private IList<Cell> SelectFreeCells(IList<Cell> cellLane)
        {
            IList<Cell> freeLane = new List<Cell>();
            if (cellLane.Count > 0)
            {
                int i = 0;
                while (i < cellLane.Count)
                {
                    if (cellLane[i].IsEmpty())
                    {
                        freeLane.Add(cellLane[i]);
                        i++;
                    }
                    else
                    {
                        freeLane.Add(cellLane[i]);
                        break;
                    }
                }
                freeLane = freeLane.OrderByDescending(p => freeLane.IndexOf(p)).ToList();
            }
            return freeLane;
        }

        //public async void ReceiveGem(Gem gem, Direction direction, SwipeResult swipeResult)
        //{
        //        _exitGem.SendGemToLane(gem, direction, swipeResult);
        //}

        //public void Teleport(Gem gem, Direction direction, SwipeResult swipeResult)
        //{
        //    _hasBeenUsed = true;
        //    GemToTeleport.Add(gem);
        //    ExitGem.SendGemToLane(gem, direction, swipeResult);
        //}

        //public async void TeleportToThenFrom(float x, float y, float exitX, float exitY, float targetX, float targetY)
        //{
        //    await GemToTeleport.MoveTo(x, y);
        //    await GemToTeleport.MoveTo(exitX, exitY);
        //    await GemToTeleport.MoveTo(targetX, targetY);
        //}

        public bool CanTeleport(Gem gem, Direction direction)
        {
            return /*!_hasBeenUsed &&*/ ExitHasSpace(direction) && !GemToTeleport.Contains(gem);
        }

        public void Reinitialize()
        {
            GemToTeleport = new List<Gem>();
            _hasBeenUsed = false;
        }
    }
}
