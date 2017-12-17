using GemSwipe.Paladin.Core;
using GemSwipe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace GemSwipe.Game.Models.Entities
//{
//    public abstract class GemBase2 : SkiaView, IGem2
//    {
//        protected const int MovementAnimationMs = 600;
//        public int IndexX2 { get; set; }
//        public int IndexY2 { get; set; }
//        public ICell2 AnchorCell2;
//        private bool _hasBeenHandled2;
//        private bool _isPerformingAction2=false;
//        protected Board _board2;
 
//        public GemBase2(int boardX, int boardY, int size, Board board) : base(0, 0, 0, 0)
//        {
//            IndexX2 = boardX;
//            IndexY2 = boardY;
//            _board2 = board;
//            AnchorCell2 = _board2.CellsList.SingleOrDefault(p => p.IndexX2 == IndexX2 && p.IndexY2 == IndexY2);
//        }

//        public GemBase2(int boardX, int boardY, int size, float x, float y, float radius, Random randomizer, Board board) : base(x, y, radius * 2, radius * 2)
//        {
//            IndexX2 = boardX;
//            IndexY2 = boardY;
//            _board2 = board;
//            AnchorCell2 = _board2.CellsList.SingleOrDefault(p => p.IndexX2 == IndexX2 && p.IndexY2 == IndexY2);
//        }


//        public Task TryResolveSwipe2(Direction direction)
//        {
//            Logger.Log("TryResolveSwipe");
//            if (CanActivate2())
//            {
//                return ResolveSwipe2(direction);
//            }
//            else
//            {
//                return Task.Delay(0);
//            }
//        }

//        public void Reinitialize2()
//        {
//            _hasBeenHandled2 = false;
//        }

//        public bool CanActivate2()
//        {
//            return !HasBeenHandled2() && CanPerform2();
//        }

//        public bool HasBeenHandled2()
//        {
//            return _hasBeenHandled2;
//        }

//        public bool CanPerform2()
//        {
//            return !_isPerformingAction2;
//        }

//        public void ValidateHandling2()
//        {
//            _hasBeenHandled2 = true;
//        }

//        public virtual Task ResolveSwipe2(Direction direction)
//        {
//            if (AnchorCell2.MustActivate2(this))
//            {
//                return AnchorCell2.Activate2(this);
//            }
//            else
//            {
//                return Activate2(direction);
//            }
//        }

//        public virtual Task Activate2(Direction direction)
//        {
//            ICell2 targetCell = GetTargetCell2(direction);
//            if (targetCell == null)
//            {
//                ValidateHandling2();
//                return Task.Delay(0);
//            }
//            else
//            {
//                if (targetCell.CanHandle2(this))
//                {
//                    return targetCell.Handle2(this);
//                }
//                else
//                {
//                    return Task.Delay(0);
//                }
//            }
//        }

//        public virtual ICell2 GetTargetCell2(Direction direction)
//        {
//            int targetX = -1;
//            int targetY = -1;

//            bool targetIsNull = true;
//            switch (direction)
//            {
//                case Direction.Top:
//                    targetX = IndexX2;
//                    targetY = IndexY2 - 1;
//                    if (targetY >= 0)
//                    {
//                        targetIsNull = false;
//                    }
//                    break;
//                case Direction.Bottom:
//                    targetX = IndexX2;
//                    targetY = IndexY2 + 1;
//                    if (targetY <= _board2.NbOfRows - 1)
//                    {
//                        targetIsNull = false;
//                    }
//                    break;
//                case Direction.Left:
//                    targetX = IndexX2 - 1;
//                    targetY = IndexY2;
//                    if (targetX >= 0)
//                    {
//                        targetIsNull = false;
//                    }
//                    break;
//                case Direction.Right:
//                    targetX = IndexX2 + 1;
//                    targetY = IndexY2;
//                    if (targetX <= _board2.NbOfColumns - 1)
//                    {
//                        targetIsNull = false;
//                    }
//                    break;
//            }
//            if (targetIsNull)
//            {
//                return null;
//            }
//            else
//            {
//                return _board2.CellsList.SingleOrDefault(p => p.IndexX2 == targetX && p.IndexY2 == targetY);
//            }
//        }

      
//        public virtual bool CanCollide2(IGem2 gem2)
//        {
//            return false;
//        }

//        public virtual Task Collide2(IGem2 gem2)
//        {
//            return Task.Delay(0);
//        }

//        public Task GoTo2(ICell2 cell2)
//        {
//            IndexX2 = cell2.IndexX2;
//            IndexY2 = cell2.IndexY2;
//            AnchorCell2 = cell2;
//            return Move(cell2.IndexX2, cell2.IndexY2,true);
//        }

//        public virtual Task Move(int x, int y, bool animator = false)
//        {
//            return Task.Delay(0);
//        }
//    }
//}
