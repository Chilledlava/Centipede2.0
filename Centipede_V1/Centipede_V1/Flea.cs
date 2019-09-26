using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Centipede_V1
{
    class Flea
    {
        public Flea (int top, int left, Image pic)
        {
            locationX = top;
            locationY = left;
            mainImage = pic;
           
        }
        public Flea()
        {

        }
        public double locationX { get; }
        public double locationY { get; }
        public Image mainImage { get; }
        public static bool checkCollision(Flea flea,Image shot)
        {
            bool hit = false;
            if (shot != null)
            {
                if (shot.Margin.Left >= flea.locationX && flea.locationX <= (shot.Margin.Left + 16))
                {
                    if (shot.Margin.Top <= flea.locationY && (flea.locationY + 5) <= (shot.Margin.Top + 16))
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
