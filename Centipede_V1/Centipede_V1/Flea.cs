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
            locationX = left;
            locationY = top;
            mainImage = pic;
            hp = 2;
        }
        public Flea()
        {

        }
        public double locationX { get; }
        public double locationY { get; set; }
        public Image mainImage { get; }
        public int hp { get; private set; }
        public int points = 200;


        public void Movement()
        {
           
        }
        public bool checkCollision(Image shot)
        {
            bool hit = false;
            if (shot != null)
            {
              if (shot.Margin.Top <= locationY + 16 && (shot.Margin.Top) <= (locationY + 5)) 
                {
                   if (shot.Margin.Left >= locationX && shot.Margin.Left <= (locationX + 16))
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
