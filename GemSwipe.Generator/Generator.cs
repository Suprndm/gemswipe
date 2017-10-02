using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Generator
{
    public  class Generator
    {
        private readonly Random _random;

        public Generator()
        {
            _random = new Random();
        }

        public string GenerateRandomLevel(int width, int height)
        {
            // Generate
           int[,] board = new int[width,height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int gem = 0;
                    var randomNumber = _random.Next(100);
                    if (randomNumber < 65) gem = 0;
                    else if (randomNumber < 85) gem = 1;
                    else if (randomNumber < 95) gem = 2;
                    else if (randomNumber < 98) gem = 3;
                    else gem = 4;

                    board[i, j] = gem;
                }
            }

            string draw = "";
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    draw += "";
                    var gem = board[i, j];
                    draw += gem;
                    draw += " ";
                }
                draw = draw.Substring(0, draw.Length - 1);
                draw += "-";
            }
            draw = draw.Substring(0, draw.Length - 1);
            return draw;
        }
    }
}
