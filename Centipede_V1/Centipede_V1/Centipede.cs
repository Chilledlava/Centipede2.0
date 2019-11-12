using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Centipede_V1
{
    class Centipede
    {
        public Boolean isHead = false;
        public Image centipedeImage = new Image();
        public int health = 1;
        public Boolean facingRight = true;
        public Boolean alive = true;
        public Boolean poisoned = false;

        public Centipede(Boolean head)
        {
            isHead = head;
            
            if (head)
            {
                centipedeImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Centipede_Head_A.png"));
            }
            else
            {
                centipedeImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Centipede_Body.png"));

            }
        }


        public void CentipedeMoveLeft()
        {
            if(isHead)
                centipedeImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Centipede_Head_A_Left.png"));
            facingRight = false;
        }

        public void CentipedeMoveRight()
        {
            if(isHead)
                centipedeImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Centipede_Head_A.png"));
            facingRight = true;
        }

        public void CentipedeMoveDown()
        {
            centipedeImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Centipede_Head_A_Down - Copy.png"));
        }
    }
}
