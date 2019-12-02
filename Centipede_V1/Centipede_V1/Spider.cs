using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Centipede_V1
{
    class Spider
    {
        public Spider(int top, int left, Image pic)
        {
            locationX = top;
            locationY = left;
            mainImage = pic;
            hp = 1;

        }
        public Spider()
        {

        }

        public double locationX { get; set; }
        public double locationY { get; set; }
        public Image mainImage { get; }
        public int hp { get; private set; }
        public int points = 600;

        public bool checkCollision(Image shot)
        {
            bool hit = false;
            if (shot != null)
            {
                if (shot.Margin.Left >= locationX && locationX <= (shot.Margin.Left + 16))
                {
                    if (shot.Margin.Top <= locationY && (locationY + 5) <= (shot.Margin.Top + 16))
                    {
                        hp -= 1;

                        hit = true;

                    }
                }
            }
            return hit;
        }
    }

}
