using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Gaming.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Centipede_V1

{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
        // for future use when setting gameover perameters
        private bool GameOver = false;
        private bool CanShoot = true;
        private DispatcherTimer timer;
        private Image shot;
        private Random random;
        private Image shroom;
        private Image flea;        
        private int timesTicked = 1;      
        private List<Shroom> ShroomList;
        int amountOfShrooms;
        int timesKilled = 0;
        private CentipedeMover centipede;
        public Spider spiderSpawn;
        private Image spider;
        private Flea ActiveFlea;
        // use for either controller support or keyboard support (if done correctly)

        private Gamepad controller;


        public void SetUpMethod()
        {
            List<Shroom> ShroomList = new List<Shroom>();
            List<Shroom> tempshrooms = new List<Shroom>();
            SpawnShrooms();
            centipede = new CentipedeMover(9 +(2*timesKilled));
            centipede.speed = centipede.speed + timesKilled;
            spawnCentipede();


        }

        public MainPage()
        {
            // gets images to the edge of the tv
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetDesiredBoundsMode(Windows.UI.ViewManagement.ApplicationViewBoundsMode.UseCoreWindow);

            this.InitializeComponent();

            random = new Random();

            // setup game timer (shamlessly reappropriating from the pong program)
            ShroomList = new List<Shroom>();
            SetUpMethod();
            timer = new DispatcherTimer();
            // add event handler to the Tick event
            timer.Tick += DispatcherTimer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 2);
            timer.Start();
            
            // testing if spider spawns
            // DropSpider();

            // ensure that keypresses are captured no matter what UI element has the focus

            Window.Current.CoreWindow.KeyDown += KeyDown_Handler;

        }


        // moves player if using joystick or keyboard
        private void MovePlayer()
        {
            if (Gamepad.Gamepads.Count > 0)
            {
                controller = Gamepad.Gamepads.First();

                var reading = controller.GetCurrentReading();

                if ((reading.LeftThumbstickX < -.1 || reading.LeftThumbstickX > .1) && (Player.Margin.Left > LeftWall.Margin.Left) && (Player.Margin.Left + Player.Width < RightWall.Margin.Left))
                {
                    Player.Margin = new Thickness(Player.Margin.Left + 8 * reading.LeftThumbstickX, Player.Margin.Top, 0, 0);

                    if (Player.Margin.Left <= LeftWall.Margin.Left)
                    {
                        Player.Margin = new Thickness(Player.Margin.Left + 8, Player.Margin.Top, 0, 0);
                    }

                    if (Player.Margin.Left + Player.Width >= RightWall.Margin.Left)
                    {
                        Player.Margin = new Thickness(Player.Margin.Left - 8, Player.Margin.Top, 0, 0);
                    }

                    foreach (Shroom mush in ShroomList)
                    {
                        bool moved = false;
                        for (double y = Player.Margin.Top; y <= Player.Height + Player.Margin.Top; y++)
                        {
                            for (double x = Player.Margin.Left; x <= Player.Width + Player.Margin.Left; x++)
                            {
                                if (y <= mush.mainImage.Margin.Top + mush.mainImage.Height && y >= mush.mainImage.Margin.Top && x <= mush.mainImage.Margin.Left + mush.mainImage.Width && x >= mush.mainImage.Margin.Left)
                                {
                                    if (!moved)
                                    {
                                        Player.Margin = new Thickness(Player.Margin.Left - 8, Player.Margin.Top, 0, 0);
                                        moved = true;
                                    }
                                }
                            }
                        }
                    }

                    foreach (Shroom mush in ShroomList)
                    {
                        bool moved = false;
                        for (double y = Player.Margin.Top; y <= Player.Height + Player.Margin.Top; y++)
                        {
                            for (double x = Player.Margin.Left; x <= Player.Width + Player.Margin.Left; x++)
                            {
                                if (y <= mush.mainImage.Margin.Top + mush.mainImage.Height && y >= mush.mainImage.Margin.Top && x <= mush.mainImage.Margin.Left + mush.mainImage.Width && x >= mush.mainImage.Margin.Left)
                                {
                                    if (!moved)
                                    {
                                        Player.Margin = new Thickness(Player.Margin.Left + 8, Player.Margin.Top, 0, 0);
                                        moved = true;
                                    }
                                }
                            }
                        }
                    }
                }
                if ((reading.LeftThumbstickY < -.1 || reading.LeftThumbstickY > .1) && (Player.Margin.Top > TopWall.Margin.Top) && (Player.Margin.Top < BottomWall.Margin.Top - BottomWall.Height))
                {
                    Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - 8 * reading.LeftThumbstickY, 0, 0);

                    if (Player.Margin.Top <= TopWall.Margin.Top)
                    {
                        Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top + 8, 0, 0);
                    }

                    if (Player.Margin.Top >= BottomWall.Margin.Top - BottomWall.Height)
                    {
                        Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - 8, 0, 0);
                    }

                    foreach (Shroom mush in ShroomList)
                    {
                        bool moved = false;
                        for (double y = Player.Margin.Top; y <= Player.Height + Player.Margin.Top; y++)
                        {
                            for (double x = Player.Margin.Left; x <= Player.Width + Player.Margin.Left; x++)
                            {
                                if (y <= mush.mainImage.Margin.Top + mush.mainImage.Height && y >= mush.mainImage.Margin.Top && x <= mush.mainImage.Margin.Left + mush.mainImage.Width && x >= mush.mainImage.Margin.Left)
                                {
                                    if (!moved)
                                    {
                                        Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top + 8, 0, 0);
                                        moved = true;
                                    }
                                }
                            }
                        }
                    }

                    foreach (Shroom mush in ShroomList)
                    {
                        bool moved = false;
                        for (double y = Player.Margin.Top; y <= Player.Height + Player.Margin.Top; y++)
                        {
                            for (double x = Player.Margin.Left; x <= Player.Width + Player.Margin.Left; x++)
                            {
                                if (y <= mush.mainImage.Margin.Top + mush.mainImage.Height && y >= mush.mainImage.Margin.Top && x <= mush.mainImage.Margin.Left + mush.mainImage.Width && x >= mush.mainImage.Margin.Left)
                                {
                                    if (!moved)
                                    {
                                        Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - 8, 0, 0);
                                        moved = true;
                                    }
                                }
                            }
                        }
                    }
                }

                if (reading.RightTrigger > .5 && CanShoot)
                {
                    CanShoot = false;

                    makeShot();
                }
            }
        }
        // detects which key/direction on the keyboard is being pressed down

        private void KeyDown_Handler(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            // Arrow Button Controls
            if (e.VirtualKey == Windows.System.VirtualKey.Left && (Player.Margin.Left > LeftWall.Margin.Left))
            {
                Player.Margin = new Thickness(Player.Margin.Left - 5, Player.Margin.Top, 0, 0);

                if (Player.Margin.Left <= LeftWall.Margin.Left)
                {
                    Player.Margin = new Thickness(Player.Margin.Left + 10, Player.Margin.Top, 0, 0);
                }

                foreach (Shroom mush in ShroomList)
                {
                    bool moved = false;
                    for (double y = Player.Margin.Top; y <= Player.Height + Player.Margin.Top; y++)
                    {
                        for (double x = Player.Margin.Left; x <= Player.Width + Player.Margin.Left; x++)
                        {
                            if (y <= mush.mainImage.Margin.Top + mush.mainImage.Height && y >= mush.mainImage.Margin.Top && x <= mush.mainImage.Margin.Left + mush.mainImage.Width && x >= mush.mainImage.Margin.Left)
                            {
                                if (!moved)
                                {
                                    Player.Margin = new Thickness(Player.Margin.Left + 8, Player.Margin.Top, 0, 0);
                                    moved = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Right && (Player.Margin.Left + Player.Width < RightWall.Margin.Left))
            {
                Player.Margin = new Thickness(Player.Margin.Left + 5, Player.Margin.Top, 0, 0);

                if (Player.Margin.Left <= LeftWall.Margin.Left)
                {
                    Player.Margin = new Thickness(Player.Margin.Left + 8, Player.Margin.Top, 0, 0);
                }



                foreach (Shroom mush in ShroomList)
                {
                    bool moved = false;
                    for (double y = Player.Margin.Top; y <= Player.Height + Player.Margin.Top; y++)
                    {
                        for (double x = Player.Margin.Left; x <= Player.Width + Player.Margin.Left; x++)
                        {
                            if (y <= mush.mainImage.Margin.Top + mush.mainImage.Height && y >= mush.mainImage.Margin.Top && x <= mush.mainImage.Margin.Left + mush.mainImage.Width && x >= mush.mainImage.Margin.Left)
                            {
                                if (!moved)
                                {
                                    Player.Margin = new Thickness(Player.Margin.Left - 8, Player.Margin.Top, 0, 0);
                                    moved = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Up && (Player.Margin.Top > TopWall.Margin.Top))
            {
                Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - 5, 0, 0);

                if (Player.Margin.Top <= TopWall.Margin.Top)
                {
                    Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top + 8, 0, 0);
                }

                foreach (Shroom mush in ShroomList)
                {
                    bool moved = false;
                    for (double y = Player.Margin.Top; y <= Player.Height + Player.Margin.Top; y++)
                    {
                        for (double x = Player.Margin.Left; x <= Player.Width + Player.Margin.Left; x++)
                        {
                            if (y <= mush.mainImage.Margin.Top + mush.mainImage.Height && y >= mush.mainImage.Margin.Top && x <= mush.mainImage.Margin.Left + mush.mainImage.Width && x >= mush.mainImage.Margin.Left)
                            {
                                if (!moved)
                                {
                                    Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top + 8, 0, 0);
                                    moved = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Down && (Player.Margin.Top < BottomWall.Margin.Top - BottomWall.Height))
            {
                Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top + 5, 0, 0);

                if (Player.Margin.Top >= BottomWall.Margin.Top - BottomWall.Height)
                {
                    Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - 8, 0, 0);
                }

                foreach (Shroom mush in ShroomList)
                {
                    bool moved = false;
                    for (double y = Player.Margin.Top; y <= Player.Height + Player.Margin.Top; y++)
                    {
                        for (double x = Player.Margin.Left; x <= Player.Width + Player.Margin.Left; x++)
                        {
                            if (y <= mush.mainImage.Margin.Top + mush.mainImage.Height && y >= mush.mainImage.Margin.Top && x <= mush.mainImage.Margin.Left + mush.mainImage.Width && x >= mush.mainImage.Margin.Left)
                            {
                                if (!moved)
                                {
                                    Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - 8, 0, 0);
                                    moved = true;
                                }
                            }
                        }
                    }
                }

            }

            // WSAD controls
            else if (e.VirtualKey == Windows.System.VirtualKey.A && (Player.Margin.Left > LeftWall.Margin.Left))
            {
                Player.Margin = new Thickness(Player.Margin.Left - 5, Player.Margin.Top, 0, 0);

                if (Player.Margin.Left <= LeftWall.Margin.Left)
                {
                    Player.Margin = new Thickness(Player.Margin.Left + 10, Player.Margin.Top, 0, 0);
                }

                foreach (Shroom mush in ShroomList)
                {
                    bool moved = false;
                    for (double y = Player.Margin.Top; y <= Player.Height + Player.Margin.Top; y++)
                    {
                        for (double x = Player.Margin.Left; x <= Player.Width + Player.Margin.Left; x++)
                        {
                            if (y <= mush.mainImage.Margin.Top + mush.mainImage.Height && y >= mush.mainImage.Margin.Top && x <= mush.mainImage.Margin.Left + mush.mainImage.Width && x >= mush.mainImage.Margin.Left)
                            {
                                if (!moved)
                                {
                                    Player.Margin = new Thickness(Player.Margin.Left + 8, Player.Margin.Top, 0, 0);
                                    moved = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.D && (Player.Margin.Left + Player.Width < RightWall.Margin.Left))
            {
                Player.Margin = new Thickness(Player.Margin.Left + 5, Player.Margin.Top, 0, 0);

                if (Player.Margin.Left <= LeftWall.Margin.Left)
                {
                    Player.Margin = new Thickness(Player.Margin.Left + 8, Player.Margin.Top, 0, 0);
                }

                foreach (Shroom mush in ShroomList)
                {
                    bool moved = false;
                    for (double y = Player.Margin.Top; y <= Player.Height + Player.Margin.Top; y++)
                    {
                        for (double x = Player.Margin.Left; x <= Player.Width + Player.Margin.Left; x++)
                        {
                            if (y <= mush.mainImage.Margin.Top + mush.mainImage.Height && y >= mush.mainImage.Margin.Top && x <= mush.mainImage.Margin.Left + mush.mainImage.Width && x >= mush.mainImage.Margin.Left)
                            {
                                if (!moved)
                                {
                                    Player.Margin = new Thickness(Player.Margin.Left - 8, Player.Margin.Top, 0, 0);
                                    moved = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.W && (Player.Margin.Top > TopWall.Margin.Top))
            {
                Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - 5, 0, 0);

                if (Player.Margin.Top <= TopWall.Margin.Top)
                {
                    Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top + 8, 0, 0);
                }

                foreach (Shroom mush in ShroomList)
                {
                    bool moved = false;
                    for (double y = Player.Margin.Top; y <= Player.Height + Player.Margin.Top; y++)
                    {
                       for (double x = Player.Margin.Left; x <= Player.Width + Player.Margin.Left; x++)
                        {
                            if (y <= mush.mainImage.Margin.Top + mush.mainImage.Height && y >= mush.mainImage.Margin.Top && x <= mush.mainImage.Margin.Left + mush.mainImage.Width && x >= mush.mainImage.Margin.Left)
                            {
                                if (!moved)
                                {
                                    Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top + 8, 0, 0);
                                    moved = true;
                                }
                            }
                        }
                    }
                }
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.S && (Player.Margin.Top < BottomWall.Margin.Top - BottomWall.Height))
            {
                Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top + 5, 0, 0);

                if (Player.Margin.Top >= BottomWall.Margin.Top - BottomWall.Height)
                {
                    Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - 8, 0, 0);
                }

                foreach (Shroom mush in ShroomList)
                {
                    bool moved = false;
                    for (double y = Player.Margin.Top; y <= Player.Height + Player.Margin.Top; y++)
                    {
                        for (double x = Player.Margin.Left; x <= Player.Width + Player.Margin.Left; x++)
                        {
                            if (y <= mush.mainImage.Margin.Top + mush.mainImage.Height && y >= mush.mainImage.Margin.Top && x <= mush.mainImage.Margin.Left + mush.mainImage.Width && x >= mush.mainImage.Margin.Left)
                            {
                                if (!moved)
                                {
                                    Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - 8, 0, 0);
                                    moved = true;
                                }
                            }
                        }
                    }
                }
            
        }
            // spacebar to shoot && also spawn shroom
            if (e.VirtualKey == Windows.System.VirtualKey.Space && CanShoot)
            {
                CanShoot = false;


                makeShot();

                //test if spawning spider works
                spiderSpawn = DropSpider();

                // comment out later since the walls will be invisible 0_o
                TopWall.Fill = GetRandomColor();
                BottomWall.Fill = GetRandomColor();
                LeftWall.Fill = GetRandomColor();
                RightWall.Fill = GetRandomColor();
            }
        }

        // makes a random color for (does not work on images)
        private SolidColorBrush GetRandomColor()
        {
            return new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255)));
        }

        // got idea from https://www.mooict.com/c-tutorial-create-a-full-space-invaders-game-using-visual-studio/ && https://msdn.microsoft.com/en-us/library/system.windows.controls.image.source(v=vs.110).aspx
        private void makeShot()
        {
            shot = new Image();

            BitmapImage bitShot = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Missile.png"));


            shot.Stretch = Stretch.None;
            shot.Source = bitShot;

            shot.Width = 2;
            shot.Height = 10;
            shot.VerticalAlignment = VerticalAlignment.Top;
            shot.HorizontalAlignment = HorizontalAlignment.Left;
            shot.Margin = new Thickness(Player.Margin.Left + 7, Player.Margin.Top - 10, 0, 0);

            Background.Children.Add(shot);
        }

        private Flea DropFlea()
        {
            flea = new Image();
            random = new Random();

            int fleaLeftMargin = random.Next(0, 29) * 16;
            int fleaTopMargin = random.Next(0, 31) * 16;

            BitmapImage bitFlea = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Flea_A.png"));

            flea.Stretch = Stretch.None;
            flea.Source = bitFlea;

            flea.Width = 16;
            flea.Height = 16;
            flea.VerticalAlignment = VerticalAlignment.Top;
            flea.HorizontalAlignment = HorizontalAlignment.Left;
            flea.Margin = new Thickness(fleaLeftMargin,0 , 0, 0);

            Background.Children.Add(flea);

            Flea fleaBoi = new Flea(fleaLeftMargin,fleaTopMargin,flea);
            
            return fleaBoi;

        }

        private void fleaMovement()
        {
            if (ActiveFlea != null)
            {
                if (ActiveFlea.hp == 2)
                {
                    int x = (int)ActiveFlea.locationX;
                    int y = (int)ActiveFlea.locationY;
                    double newY = ActiveFlea.locationY + 8;
                    ActiveFlea.mainImage.Margin = new Thickness(ActiveFlea.locationX, newY, 0, 0);
                    ActiveFlea.locationY = newY;

                    if (ActiveFlea.locationY < 450)
                    {
                        Random random = new Random();
                        int number = random.Next(1, 100);
                        if (number % 8 == 0)
                        {
                            SpawnOneShroom(x, y);
                        }
                    }
                }
                else
                {
                    double newY = ActiveFlea.locationY + 12;
                    ActiveFlea.mainImage.Margin = new Thickness(ActiveFlea.locationX, newY, 0, 0);
                    ActiveFlea.locationY = newY;
                }
                if (ActiveFlea.locationY >= 496)
                {
                    Background.Children.Remove(flea);
                    ActiveFlea = null;
                }
            }
        }
        private void fleaCollision()
        {
            if (ActiveFlea != null)
            {
                double p = ActiveFlea.locationY;

                if (380 > p)
                {
                    return;
                }
                if (Player.Margin.Top <= ActiveFlea.locationY + 16 && (Player.Margin.Top) <= (ActiveFlea.locationY + 5))
                {

                    if (Player.Margin.Left + 16 >= ActiveFlea.locationX)
                    {
                        Background.Children.Remove(flea);
                        ActiveFlea = null;
                        //lives -= 1;
                        //if(lives == 0){gameover};

                    }
                }
            }

        }

        public void checkMushrooms()
        {
            int amount = 0;
            foreach (Shroom shroom in ShroomList)
            {

                if (shroom.locationY > 380)
                {
                    amount += 1;
                }
            }
            if (amount < 5 && ActiveFlea == null)
            {
                ActiveFlea = DropFlea();
            }
        }

        public Spider DropSpider()
        {
            spider = new Image();
            random = new Random();
            int[] possiblePositions = { 0, 450 };
            int randomIndex;
            randomIndex = random.Next(0, 2);

            int spiderLeftMargin = possiblePositions[randomIndex];
            int spiderTopMargin = (18) * 16;

            BitmapImage bitFlea = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Spider_A.png"));

            spider.Stretch = Stretch.None;
            spider.Source = bitFlea;

            spider.Width = 30;
            spider.Height = 16;
            spider.VerticalAlignment = VerticalAlignment.Top;
            spider.HorizontalAlignment = HorizontalAlignment.Left;
            spider.Margin = new Thickness(spiderLeftMargin, spiderTopMargin, 0, 0);

            Background.Children.Add(spider);

            Spider SpiderVar = new Spider(spiderLeftMargin, spiderTopMargin, spider);

            return SpiderVar;
        }

        private bool CheckForBulletWallCollision(Image shot)
        {
            bool destroyed = false;
            // if bullet hits the bullet wall it gets removed from the grid
            if ((shot.Margin.Top <= BulletBoundary.Margin.Top) && (shot != null))
            {
                Background.Children.Remove(shot);
                destroyed = true;
                CanShoot = true;
            }
            return destroyed;
        }


        //private void CheckForMushroomCollision(Image shroom)
        //{
        //    // if bullet hits a mushroom it gets removed from the grid (and should be able to set the shroom to a different image showing it lost health)
        //     if ((shot.Margin.Left <= shroom.Margin.Left - shroom.Width) &&  (shot.Margin.Top <= shroom.Margin.Top + shroom.Height) && (shot != null) )
        //    {
        //        Background.Children.Remove(shot);
        //        Background.Children.Remove(shroom);

        //        CanShoot = true;
        //    }
        //}

            private void SpawnOneShroom(int x, int y)
        {
            int shroomLeftMargin = x;
            int shroomTopMargin = y;
            shroom = new Image();
            BitmapImage bitShroom = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Normal_Full.png"));

            shroom.Stretch = Stretch.None;
            shroom.Source = bitShroom;

            shroom.Width = 16;
            shroom.Height = 16;
            shroom.VerticalAlignment = VerticalAlignment.Top;
            shroom.HorizontalAlignment = HorizontalAlignment.Left;
            shroom.Margin = new Thickness(shroomLeftMargin, shroomTopMargin, 0, 0);

            Background.Children.Add(shroom);

            Shroom spawnedShroom = new Shroom(shroomLeftMargin, shroomTopMargin, shroom);


            ShroomList.Add(spawnedShroom);
        }


        // create an instance of a multi mushroom
        private void SpawnShrooms()
        {
            List<Shroom> spawnedShrooms = new List<Shroom>();

            
            random = new Random();
            Random randomForLoop = new Random();
            int bound = randomForLoop.Next(25, 40);

            amountOfShrooms = bound;

            for (int i = 0; i < bound; i++)
            {
                shroom = new Image();
                int shroomLeftMargin = random.Next(0, 29) * 16;
                int shroomTopMargin = random.Next(0, 28) * 16;

                BitmapImage bitShroom = new BitmapImage(new Uri("ms-appx:/Assets/Cenitpede Sprites to .png/Mushroom_Normal_Full.png"));

                shroom.Stretch = Stretch.None;
                shroom.Source = bitShroom;

                shroom.Width = 16;
                shroom.Height = 16;
                shroom.VerticalAlignment = VerticalAlignment.Top;
                shroom.HorizontalAlignment = HorizontalAlignment.Left;
                shroom.Margin = new Thickness(shroomLeftMargin, shroomTopMargin, 0, 0);

                

                Background.Children.Add(shroom);

                Shroom spawnedShroom = new Shroom(shroomLeftMargin, shroomTopMargin, shroom);
                

                ShroomList.Add(spawnedShroom);
            }
            
        }


//Centipeded stuff
        public void spawnCentipede()
        {
            for (int i = centipede.amount; i > 0; i--)
            {
                centipede.parts[i-1].centipedeImage.VerticalAlignment = VerticalAlignment.Top;
                centipede.parts[i-1].centipedeImage.HorizontalAlignment = HorizontalAlignment.Left;
                centipede.parts[i - 1].centipedeImage.Margin = new Thickness(0 + (16 * (centipede.amount - i)), 0, 0, 0);
                centipede.parts[i - 1].centipedeImage.Height = 16;
                centipede.parts[i - 1].centipedeImage.Width = 16;
                Background.Children.Add(centipede.parts[i - 1].centipedeImage);
                centipede.parts[i - 1].centipedeImage.Visibility = Visibility.Visible;
            }
        }

        public void moveCentipede()
        {
            for (int i = 0; i < centipede.amount; i++)
            {
                Centipede current = centipede.parts[i];
                Boolean right = current.facingRight;
                if (current.poisoned)
                {
                    current.centipedeImage.Margin = new Thickness(current.centipedeImage.Margin.Left, current.centipedeImage.Margin.Top + centipede.speed + 12, 0, 0);
                }
                else
                {
                    if (right)//facing right
                    {
                        if (476 < current.centipedeImage.Margin.Left)
                        {
                            current.centipedeImage.Margin = new Thickness(current.centipedeImage.Margin.Left, current.centipedeImage.Margin.Top + 16, 0, 0);
                            current.centipedeImage.VerticalAlignment = VerticalAlignment.Top;
                            current.centipedeImage.HorizontalAlignment = HorizontalAlignment.Left;
                            current.CentipedeMoveLeft();
                        }
                        else
                        {
                            Boolean mushroom = false;
                            int mushIndex = -1;
                            for (int j = 0; j < amountOfShrooms; j++)
                            {
                                if (ShroomList[j].locationX-8 <= current.centipedeImage.Margin.Left
                                    && (ShroomList[j].locationY + 4 >= current.centipedeImage.Margin.Top && ShroomList[j].locationY - 4 <= current.centipedeImage.Margin.Top))
                                {
                                    mushroom = true;
                                    mushIndex = j;
                                }
                            }
                            if (mushroom)
                            {
                                if (ShroomList[mushIndex].poisioned)
                                {
                                    current.poisoned = true;
                                    current.centipedeImage.Margin = new Thickness(current.centipedeImage.Margin.Left, current.centipedeImage.Margin.Top + 16, 0, 0);
                                    current.CentipedeMoveDown();
                                }
                                else
                                {
                                    current.centipedeImage.Margin = new Thickness(current.centipedeImage.Margin.Left, current.centipedeImage.Margin.Top + 16, 0, 0);
                                    current.centipedeImage.Margin = new Thickness(current.centipedeImage.Margin.Left - (centipede.speed + 5), current.centipedeImage.Margin.Top, 0, 0);
                                    current.CentipedeMoveLeft();
                                }
                                current.centipedeImage.VerticalAlignment = VerticalAlignment.Top;
                                current.centipedeImage.HorizontalAlignment = HorizontalAlignment.Left;

                            }
                            else
                            {
                                current.centipedeImage.Margin = new Thickness(current.centipedeImage.Margin.Left + centipede.speed, current.centipedeImage.Margin.Top, 0, 0);
                                current.centipedeImage.VerticalAlignment = VerticalAlignment.Top;
                                current.centipedeImage.HorizontalAlignment = HorizontalAlignment.Left;
                            }
                        }
                    }
                    else//facing left
                    {
                        if (-2 > current.centipedeImage.Margin.Left)
                        {
                            current.centipedeImage.Margin = new Thickness(current.centipedeImage.Margin.Left, current.centipedeImage.Margin.Top + 16, 0, 0);
                            current.centipedeImage.VerticalAlignment = VerticalAlignment.Top;
                            current.centipedeImage.HorizontalAlignment = HorizontalAlignment.Left;
                            current.CentipedeMoveRight();
                        }
                        else
                        {
                            Boolean mushroom = false;
                            int mushIndex = -1;
                            for (int j = 0; j < amountOfShrooms; j++)
                            {
                                if (ShroomList[j].locationX + 8 >= current.centipedeImage.Margin.Left
                                    && (ShroomList[j].locationY + 4 >= current.centipedeImage.Margin.Top && ShroomList[j].locationY - 4 <= current.centipedeImage.Margin.Top))
                                {
                                    mushroom = true;
                                    mushIndex = j;
                                }
                            }
                            if (mushroom)
                            {
                                if (ShroomList[mushIndex].poisioned)
                                {
                                    current.poisoned = true;
                                    current.centipedeImage.Margin = new Thickness(current.centipedeImage.Margin.Left, current.centipedeImage.Margin.Top + 16, 0, 0);
                                    current.CentipedeMoveDown();
                                }
                                else
                                {
                                    current.centipedeImage.Margin = new Thickness(current.centipedeImage.Margin.Left, current.centipedeImage.Margin.Top + 16, 0, 0);
                                    current.centipedeImage.Margin = new Thickness(current.centipedeImage.Margin.Left + (centipede.speed + 5), current.centipedeImage.Margin.Top, 0, 0);
                                    current.CentipedeMoveRight();
                                }

                                current.centipedeImage.VerticalAlignment = VerticalAlignment.Top;
                                current.centipedeImage.HorizontalAlignment = HorizontalAlignment.Left;
                                
                            }
                            else
                            {
                                current.centipedeImage.Margin = new Thickness(current.centipedeImage.Margin.Left - centipede.speed, current.centipedeImage.Margin.Top, 0, 0);
                                current.centipedeImage.VerticalAlignment = VerticalAlignment.Top;
                                current.centipedeImage.HorizontalAlignment = HorizontalAlignment.Left;
                            }
                        }
                    }
                }
            }
        }


        private void DispatcherTimer_Tick(object sender, object e)
        {
            bool shotDisapeered = false, hit = false, hitSpider = false, hitFlea = false;

            bool done = false;
            MovePlayer();
            moveCentipede();
            // if shot is created on the board it will continue to move up until it is deleted
            if (shot != null)
            {
                shot.Margin = new Thickness(shot.Margin.Left, shot.Margin.Top - 20, 0, 0);
                List<Shroom> DeleteList = new List<Shroom>();
                DeleteList = Shroom.checkCollisionShroom(ShroomList, shot, out hit);
                amountOfShrooms = amountOfShrooms - DeleteList.Count;
                if (hit)
                {
                    Background.Children.Remove(shot);
                    shot = null;
                    CanShoot = true;
                }
                 foreach (Shroom shroom in DeleteList)
                {
                    Background.Children.Remove(shroom.mainImage);
                    if (ShroomList.Contains(shroom))
                    {
                        ShroomList.Remove(shroom);
                    }
                }
                DeleteList.Clear();

                if (ActiveFlea != null)
                {
                    hitFlea = ActiveFlea.checkCollision(shot);
                }

                if (ActiveFlea != null && ActiveFlea.hp <= 0)
                {
                    Background.Children.Remove(flea);
                    ActiveFlea = null;
                }


                if (hit || hitFlea)
                {
                    Background.Children.Remove(shot);
                    shot = null;
                    CanShoot = true;
                }

                Boolean hitCentipede = false;
                if (shot != null)
                {
                    centipede.checkCentipedeCollision(shot, out hitCentipede);
                }

                if (hitCentipede)
                {
                    
                    CanShoot = true;
                    Background.Children.Remove(shot);
                    shot = null;
                    for (int i = 0; i < centipede.amount; i++)
                    {
                        if (centipede.parts[i].alive == false)
                        {
                            Background.Children.Remove(centipede.parts[i].centipedeImage);
                            centipede.parts.RemoveAt(i);
                            centipede.amount--;
                        }
                    }
                }
                if(shot != null)
                    shotDisapeered = CheckForBulletWallCollision(shot);
                if (shotDisapeered)
                {
                    shot = null;
                }
                
                hitSpider = Spider.checkCollision(spiderSpawn, shot);
            }
            checkMushrooms(); // checks if there are 5 mushrooms in the playing field in order to spawn flea
            fleaMovement(); // moves the flea
            fleaCollision();

            if (centipede.amount == 0)
            {
                timesKilled++;

                foreach (Shroom mush in ShroomList)
                {
                    Background.Children.Remove(mush.mainImage);
                }
                ShroomList.Clear();
                SetUpMethod();
            }
            if (GameOver)
            {
                this.Frame.Navigate(typeof(GameOverPage));
            }
            timesTicked += 1;

        }

    }
}