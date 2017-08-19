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

            return new BoardSetup(4, 4, boardString, moveCount);
        }
    }
}
