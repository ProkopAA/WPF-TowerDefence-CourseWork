using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using WMPLib;

namespace Cursovich
{



    public partial class MainWindow : Window
    {
        private string GetResourcePath(string fileName)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return System.IO.Path.Combine(baseDir, "Resources", fileName);
        }

        private Uri GetResourceUri(string fileName)
        {
            return new Uri(GetResourcePath(fileName), UriKind.Absolute);
        }

        Button StartWave;
        Rectangle BaseHealthBar;
        int Wave, Money, WaveDelay, WaveDogCount, WaveZombieCount, WaveBossCount;
        Label MoneyCount, WaveCount;
        string Tower1Source, Tower2Source, Tower3Source;
        DispatcherTimer attackTimer;
        int DogHealth = 50, ZombieHealth = 200, BossHealth = 1000;

        WindowsMediaPlayer mediaPlayer, ShotSoundPlayer;
        string ScoutShot = System.IO.Path.Combine("Resources", "ScoutSound.mp3");
        string SniperShot = System.IO.Path.Combine("Resources", "SniperSound.mp3");
        string FlamerShot = System.IO.Path.Combine("Resources", "FlamerSound.mp3");
        public MainWindow()
        {
            InitializeComponent();
            ShotSoundPlayer = new WindowsMediaPlayer();
        }

        private List<EnemyUnit> enemyUnits = new List<EnemyUnit>();

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        class TowerUnit
        {
            public Image TowerImage { get; set; }
            public int Distance { get; set; }
            public int Damage { get; set; }
            public double ShootSPD { get; set; }
            public bool AOEStatus { get; set; }
            public bool SniperStatus { get; set; }
            public DispatcherTimer AttackTimer { get; set; }
            public Panel ParentGrid { get; set; }

            public TowerUnit(Image towerImage, int Adistance, int damage, double shootSpeed, bool aoe, Panel parentGrid, bool sniperStatus)
            {
                TowerImage = towerImage;
                Distance = Adistance;
                ShootSPD = shootSpeed;
                Damage = damage;
                AOEStatus = aoe;
                SniperStatus = sniperStatus;
                ParentGrid = parentGrid;
            }
        }


        public class EnemyUnit
        {
            public Image EnemyImage { get; set; }
            public Rectangle HealthBar { get; set; }
            public Rectangle MaxHealthBar { get; set; }
            public int Health { get; set; }
            public int Damage { get; set; }
            public double Speed { get; set; }
            public bool BossStatus { get; set; }
            public int MaxHealth { get; set; }

            public EnemyUnit(Image enemyImage, Rectangle healthBar, Rectangle maxHealthBar, int maxHealth, int health, int damage, double speed, bool boss)
            {
                EnemyImage = enemyImage;
                HealthBar = healthBar;
                MaxHealthBar = maxHealthBar;
                MaxHealth = maxHealth;
                Health = health;
                Damage = damage;
                Speed = speed;
                BossStatus = boss;
            }
        }


        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        public void MoveonWay(EnemyUnit enemyUnit, double SPD)
        {
            enemyUnit.EnemyImage.RenderTransform = new TranslateTransform();
            TranslateTransform transform = (TranslateTransform)enemyUnit.EnemyImage.RenderTransform;

            DoubleAnimation FirstWay = new DoubleAnimation
            {
                From = 0,
                To = 150,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 3))
            };
            DoubleAnimation SecondWay = new DoubleAnimation
            {
                From = 0,
                To = -100,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 2))
            };
            DoubleAnimation ThirdWay = new DoubleAnimation
            {
                From = 150,
                To = 400,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 5))
            };
            DoubleAnimation FourthWay = new DoubleAnimation
            {
                From = -100,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 2))
            };
            DoubleAnimation FifthWay = new DoubleAnimation
            {
                From = 400,
                To = 500,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 2))
            };
            DoubleAnimation SixthWay = new DoubleAnimation
            {
                From = 0,
                To = 50,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 1))
            };
            DoubleAnimation SeventhWay = new DoubleAnimation
            {
                From = 500,
                To = 750,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 5))
            };
            DoubleAnimation EighthWay = new DoubleAnimation
            {
                From = 50,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 1))
            };
            DoubleAnimation NinethWay = new DoubleAnimation
            {
                From = 750,
                To = 900,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 3))
            };
            DoubleAnimation TenthWay = new DoubleAnimation
            {
                From = 900,
                To = 950,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 1))
            };

            FirstWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.YProperty, SecondWay);
            };
            SecondWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.XProperty, ThirdWay);
            };
            ThirdWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.YProperty, FourthWay);
            };
            FourthWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.XProperty, FifthWay);
            };
            FifthWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.YProperty, SixthWay);
            };
            SixthWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.XProperty, SeventhWay);
            };
            SeventhWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.YProperty, EighthWay);
            };
            EighthWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.XProperty, NinethWay);
            };
            NinethWay.Completed += (S, E) =>
            {
                if (BaseHealthBar.Width == 0 && enemyUnit.Health > 0)
                {
                    transform.BeginAnimation(TranslateTransform.XProperty, TenthWay);
                }
                else if (enemyUnit.Health > 0)
                {
                    BaseHealthBar.Width = Math.Max(0, BaseHealthBar.Width - (enemyUnit.Damage * 3));
                    GameGrid.Children.Remove(enemyUnit.EnemyImage);
                    GameGrid.Children.Remove(enemyUnit.HealthBar);
                    GameGrid.Children.Remove(enemyUnit.MaxHealthBar);
                    enemyUnits.Remove(enemyUnit);
                }
            };
            TenthWay.Completed += (S, E) =>
            {
                MessageBoxResult result = MessageBox.Show($"Игра окончена! Вы сдержали {Wave - 1} волн(ы).",
                "Игра окончена", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown(); // Закрываем приложение
                }
                GameGrid.Children.Remove(enemyUnit.EnemyImage);
                GameGrid.Children.Remove(enemyUnit.HealthBar);
                GameGrid.Children.Remove(enemyUnit.MaxHealthBar);
                enemyUnits.Remove(enemyUnit);
            };
            transform.BeginAnimation(TranslateTransform.XProperty, FirstWay);
        }

        public void HealthBarWay(Rectangle Bar, double SPD)
        {
            Bar.RenderTransform = new TranslateTransform();
            TranslateTransform transform = (TranslateTransform)Bar.RenderTransform;

            DoubleAnimation FirstWay = new DoubleAnimation
            {
                From = 0,
                To = 150,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 3))
            };
            DoubleAnimation SecondWay = new DoubleAnimation
            {
                From = 0,
                To = -100,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 2))
            };
            DoubleAnimation ThirdWay = new DoubleAnimation
            {
                From = 150,
                To = 400,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 5))
            };
            DoubleAnimation FourthWay = new DoubleAnimation
            {
                From = -100,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 2))
            };
            DoubleAnimation FifthWay = new DoubleAnimation
            {
                From = 400,
                To = 500,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 2))
            };
            DoubleAnimation SixthWay = new DoubleAnimation
            {
                From = 0,
                To = 50,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 1))
            };
            DoubleAnimation SeventhWay = new DoubleAnimation
            {
                From = 500,
                To = 750,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 5))
            };
            DoubleAnimation EighthWay = new DoubleAnimation
            {
                From = 50,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 1))
            };
            DoubleAnimation NinethWay = new DoubleAnimation
            {
                From = 750,
                To = 900,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 3))
            };
            DoubleAnimation TenthWay = new DoubleAnimation
            {
                From = 900,
                To = 950,
                Duration = new Duration(TimeSpan.FromSeconds(SPD * 1))
            };

            FirstWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.YProperty, SecondWay);
            };
            SecondWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.XProperty, ThirdWay);
            };
            ThirdWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.YProperty, FourthWay);
            };
            FourthWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.XProperty, FifthWay);
            };
            FifthWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.YProperty, SixthWay);
            };
            SixthWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.XProperty, SeventhWay);
            };
            SeventhWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.YProperty, EighthWay);
            };
            EighthWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.XProperty, NinethWay);
            };
            NinethWay.Completed += (S, E) =>
            {
                transform.BeginAnimation(TranslateTransform.XProperty, TenthWay);
            };
            transform.BeginAnimation(TranslateTransform.XProperty, FirstWay);
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer = new WindowsMediaPlayer();
            mediaPlayer.URL = GetResourcePath("backgroundMusic.mp3");
            Tower1Source = GetResourcePath("ScoutImage.png");
            Tower2Source = GetResourcePath("FlamerImage.png");
            Tower3Source = GetResourcePath("SniperImage.png");
            MenuGrid.Visibility = Visibility.Hidden;
            Money = 200;
            Wave = 0;
            WaveDogCount = 2;
            WaveZombieCount = 0;
            WaveBossCount = 0;
            WaveDelay = 3000;



            Rectangle[] Enemies = new Rectangle[WaveDogCount];

            WaveCount = new Label
            {
                Content = $"Волна: {Wave}",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(50, 80, 0, 0),
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                FontSize = 30,
                FontWeight = FontWeights.Bold
            };

            MoneyCount = new Label
            {
                Content = $"Рубли: {Money}",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(350, 30, 0, 0),
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                FontSize = 30,
                FontWeight = FontWeights.Bold
            };

            string MapSource = GetResourcePath("NearBase.png");

            Image Map = new Image
            {
                Source = new BitmapImage(new Uri(MapSource)),
                Width = 1000,
                Height = 470,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 25, 0, 0)
            };

            StartWave = new Button
            {
                Content = "Начать волну",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 220,
                Height = 60,
                FontSize = 25,
                Margin = new Thickness(0, 50, 100, 0)
            };
            StartWave.Click += StartWave_Click;

            Label SniperName = new Label
            {
                Content = "Снайпер",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 302, 82),
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                FontSize = 15,
                FontWeight = FontWeights.Bold
            };

            Label ScoutName = new Label
            {
                Content = "Разведчик",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(2, 0, 0, 82),
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                FontSize = 15,
                FontWeight = FontWeights.Bold
            };

            Label FlamerName = new Label
            {
                Content = "Огнемётчик",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(302, 0, 0, 82),
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                FontSize = 15,
                FontWeight = FontWeights.Bold
            };

            Button SniperTower = new Button
            {

                Content = "",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0,
                FontSize = 15,
                Margin = new Thickness(0, 0, 302, 32)
            };
            SniperTower.Click += SniperSpawn;

            Button ScoutTower = new Button
            {

                Content = "",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0,
                FontSize = 15,
                Margin = new Thickness(2, 0, 0, 32)
            };
            ScoutTower.Click += ScoutSpawn;

            Button FlamerTower = new Button
            {
                Content = "",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0,
                FontSize = 15,
                Margin = new Thickness(302, 0, 0, 32)
            };
            FlamerTower.Click += FlamerSpawn;

            Image FlamerImage = new Image
            {
                Source = new BitmapImage(new Uri(Tower2Source)),
                Width = 50,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(302, 0, 0, 32)
            };

            Image ScoutImage = new Image
            {
                Source = new BitmapImage(new Uri(Tower1Source)),
                Width = 50,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(2, 0, 0, 32)
            };

            Image SniperImage = new Image
            {
                Source = new BitmapImage(new Uri(Tower3Source)),
                Width = 50,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 302, 32)
            };

            Label SniperCost = new Label
            {
                Content = "300",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 302, 0),
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                FontSize = 15,
                FontWeight = FontWeights.Bold
            };

            Label ScoutCost = new Label
            {
                Content = "100",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(2, 0, 0, 0),
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                FontSize = 15,
                FontWeight = FontWeights.Bold
            };

            Label FlamerCost = new Label
            {
                Content = "200",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(302, 0, 0, 0),
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                FontSize = 15,
                FontWeight = FontWeights.Bold
            };
            BaseHealthBar = new Rectangle
            {
                Width = 300,
                Height = 30,
                Fill = Brushes.LawnGreen,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 40, 0, 0)
            };
            Rectangle BaseMaxHealthBar = new Rectangle
            {
                Width = 300,
                Height = 30,
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20, 40, 0, 0)
            };

            GameGrid.Children.Add(Map);
            GameGrid.Children.Add(WaveCount);
            GameGrid.Children.Add(MoneyCount);
            GameGrid.Children.Add(StartWave);
            GameGrid.Children.Add(ScoutImage);
            GameGrid.Children.Add(ScoutTower);
            GameGrid.Children.Add(ScoutName);
            GameGrid.Children.Add(ScoutCost);
            GameGrid.Children.Add(FlamerImage);
            GameGrid.Children.Add(FlamerTower);
            GameGrid.Children.Add(FlamerName);
            GameGrid.Children.Add(FlamerCost);
            GameGrid.Children.Add(SniperImage);
            GameGrid.Children.Add(SniperTower);
            GameGrid.Children.Add(SniperName);
            GameGrid.Children.Add(SniperCost);
            GameGrid.Children.Add(BaseMaxHealthBar);
            GameGrid.Children.Add(BaseHealthBar);
        }



        private async void StartWave_Click(object sender, RoutedEventArgs e)
        {
            Wave += 1;
            WaveCount.Content = $"Волна: {Wave}";
            WaveDelay += 500;
            WaveEvolution();
            Money += 50;
            MoneyCount.Content = $"Рубли: {Money}";
            StartWave.IsEnabled = false;

            string Enemy3Source = GetResourcePath("Boss.png");
            for (int i = 0; i < WaveBossCount; i++)
            {
                Image BossImage = new Image
                {
                    Source = new BitmapImage(new Uri(Enemy3Source)),
                    Width = 50,
                    Height = 50,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 25, 0, 0)
                };

                Rectangle healthBar = new Rectangle
                {
                    Width = 50,
                    Height = 10,
                    Fill = Brushes.Violet,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 90)
                };

                Rectangle MaxhealthBar = new Rectangle
                {
                    Width = 50,
                    Height = 10,
                    Fill = Brushes.Red,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 90)
                };

                GameGrid.Children.Add(MaxhealthBar);
                GameGrid.Children.Add(healthBar);
                EnemyUnit BossUnit = new EnemyUnit(BossImage, healthBar, MaxhealthBar, BossHealth, BossHealth, 100, 1.2, true);
                enemyUnits.Add(BossUnit);
                GameGrid.Children.Add(BossUnit.EnemyImage);
                MoveonWay(BossUnit, BossUnit.Speed);
                HealthBarWay(healthBar, BossUnit.Speed);
                HealthBarWay(MaxhealthBar, BossUnit.Speed);
                await Task.Delay(1500);
            }
            await Task.Delay(500);
            string Enemy2Source = GetResourcePath("Zombie.png");
            for (int i = 0; i < WaveZombieCount; i++)
            {
                Image ZombieImage = new Image
                {
                    Source = new BitmapImage(new Uri(Enemy2Source)),
                    Width = 50,
                    Height = 50,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 25, 0, 0)
                };

                Rectangle healthBar = new Rectangle
                {
                    Width = 50,
                    Height = 10,
                    Fill = Brushes.LawnGreen,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 90)
                };

                Rectangle MaxhealthBar = new Rectangle
                {
                    Width = 50,
                    Height = 10,
                    Fill = Brushes.Red,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 90)
                };

                GameGrid.Children.Add(MaxhealthBar);
                GameGrid.Children.Add(healthBar);
                EnemyUnit ZombieUnit = new EnemyUnit(ZombieImage, healthBar, MaxhealthBar, ZombieHealth, ZombieHealth, 20, 1, false);
                enemyUnits.Add(ZombieUnit);
                GameGrid.Children.Add(ZombieUnit.EnemyImage);
                MoveonWay(ZombieUnit, ZombieUnit.Speed);
                HealthBarWay(healthBar, ZombieUnit.Speed);
                HealthBarWay(MaxhealthBar, ZombieUnit.Speed);
                await Task.Delay(1500);
            }
            await Task.Delay(500);
            string Enemy1Source = GetResourcePath("Dog.png");
            for (int i = 0; i < WaveDogCount; i++)
            {
                Image dogImage = new Image
                {
                    Source = new BitmapImage(new Uri(Enemy1Source)),
                    Width = 50,
                    Height = 50,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 25, 0, 0)
                };

                Rectangle healthBar = new Rectangle
                {
                    Width = 50,
                    Height = 10,
                    Fill = Brushes.LawnGreen,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 90)
                };

                Rectangle MaxhealthBar = new Rectangle
                {
                    Width = 50,
                    Height = 10,
                    Fill = Brushes.Red,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 90)
                };

                GameGrid.Children.Add(MaxhealthBar);
                GameGrid.Children.Add(healthBar);
                EnemyUnit DogUnit = new EnemyUnit(dogImage, healthBar, MaxhealthBar, DogHealth, DogHealth, 10, 0.3, false);
                enemyUnits.Add(DogUnit);
                GameGrid.Children.Add(DogUnit.EnemyImage);
                MoveonWay(DogUnit, DogUnit.Speed);
                HealthBarWay(healthBar, DogUnit.Speed);
                HealthBarWay(MaxhealthBar, DogUnit.Speed);
                await Task.Delay(1500);
            }
            await Task.Delay(WaveDelay);
            StartWave.IsEnabled = true;
        }

        private void WaveEvolution()
        {
            if (WaveDogCount < 20)
            { WaveDogCount += 1; }
            DogHealth += 20;
            if (WaveZombieCount > 0)
            {
                ZombieHealth += 50;
            }

            if (WaveDogCount % 5 == 0 && WaveZombieCount < 20)
            {
                WaveZombieCount += 2;

            }

            if (Wave % 10 == 0)
            {
                WaveBossCount = 1;
                if (WaveBossCount > 1)
                {
                    BossHealth += 500;
                }
            }
            else
            {
                WaveBossCount = 0;
            }
        }

        private List<Button> towerPlacementButtons = new List<Button>();
        private void ShowTowerPlacementOptions(int TowerVariant)
        {

            foreach (var button in towerPlacementButtons)
            {
                GameGrid.Children.Remove(button);
            }
            towerPlacementButtons.Clear();

            Button TowerPlace1 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(0, 0, 750, 75)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace1.Click += (s, e) => PlaceSniperTower(TowerPlace1);
                    break;
                case 2:
                    TowerPlace1.Click += (s, e) => PlaceScoutTower(TowerPlace1);
                    break;
                case 3:
                    TowerPlace1.Click += (s, e) => PlaceFlamerTower(TowerPlace1);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace1);
            GameGrid.Children.Add(TowerPlace1);

            Button TowerPlace2 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(0, 0, 550, 75)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace2.Click += (s, e) => PlaceSniperTower(TowerPlace2);
                    break;
                case 2:
                    TowerPlace2.Click += (s, e) => PlaceScoutTower(TowerPlace2);
                    break;
                case 3:
                    TowerPlace2.Click += (s, e) => PlaceFlamerTower(TowerPlace2);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace2);
            GameGrid.Children.Add(TowerPlace2);

            Button TowerPlace3 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(0, 0, 450, 75)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace3.Click += (s, e) => PlaceSniperTower(TowerPlace3);
                    break;
                case 2:
                    TowerPlace3.Click += (s, e) => PlaceScoutTower(TowerPlace3);
                    break;
                case 3:
                    TowerPlace3.Click += (s, e) => PlaceFlamerTower(TowerPlace3);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace3);
            GameGrid.Children.Add(TowerPlace3);

            Button TowerPlace4 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(0, 0, 350, 75)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace4.Click += (s, e) => PlaceSniperTower(TowerPlace4);
                    break;
                case 2:
                    TowerPlace4.Click += (s, e) => PlaceScoutTower(TowerPlace4);
                    break;
                case 3:
                    TowerPlace4.Click += (s, e) => PlaceFlamerTower(TowerPlace4);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace4);
            GameGrid.Children.Add(TowerPlace4);

            Button TowerPlace5 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(0, 0, 250, 75)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace5.Click += (s, e) => PlaceSniperTower(TowerPlace5);
                    break;
                case 2:
                    TowerPlace5.Click += (s, e) => PlaceScoutTower(TowerPlace5);
                    break;
                case 3:
                    TowerPlace5.Click += (s, e) => PlaceFlamerTower(TowerPlace5);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace5);
            GameGrid.Children.Add(TowerPlace5);

            Button TowerPlace6 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(0, 0, 450, 275)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace6.Click += (s, e) => PlaceSniperTower(TowerPlace6);
                    break;
                case 2:
                    TowerPlace6.Click += (s, e) => PlaceScoutTower(TowerPlace6);
                    break;
                case 3:
                    TowerPlace6.Click += (s, e) => PlaceFlamerTower(TowerPlace6);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace6);
            GameGrid.Children.Add(TowerPlace6);

            Button TowerPlace7 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(0, 0, 350, 275)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace7.Click += (s, e) => PlaceSniperTower(TowerPlace7);
                    break;
                case 2:
                    TowerPlace7.Click += (s, e) => PlaceScoutTower(TowerPlace7);
                    break;
                case 3:
                    TowerPlace7.Click += (s, e) => PlaceFlamerTower(TowerPlace7);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace7);
            GameGrid.Children.Add(TowerPlace7);

            Button TowerPlace8 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(0, 0, 50, 75)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace8.Click += (s, e) => PlaceSniperTower(TowerPlace8);
                    break;
                case 2:
                    TowerPlace8.Click += (s, e) => PlaceScoutTower(TowerPlace8);
                    break;
                case 3:
                    TowerPlace8.Click += (s, e) => PlaceFlamerTower(TowerPlace8);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace8);
            GameGrid.Children.Add(TowerPlace8);

            Button TowerPlace9 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(0, 125, 50, 0)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace9.Click += (s, e) => PlaceSniperTower(TowerPlace9);
                    break;
                case 2:
                    TowerPlace9.Click += (s, e) => PlaceScoutTower(TowerPlace9);
                    break;
                case 3:
                    TowerPlace9.Click += (s, e) => PlaceFlamerTower(TowerPlace9);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace9);
            GameGrid.Children.Add(TowerPlace9);

            Button TowerPlace10 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(150, 20, 0, 0)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace10.Click += (s, e) => PlaceSniperTower(TowerPlace10);
                    break;
                case 2:
                    TowerPlace10.Click += (s, e) => PlaceScoutTower(TowerPlace10);
                    break;
                case 3:
                    TowerPlace10.Click += (s, e) => PlaceFlamerTower(TowerPlace10);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace10);
            GameGrid.Children.Add(TowerPlace10);

            Button TowerPlace11 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(250, 20, 0, 0)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace11.Click += (s, e) => PlaceSniperTower(TowerPlace11);
                    break;
                case 2:
                    TowerPlace11.Click += (s, e) => PlaceScoutTower(TowerPlace11);
                    break;
                case 3:
                    TowerPlace11.Click += (s, e) => PlaceFlamerTower(TowerPlace11);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace11);
            GameGrid.Children.Add(TowerPlace11);

            Button TowerPlace12 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(350, 20, 0, 0)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace12.Click += (s, e) => PlaceSniperTower(TowerPlace12);
                    break;
                case 2:
                    TowerPlace12.Click += (s, e) => PlaceScoutTower(TowerPlace12);
                    break;
                case 3:
                    TowerPlace12.Click += (s, e) => PlaceFlamerTower(TowerPlace12);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace12);
            GameGrid.Children.Add(TowerPlace12);

            Button TowerPlace13 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(450, 20, 0, 0)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace13.Click += (s, e) => PlaceSniperTower(TowerPlace13);
                    break;
                case 2:
                    TowerPlace13.Click += (s, e) => PlaceScoutTower(TowerPlace13);
                    break;
                case 3:
                    TowerPlace13.Click += (s, e) => PlaceFlamerTower(TowerPlace13);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace13);
            GameGrid.Children.Add(TowerPlace13);

            Button TowerPlace14 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(250, 220, 0, 0)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace14.Click += (s, e) => PlaceSniperTower(TowerPlace14);
                    break;
                case 2:
                    TowerPlace14.Click += (s, e) => PlaceScoutTower(TowerPlace14);
                    break;
                case 3:
                    TowerPlace14.Click += (s, e) => PlaceFlamerTower(TowerPlace14);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace14);
            GameGrid.Children.Add(TowerPlace14);

            Button TowerPlace15 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(350, 220, 0, 0)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace15.Click += (s, e) => PlaceSniperTower(TowerPlace15);
                    break;
                case 2:
                    TowerPlace15.Click += (s, e) => PlaceScoutTower(TowerPlace15);
                    break;
                case 3:
                    TowerPlace15.Click += (s, e) => PlaceFlamerTower(TowerPlace15);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace15);
            GameGrid.Children.Add(TowerPlace15);

            Button TowerPlace16 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(650, 120, 0, 0)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace16.Click += (s, e) => PlaceSniperTower(TowerPlace16);
                    break;
                case 2:
                    TowerPlace16.Click += (s, e) => PlaceScoutTower(TowerPlace16);
                    break;
                case 3:
                    TowerPlace16.Click += (s, e) => PlaceFlamerTower(TowerPlace16);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace16);
            GameGrid.Children.Add(TowerPlace16);

            Button TowerPlace17 = new Button
            {

                Content = "$",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Rockwell Extra Bold"),
                Width = 50,
                Height = 50,
                Opacity = 0.3,
                FontSize = 30,
                Foreground = Brushes.Green,
                BorderBrush = Brushes.DarkGreen,
                Margin = new Thickness(650, 0, 0, 75)
            };
            switch (TowerVariant)
            {
                case 1:
                    TowerPlace17.Click += (s, e) => PlaceSniperTower(TowerPlace17);
                    break;
                case 2:
                    TowerPlace17.Click += (s, e) => PlaceScoutTower(TowerPlace17);
                    break;
                case 3:
                    TowerPlace17.Click += (s, e) => PlaceFlamerTower(TowerPlace17);
                    break;
            }
            towerPlacementButtons.Add(TowerPlace17);
            GameGrid.Children.Add(TowerPlace17);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private List<TowerUnit> towers = new List<TowerUnit>();

        private void AttackEnemies(object sender, EventArgs e, TowerUnit Tower)
        {
            EnemyUnit targetEnemy = null;

            if (Tower.AOEStatus == false)
            {
                for (int i = 0; i < enemyUnits.Count; i++)
                {
                    var enemyUnit = enemyUnits[i];
                    double distance = GetDistance(Tower.TowerImage, enemyUnit.EnemyImage);

                    if (distance <= Tower.Distance)
                    {
                        targetEnemy = enemyUnit;
                        break;
                    }
                }

                if (targetEnemy != null)
                {
                    Line attackLine = new Line
                    {
                        X1 = Tower.TowerImage.TranslatePoint(new Point(25, 25), GameGrid).X,
                        Y1 = Tower.TowerImage.TranslatePoint(new Point(25, 25), GameGrid).Y,
                        X2 = targetEnemy.EnemyImage.TranslatePoint(new Point(25, 25), GameGrid).X,
                        Y2 = targetEnemy.EnemyImage.TranslatePoint(new Point(25, 25), GameGrid).Y,
                        Stroke = Brushes.Yellow,
                        StrokeThickness = 1.5
                    };
                    GameGrid.Children.Add(attackLine);

                    double healthBarWidthReduction;

                    if (Tower.SniperStatus == true)
                    {
                        ShotSoundPlayer.URL = SniperShot;
                    }
                    else
                    {
                        ShotSoundPlayer.URL = ScoutShot;
                    }

                    if (Tower.SniperStatus == true && targetEnemy.BossStatus == true)
                    {

                        targetEnemy.Health -= Tower.Damage * 2;


                        double damagePercentage = ((double)Tower.Damage * 2) / targetEnemy.MaxHealth;
                        healthBarWidthReduction = 50 * damagePercentage;
                    }
                    else
                    {
                        targetEnemy.Health -= Tower.Damage;


                        double damagePercentage = (double)Tower.Damage / targetEnemy.MaxHealth;
                        healthBarWidthReduction = 50 * damagePercentage;
                    }

                    if (targetEnemy.Health > 0)
                    {
                        targetEnemy.HealthBar.Width = Math.Max(0, targetEnemy.HealthBar.Width - healthBarWidthReduction);
                    }
                    else
                    {
                        GameGrid.Children.Remove(targetEnemy.EnemyImage);
                        GameGrid.Children.Remove(targetEnemy.HealthBar);
                        GameGrid.Children.Remove(targetEnemy.MaxHealthBar);
                        enemyUnits.Remove(targetEnemy);
                        Money += targetEnemy.MaxHealth / 5;
                        MoneyCount.Content = $"Рубли: {Money}";
                    }

                    Task.Delay(300).ContinueWith(_ =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            GameGrid.Children.Remove(attackLine);
                        });
                    });
                }
            }
            else
            {
                for (int i = 0; i < enemyUnits.Count; i++)
                {
                    var enemyUnit = enemyUnits[i];
                    double distance = GetDistance(Tower.TowerImage, enemyUnit.EnemyImage);

                    if (distance <= Tower.Distance)
                    {
                        Line attackLine = new Line
                        {
                            X1 = Tower.TowerImage.TranslatePoint(new Point(25, 25), GameGrid).X,
                            Y1 = Tower.TowerImage.TranslatePoint(new Point(25, 25), GameGrid).Y,
                            X2 = enemyUnit.EnemyImage.TranslatePoint(new Point(25, 25), GameGrid).X,
                            Y2 = enemyUnit.EnemyImage.TranslatePoint(new Point(25, 25), GameGrid).Y,
                            Stroke = Brushes.Red,
                            StrokeThickness = 2
                        };
                        GameGrid.Children.Add(attackLine);

                        ShotSoundPlayer.URL = FlamerShot;

                        enemyUnit.Health -= Tower.Damage;


                        double damagePercentage = (double)Tower.Damage / enemyUnit.MaxHealth;
                        double healthBarWidthReduction = 50 * damagePercentage;

                        if (enemyUnit.Health > 0)
                        {
                            enemyUnit.HealthBar.Width = Math.Max(0, enemyUnit.HealthBar.Width - healthBarWidthReduction);
                        }
                        else
                        {
                            GameGrid.Children.Remove(enemyUnit.EnemyImage);
                            GameGrid.Children.Remove(enemyUnit.HealthBar);
                            enemyUnits.Remove(enemyUnit);
                            Money += enemyUnit.MaxHealth / 8;
                            MoneyCount.Content = $"Рубли: {Money}";
                        }

                        Task.Delay(300).ContinueWith(_ =>
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                GameGrid.Children.Remove(attackLine);
                            });
                        });
                    }
                }
            }
        }


        private void PlaceScoutTower(Button button)
        {
            foreach (var child in GameGrid.Children)
            {
                if (child is Image towerImage)
                {
                    if (towerImage.Margin == button.Margin)
                    {
                        MessageBox.Show("Эта позиция уже занята!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }

            Image ScoutImage = new Image
            {
                Source = new BitmapImage(new Uri(Tower1Source)),
                Width = 50,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(button.Margin.Left, button.Margin.Top, button.Margin.Right, button.Margin.Bottom)
            };

            TowerUnit scoutTower = new TowerUnit(ScoutImage, 250, 10, 0.5, false, GameGrid, false);
            DispatcherTimer AttackTimer = new DispatcherTimer();
            AttackTimer.Interval = TimeSpan.FromSeconds(scoutTower.ShootSPD);
            AttackTimer.Tick += (sender, e) => AttackEnemies(sender, e, scoutTower);
            AttackTimer.Start();
            towers.Add(scoutTower);
            GameGrid.Children.Add(ScoutImage);
            StopTowerPlacement();
            Money -= 100;
            MoneyCount.Content = $"Рубли: {Money}";
        }

        private void PlaceSniperTower(Button button)
        {
            foreach (var child in GameGrid.Children)
            {
                if (child is Image towerImage)
                {
                    if (towerImage.Margin == button.Margin)
                    {
                        MessageBox.Show("Эта позиция уже занята!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }

            Image SniperImage = new Image
            {
                Source = new BitmapImage(new Uri(Tower3Source)),
                Width = 50,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(button.Margin.Left, button.Margin.Top, button.Margin.Right, button.Margin.Bottom)
            };

            TowerUnit sniperTower = new TowerUnit(SniperImage, 500, 70, 3, false, GameGrid, true);
            DispatcherTimer AttackTimer = new DispatcherTimer();
            AttackTimer.Interval = TimeSpan.FromSeconds(sniperTower.ShootSPD);
            AttackTimer.Tick += (sender, e) => AttackEnemies(sender, e, sniperTower);
            AttackTimer.Start();
            towers.Add(sniperTower);
            GameGrid.Children.Add(SniperImage);
            StopTowerPlacement();
            Money -= 300;
            MoneyCount.Content = $"Рубли: {Money}";
        }

        private void PlaceFlamerTower(Button button)
        {
            foreach (var child in GameGrid.Children)
            {
                if (child is Image towerImage)
                {
                    if (towerImage.Margin == button.Margin)
                    {
                        MessageBox.Show("Эта позиция уже занята!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }

            Image FlamerImage = new Image
            {
                Source = new BitmapImage(new Uri(Tower2Source)),
                Width = 50,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(button.Margin.Left, button.Margin.Top, button.Margin.Right, button.Margin.Bottom)
            };

            TowerUnit flamerTower = new TowerUnit(FlamerImage, 150, 5, 0.3, true, GameGrid, false);
            DispatcherTimer AttackTimer = new DispatcherTimer();
            AttackTimer.Interval = TimeSpan.FromSeconds(flamerTower.ShootSPD);
            AttackTimer.Tick += (sender, e) => AttackEnemies(sender, e, flamerTower);
            AttackTimer.Start();
            towers.Add(flamerTower);
            GameGrid.Children.Add(FlamerImage);
            StopTowerPlacement();
            Money -= 200;
            MoneyCount.Content = $"Рубли: {Money}";
        }


        private void StopTowerPlacement()
        {
            foreach (var button in towerPlacementButtons)
            {
                GameGrid.Children.Remove(button);
            }
            towerPlacementButtons.Clear();
        }

        private void ScoutSpawn(object sender, RoutedEventArgs e)
        {
            if (Money >= 100)
            {
                ShowTowerPlacementOptions(2);
            }
        }

        private void SniperSpawn(object sender, RoutedEventArgs e)
        {
            if (Money >= 300)
            {
                ShowTowerPlacementOptions(1);
            }
        }

        private void FlamerSpawn(object sender, RoutedEventArgs e)
        {
            if (Money >= 200)
            {
                ShowTowerPlacementOptions(3);
            }
        }


        private double GetDistance(UIElement element1, UIElement element2)
        {
            Point position1 = element1.TranslatePoint(new Point(0, 0), GameGrid);
            Point position2 = element2.TranslatePoint(new Point(0, 0), GameGrid);


            return Math.Sqrt(Math.Pow(position1.X - position2.X, 2) + Math.Pow(position1.Y - position2.Y, 2));
        }


    }
}
