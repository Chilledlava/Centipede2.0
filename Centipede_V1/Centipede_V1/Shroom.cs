using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;


namespace Centipede_V1
{

    class Shroom
    {
        public Shroom(int left, int top, Image shroomObj)
        {
            hp = 4;
            locationX = left;
            locationY = top;
            mainImage = shroomObj;

        }

        public double locationX { get; }
        public double locationY { get; }
        public Image mainImage { get; }

        public int hp { get; private set; }
        public bool poisioned { get; private set; }



        bool shootShroom()
        {
            bool dead = true;
            hp = hp - 1;
            if (!poisioned)
            {
                switch (hp)
                {
                    case 0:

                        return true;                

                    case 1:

                        BitmapImage bitShroom1 = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Normal_25.png"));
                        mainImage.Source = bitShroom1;
                        dead = false;
                        break;

                    case 2:

                        BitmapImage bitShroom2 = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Normal_50.png"));
                        mainImage.Source = bitShroom2;
                        dead = false;
                        break;

                    case 3:

                        BitmapImage bitShroom3 = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Normal_75.png"));
                        mainImage.Source = bitShroom3;
                        dead = false;
                        break;

                    case 4:

                        BitmapImage bitShroom4 = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Normal_Full.png"));
                        mainImage.Source = bitShroom4;
                        dead = false;
                        break;
                }
            }

            else
            {
                switch (hp)
                {
                    case 0:
                        return true;

                    case 1:

                        BitmapImage bitShroom1 = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Poison_25.png"));
                        mainImage.Source = bitShroom1;
                        dead = false;
                        break;

                    case 2:

                        BitmapImage bitShroom2 = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Poision_50.png"));
                        mainImage.Source = bitShroom2;
                        dead = false;
                        break;

                    case 3:

                        BitmapImage bitShroom3 = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Poison_75.png"));
                        mainImage.Source = bitShroom3;
                        dead = false;
                        break;

                    case 4:

                        BitmapImage bitShroom4 = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Poison_Full.png"));
                        mainImage.Source = bitShroom4;
                        dead = false;
                        break;
                }
            }
            return dead;
        }


        void poisonShroom()
        {
            poisioned = true;
            switch (hp)
            {
                case 1:

                    BitmapImage bitShroom1 = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Poison_25.png"));
                    mainImage.Source = bitShroom1;
                    break;

                case 2:

                    BitmapImage bitShroom2 = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Poision_50.png"));
                    mainImage.Source = bitShroom2;
                    break;

                case 3:

                    BitmapImage bitShroom3 = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Poison_75.png"));
                    mainImage.Source = bitShroom3;
                    break;

                case 4:

                    BitmapImage bitShroom4 = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Poison_Full.png"));
                    mainImage.Source = bitShroom4;
                    break;
            }
        }

        public static List<Shroom> checkCollisionShroom(List<Shroom> MushroomListInport, Image shot, out bool hit) //Remember to delete the return list :) 
        {
            hit = false;
            List<Shroom> DeleteThese = new List<Shroom>();

            if (shot != null)
            {
                List<Shroom> MushroomList = new List<Shroom>();
                MushroomList = MushroomListInport;
                 foreach (Shroom shroom in MushroomList)
                {
                    if (shroom.locationY >= shot.Margin.Top && shot.Margin.Top <= (shroom.locationY + shroom.mainImage.Width))
                    {
                        if (shroom.locationX <= shot.Margin.Left && shot.Margin.Left <= (shroom.locationX + shroom.mainImage.Height))
                        {
                            //😍 Valid Code.

                            bool died;
                            died = shroom.shootShroom();
                            hit = true;
                            if (died)
                            {
                                DeleteThese.Add(shroom);
                            }
                        }
                    }

                }
                foreach (Shroom shroom in DeleteThese)
                {
                    MushroomList.Remove(shroom);
                }
            }
            return DeleteThese;
        }
    }
}