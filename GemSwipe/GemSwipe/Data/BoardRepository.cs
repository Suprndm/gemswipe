using System;
using System.Collections.Generic;
using GemSwipe.Models;

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
            return new BoardSetup(6, 6, boardString, moveCount);
        }
    }
}
