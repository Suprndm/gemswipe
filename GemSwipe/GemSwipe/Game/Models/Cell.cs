using GemSwipe.Game.Models.BoardModel;
using GemSwipe.Game.Models.Entities;
using System;
using System.Collections.Generic;

namespace GemSwipe.Game.Models
{
    public class Cell : CellBase
    {

        public int X { get; }
        public int Y { get; }

        public bool IsBlocked { get; set; }
        public CellModifier Modifier { get; set; }


        public Cell(int x, int y, Board board, bool isBlocked = false) : base(x, y, board)
        {
            X = x;
            Y = y;
            IsBlocked = isBlocked;
        }

        public Cell(int x, int y, GemType gemType, Board board) : base(x, y, board)
        {
            X = x;
            Y = y;
            SetModifier(gemType);
        }

        public Cell(int x, int y, Gem gem, Board board) : base(x, y, board)
        {
            X = x;
            Y = y;
            if (gem != null)
            {
                SetModifier(gem.Type);
            }
        }

        public void SetModifier(GemType gemType)
        {
            switch (gemType)
            {
                default:
                    IsBlocked = false;
                    Modifier = CellModifier.Base;
                    break;
                case GemType.Blocking:
                    IsBlocked = true;
                    Modifier = CellModifier.Blocked;
                    break;
                case GemType.Teleportation:
                    IsBlocked = true;
                    Modifier = CellModifier.Teleporter;
                    break;
            }
        }


        public bool IsEmpty()
        {
            return AttachedGem == null && !IsBlocked;
        }



        public Gem GetAttachedGem()
        {
            return (Gem)AttachedGem;
        }


    }
    //    public int X { get; }
    //    public int Y { get; }

    //    public bool IsBlocked { get; set; }
    //    public CellModifier Modifier { get; set; }


    //    public Cell(int x, int y, Board board, bool isBlocked = false) : base(x,y,board)
    //    {
    //        X = x;
    //        Y = y;
    //        IsBlocked = isBlocked;
    //    }

    //    public Cell(int x, int y, GemType gemType,Board board) : base(x,y,board)
    //    {
    //        X = x;
    //        Y = y;
    //        SetModifier(gemType);
    //    }

    //    public Cell(int x, int y, Gem gem, Board board) : base(x,y,board)
    //    {
    //        X = x;
    //        Y = y;
    //        if (gem != null)
    //        {
    //            SetModifier(gem.Type);
    //        }
    //    }

    //    public void SetModifier(GemType gemType)
    //    {
    //        switch (gemType)
    //        {
    //            default:
    //                IsBlocked = false;
    //                Modifier = CellModifier.Base;
    //                break;
    //            case GemType.Blocking:
    //                IsBlocked = true;
    //                Modifier = CellModifier.Blocked;
    //                break;
    //            case GemType.Teleportation:
    //                IsBlocked = true;
    //                Modifier = CellModifier.Teleporter;
    //                break;
    //        }
    //    }


    //    public bool IsEmpty()
    //    {
    //        return AttachedGem == null && !IsBlocked;
    //    }



    //    public Gem GetAttachedGem()
    //    {
    //        return (Gem)AttachedGem;
    //    }


    //}
}
