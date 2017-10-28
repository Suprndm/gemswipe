using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Data.PlayerLife
{
    public class PlayerLife
    {
        public int Count { get; set; }
        private int _maxCount;
        private int _minCount;


        public PlayerLife()
        {
            _maxCount = 3;
            _minCount = 0;
            Count = _maxCount;
        }

        public bool HasLife()
        {
            return Count > _minCount;
        }

        public void Increment()
        {
            if (Count < _maxCount)
            {
                Count++;
            }
        }

        public void Decrement()
        {
            if (Count > _minCount)
            {
                Count--;
            }
        }
    }
}
