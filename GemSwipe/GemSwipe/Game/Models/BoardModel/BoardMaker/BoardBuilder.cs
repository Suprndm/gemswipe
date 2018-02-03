using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Models.BoardModel.BoardMaker
{
    public class BoardBuilder
    {

        private int[,] _boardModel;
        private string _baseSetupString;
        private Random _randomizer;

        public BoardBuilder()
        {
            _randomizer = new Random();
        }

        public int[,] GetModel()
        {
            return _boardModel;
        }

        public int[,] StringToModel(BoardSetup boardSetup)
        {
            var boardString = boardSetup.SetupString;
            var rows = boardString.Split('-');
            var nbOfRows = rows.Length;
            var nbOfColumns = rows[0].Split(' ').Length;

            _boardModel = new int[nbOfRows, nbOfColumns];

            for (int i = 0; i < nbOfRows; i++)
            {
                var rowCells = rows[i].Split(' ');
                for (int j = 0; j < nbOfColumns; j++)
                {
                    if (rowCells[j] == "BL")
                    {
                        _boardModel[i, j] = -1;
                    }
                    else
                    {
                        int gemValue;
                        int.TryParse(rowCells[j], out gemValue);
                        _boardModel[i, j] = gemValue;
                    }
                }
            }

            return _boardModel;
        }

        public string ModelToString()
        {
            string boardSetupString = "";

            for (int i = 0; i <= _boardModel.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= _boardModel.GetUpperBound(1); j++)
                {
                    string cellValue = _boardModel[i, j].ToString();
                    if (_boardModel[i, j] == -1)
                    {
                        cellValue = "BL";
                    }
                    boardSetupString += cellValue;
                    if (j == _boardModel.GetUpperBound(1))
                    {
                        break;
                    }
                    boardSetupString += " ";
                }
                if (i == _boardModel.GetUpperBound(0))
                {
                    break;
                }
                boardSetupString += "-";
            }
            return boardSetupString;
        }


        public void ReverseSwipe(Direction direction)
        {
            switch (direction)
            {
                case Direction.Bottom:
                    for (int j = 0; j <= _boardModel.GetUpperBound(1); j++)
                    {
                        for (int i = _boardModel.GetUpperBound(0); i >= 0; i--)
                        {
                            ReverseSwipeGem(i, j, direction);
                        }
                    }


                    break;
                case Direction.Left:
                    for (int i = 0; i <= _boardModel.GetUpperBound(0); i++)
                    {
                        for (int j = 0; j <= _boardModel.GetUpperBound(1); j++)
                        {
                            ReverseSwipeGem(i, j, direction);
                        }
                    }

                    break;
                case Direction.Right:
                    for (int i = 0; i <= _boardModel.GetUpperBound(0); i++)
                    {
                        for (int j = _boardModel.GetUpperBound(1); j >= 0; j--)
                        {
                            ReverseSwipeGem(i, j, direction);
                        }
                    }

                    break;
                case Direction.Top:
                    for (int j = 0; j <= _boardModel.GetUpperBound(1); j++)
                    {
                        for (int i = 0; i <= _boardModel.GetUpperBound(0); i++)
                        {
                            ReverseSwipeGem(i, j, direction);
                        }
                    }
                    break;
            }
        }

        public void ReverseSwipeGem(int indexY, int indexX, Direction direction)
        {
            if ((_boardModel[indexY, indexX] != 0)&&(_boardModel[indexY, indexX]!=-1))
            {
                if (_boardModel[indexY, indexX] == 1)
                {
                    ReturnGem(indexY, indexX, direction);
                }
                else
                {
                    SplitGem(indexY, indexX, direction);
                }
            }
        }

        public void SplitGem(int indexY, int indexX, Direction direction)
        {
            int originalValue = _boardModel[indexY, indexX];
            if (originalValue >= 2)
            {
                int pasX = 0;
                int pasY = 0;
                int i = indexY;
                int j = indexX;
                switch (direction)
                {
                    case Direction.Bottom:
                        pasY = 1;
                        break;
                    case Direction.Left:
                        pasX = -1;
                        break;
                    case Direction.Right:
                        pasX = 1;
                        break;
                    case Direction.Top:
                        pasY = -1;
                        break;
                }

                if (direction == Direction.Bottom)
                {
                    while (i + pasY <= _boardModel.GetUpperBound(0))
                    {
                        if (_boardModel[i + pasY, j] == -1)
                        {
                            break;
                        }
                        else if (_boardModel[i + pasY, j] > 0)
                        {
                            break;
                        }
                        else if (_boardModel[i + pasY, j] == 0)
                        {
                            i += pasY;
                        }
                    }
                    if (i > indexY)
                    {
                        _boardModel[indexY, indexX] = 0;
                        _boardModel[i, j] = originalValue - 1;
                        _boardModel[i - pasY, j] = originalValue - 1;
                    }
                }

                if (direction == Direction.Top)
                {
                    while (i + pasY >= 0)
                    {
                        if (_boardModel[i + pasY, j] == -1)
                        {
                            break;
                        }
                        else if (_boardModel[i + pasY, j] > 0)
                        {
                            break;
                        }
                        else if (_boardModel[i + pasY, j] == 0)
                        {
                            i += pasY;
                        }
                    }
                    if (i < indexY)
                    {
                        _boardModel[indexY, indexX] = 0;
                        _boardModel[i, j] = originalValue - 1;
                        _boardModel[i - pasY, j] = originalValue - 1;
                    }
                }

                if (direction == Direction.Right)
                {
                    while (j + pasX <= _boardModel.GetUpperBound(1))
                    {
                        if (_boardModel[i, j + pasX] == -1)
                        {
                            break;
                        }
                        else if (_boardModel[i, j + pasX] > 0)
                        {
                            break;
                        }
                        else if (_boardModel[i, j + pasX] == 0)
                        {
                            j += pasX;
                        }
                    }
                    if (j > indexX)
                    {
                        _boardModel[indexY, indexX] = 0;
                        _boardModel[i, j] = originalValue - 1;
                        _boardModel[i, j - pasX] = originalValue - 1;
                    }
                }

                if (direction == Direction.Left)
                {
                    while (j + pasX >= 0)
                    {
                        if (_boardModel[i, j + pasX] == -1)
                        {
                            break;
                        }
                        else if (_boardModel[i, j + pasX] > 0)
                        {
                            break;
                        }
                        else if (_boardModel[i, j + pasX] == 0)
                        {
                            j += pasX;
                        }
                    }
                    if (j < indexX)
                    {
                        _boardModel[indexY, indexX] = 0;
                        _boardModel[i, j] = originalValue - 1;
                        _boardModel[i, j - pasX] = originalValue - 1;
                    }
                }
            }
        }

        public void ReturnGem(int indexY, int indexX, Direction direction)
        {
            int originalValue = _boardModel[indexY, indexX];
            int pasX = 0;
            int pasY = 0;
            int i = indexY;
            int j = indexX;
            switch (direction)
            {
                case Direction.Bottom:
                    pasY = 1;
                    break;
                case Direction.Left:
                    pasX = -1;
                    break;
                case Direction.Right:
                    pasX = 1;
                    break;
                case Direction.Top:
                    pasY = -1;
                    break;
            }

            if (direction == Direction.Bottom)
            {
                while (i + pasY <= _boardModel.GetUpperBound(0))
                {
                    if (_boardModel[i + pasY, j] == -1)
                    {
                        break;
                    }
                    else if (_boardModel[i + pasY, j] > 0)
                    {
                        break;
                    }
                    else if (_boardModel[i + pasY, j] == 0)
                    {
                        i += pasY;
                    }
                }
                if (i > indexY)
                {
                    _boardModel[indexY, indexX] = 0;
                    _boardModel[i, j] = originalValue;
                }
            }

            if (direction == Direction.Top)
            {
                while (i + pasY >= 0)
                {
                    if (_boardModel[i + pasY, j] == -1)
                    {
                        break;
                    }
                    else if (_boardModel[i + pasY, j] > 0)
                    {
                        break;
                    }
                    else if (_boardModel[i + pasY, j] == 0)
                    {
                        i += pasY;
                    }
                }
                if (i < indexY)
                {
                    _boardModel[indexY, indexX] = 0;
                    _boardModel[i, j] = originalValue;
                }
            }

            if (direction == Direction.Right)
            {
                while (j + pasX <= _boardModel.GetUpperBound(1))
                {
                    if (_boardModel[i, j + pasX] == -1)
                    {
                        break;
                    }
                    else if (_boardModel[i, j + pasX] > 0)
                    {
                        break;
                    }
                    else if (_boardModel[i, j + pasX] == 0)
                    {
                        j += pasX;
                    }
                }
                if (j > indexX)
                {
                    _boardModel[indexY, indexX] = 0;
                    _boardModel[i, j] = originalValue;
                }
            }

            if (direction == Direction.Left)
            {
                while (j + pasX >= 0)
                {
                    if (_boardModel[i, j + pasX] == -1)
                    {
                        break;
                    }
                    else if (_boardModel[i, j + pasX] > 0)
                    {
                        break;
                    }
                    else if (_boardModel[i, j + pasX] == 0)
                    {
                        j += pasX;
                    }
                }
                if (j < indexX)
                {
                    _boardModel[indexY, indexX] = 0;
                    _boardModel[i, j] = originalValue;
                }
            }
        }

        public void AddInitialShift(int indexY, int indexX, Direction direction)
        {

        }
    }
}
