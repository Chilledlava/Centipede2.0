using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Centipede_V1
{
    public class Spider
    {
        public Spider(int top, int left, Image pic)
        {
            locationX = top;
            locationY = left;
            mainImage = pic;

        }
        public Spider()
        {

        }
        public double locationX { get; }
        public double locationY { get; }
        public Image mainImage { get; }
        public static bool checkCollision(Spider spider, Image shot)
        {
            bool hit = false;
            if (shot != null)
            {
                if (shot.Margin.Left >= spider.locationX && spider.locationX <= (shot.Margin.Left + 16))
                {
                    if (shot.Margin.Top <= spider.locationY && (spider.locationY + 5) <= (shot.Margin.Top + 16))
                    {
                        //😍 Valid Code.

                        hit = true;

                    }
                }
            }
            return hit;
        }
    }
}

