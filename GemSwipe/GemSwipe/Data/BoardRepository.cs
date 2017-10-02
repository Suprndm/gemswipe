using System;
using System.Collections.Generic;
using GemSwipe.Game.Models;

namespace GemSwipe.Data
{
    public class BoardRepository
    {
        private IDictionary<int, List<string>> _boardData;
        private IDictionary<int, List<int>> _levelMovesMap;
        private Random _randomizer;
        public BoardRepository()
        {
            _randomizer = new Random();
            _boardData = BoardData.GetBoardData();
            _levelMovesMap = LevelMovesData.GetlevelMovesMap();

        }

        public BoardSetup GetRandomBoardSetup(int level)
        {
            var moves = _levelMovesMap[level];

            var moveCount = moves[_randomizer.Next(moves.Count)];
            var boardSection = _boardData[moveCount];

            var boardString = boardSection[_randomizer.Next(boardSection.Count)];
            boardString = "0 9 0 0 0 0-2 0 0 0 0 0-9 0 0 3 0 0-2 0 0 0 0 0-0 0 0 0 0 0-0 0 0 0 0 0";
            return new BoardSetup(level, 6, 6, boardString, moveCount);
        }

        public BoardSetup GetBoard(int levelId)
        {
            string boardString;
            if (levelId == 1)
            {
                boardString = "1 1 0 0-0 0 1 1-0 0 1 0-0 1 2 0";
                return new BoardSetup(levelId,4, 4, boardString, 4);
            }
            else if (levelId == 2)
            {
                boardString = "0 0 3 0-1 2 0 1-0 0 0 1-0 2 1 3";
                return new BoardSetup(levelId, 4, 4, boardString, 7);
            }
            else if (levelId == 3)
            {
                boardString = "2 0 1 0-3 1 0 0-1 3 0 0-1 1 1 0";
                return new BoardSetup(levelId, 4, 4, boardString, 15);
            }
            else
            {
                throw new ArgumentException($"unknown level Id {levelId}");
            }
        }
    }
}
