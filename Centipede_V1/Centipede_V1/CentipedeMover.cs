using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Centipede_V1
{
    class CentipedeMover
    {
        public List<Centipede> parts = new List<Centipede>();
        public int speed = 1; //for easy changing how fast the centipede moves
        public int amount;
        public int totalCentipedeScore = 0;

        public CentipedeMover(int bodyParts)
        {
            amount = bodyParts;
            for (int i = 0; i < bodyParts; i++)
            {
                if (i == 0)
                    parts.Add(new Centipede(true));
                else
                    parts.Add(new Centipede(false));//true for head, false for body part
            }
        }

        public void checkCentipedeCollision(Image shot, out Boolean hit)
        {
            hit = false;
            for(int i = 0;i < amount; i++)
            {
                if (shot.Margin.Top <= parts[i].centipedeImage.Margin.Top - 16 &&
                    (shot.Margin.Left >= parts[i].centipedeImage.Margin.Left && shot.Margin.Left <= parts[i].centipedeImage.Margin.Left + 16))
                {
                    parts[i].alive = false;
                    if (parts[i].isHead)
                        totalCentipedeScore += 100;
                    else
                        totalCentipedeScore += 10;

                    if (i + 1 < amount)
                    {
                        parts[i + 1].isHead = true;
                        if(parts[i+1].facingRight)
                            parts[i+1].centipedeImage.Source =  new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Centipede_Head_A.png"));
                        else
                            parts[i + 1].centipedeImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Centipede_Head_A_Left.png"));

                    }

                    hit = true;
                }
            }
        }

        
    }
}
