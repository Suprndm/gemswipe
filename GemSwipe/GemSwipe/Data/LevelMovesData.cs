using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace GemSwipe.Data
{
    public static class LevelMovesData
    {
        public static IDictionary<int, List<int>> GetlevelMovesMap()
        {
            var map = new Dictionary<int, List<int>>();

            map.Add(1, new List<int> { 2, 3 });
            map.Add(2, new List<int> { 3, 4 });
            map.Add(3, new List<int> { 4, 5 });

            map.Add(4, new List<int> { 4, 5, 6 });
            map.Add(5, new List<int> { 5, 6, 7 });
            map.Add(6, new List<int> { 6, 7, 8 });
            map.Add(7, new List<int> { 7, 8, 9 });
            map.Add(8, new List<int> { 8, 9, 10 });
            map.Add(9, new List<int> { 9, 10, 11 });
            map.Add(10, new List<int> { 9, 10, 11, 12, 13, 14, 15, 16, 17 });


            return map;
        }
    }
}
