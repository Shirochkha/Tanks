using System;
using System.Windows.Input;
using System.Windows;
using System.Drawing;
using System.Windows.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Text;

using Rectangle = System.Drawing.Rectangle;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

using GameEngine.Game;
using GameEngine.Input;
using GameEngine.Objects;

using GameLibrary.Factories;
using GameLibrary.Weapons;
using GameLibrary.Networks;
using GameLibrary.Decorators;
using GameLibrary.Converters;

using OpenTK.Graphics.OpenGL;
using Newtonsoft.Json;

namespace TanksDuel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Метод оповещения при изменении свойства
        /// </summary>
        /// <param name="prop"></param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        /// <summary>
        /// Событие изменения свойства
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Таймер
        /// </summary>
        private readonly DispatcherTimer _animationTimer;
        /// <summary>
        /// Игровое поле
        /// </summary>
        private GameField _gameField;
        /// <summary>
        /// Показывает начата ли игра
        /// </summary>
        private bool _gameStarted;
        /// <summary>
        /// Уровень игры
        /// </summary>
        private Level _objects;
        /// <summary>
        /// Танки
        /// </summary>
        private Tank player, enemy;

        /// <summary>
        /// Контроллеры управления игрока
        /// </summary>
        private KeyboardController _myController;
        /// <summary>
        /// Контроллер управления врага
        /// </summary>
        private KeyboardController _enemyController;

        /// <summary>
        /// Поток
        /// </summary>
        private Thread _thread;
        /// <summary>
        /// Порт
        /// </summary>
        private int port = 12345;

        /// <summary>
        /// Сокет клиента
        /// </summary>
        private Socket ClientSocket { get; set; }
        /// <summary>
        /// Свойства для привязки GUI к модели
        /// </summary>
        #region GUI Properties

        private string _ipAdressLabel;
        /// <summary>
        /// ip-адрес
        /// </summary>
        public string IpAdress
        {
            get { return _ipAdressLabel; }
            set
            {
                _ipAdressLabel = value;
                OnPropertyChanged();
            }
        }

        private string _portLabel;
        /// <summary>
        /// Порт
        /// </summary>
        public string Port
        {
            get { return _portLabel; }
            set
            {
                _portLabel = value;
                OnPropertyChanged();
            }
        }

        private string _result;
        /// <summary>
        /// Конец игры
        /// </summary>
        public string GameOver
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        private int _healthPlayerTank;
        /// <summary>
        /// Здоровье игрока
        /// </summary>
        public int HealthPlayer
        {
            get { return _healthPlayerTank; }
            set
            {
                _healthPlayerTank = value;
                OnPropertyChanged();
            }
        }

        private int _healthEnemyTank;
        /// <summary>
        /// Здоровье врага
        /// </summary>
        public int HealthEnemy
        {
            get { return _healthEnemyTank; }
            set
            {
                _healthEnemyTank = value;
                OnPropertyChanged();
            }
        }

        private int _armorPlayerTank;
        /// <summary>
        /// Броня игрока
        /// </summary>
        public int ArmorPlayer
        {
            get { return _armorPlayerTank; }
            set
            {
                _armorPlayerTank = value;
                OnPropertyChanged();
            }
        }

        private int _armorEnemyTank;
        /// <summary>
        /// Броня врага
        /// </summary>
        public int ArmorEnemy
        {
            get { return _armorEnemyTank; }
            set
            {
                _armorEnemyTank = value;
                OnPropertyChanged();
            }
        }

        private int _fuelPlayerTank;
        /// <summary>
        /// Топливо игрока
        /// </summary>
        public int FuelPlayer
        {
            get { return _fuelPlayerTank; }
            set
            {
                _fuelPlayerTank = value;
                OnPropertyChanged();
            }
        }

        private int _fuelEnemyTank;
        /// <summary>
        /// Топливо врага
        /// </summary>
        public int FuelEnemy
        {
            get { return _fuelEnemyTank; }
            set
            {
                _fuelEnemyTank = value;
                OnPropertyChanged();
            }
        }

        private int _bulletPlayerTank;
        /// <summary>
        /// Пули игрока
        /// </summary>
        public int BulletPlayer
        {
            get { return _bulletPlayerTank; }
            set
            {
                _bulletPlayerTank = value;
                OnPropertyChanged();
            }
        }

        private int _bulletEnemyTank;
        /// <summary>
        /// Пули врага
        /// </summary>
        public int BulletEnemy
        {
            get { return _bulletEnemyTank; }
            set
            {
                _bulletEnemyTank = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _animationTimer = new DispatcherTimer(DispatcherPriority.ContextIdle, Dispatcher)
            {
                Interval = new TimeSpan(0, 0, 0, 0, 33)
            };

            TextureRepository.Add("playerTank", TextureCreator.CreatePlayerTank());
            TextureRepository.Add("enemyTank", TextureCreator.CreateEnemyTank());
            TextureRepository.Add("explosion", TextureCreator.CreateExplosion());
            TextureRepository.Add("bullet", TextureCreator.CreateAmmo());
            TextureRepository.Add("background", TextureCreator.CreateBackground());
            TextureRepository.Add("outerWall", TextureCreator.CreateOuterWall());
            TextureRepository.Add("interWall", TextureCreator.CreateInterWall());
            TextureRepository.Add("dirt", TextureCreator.CreateShallow());

            TextureRepository.Add("speedBonus", TextureCreator.CreateSpeedBonus());
            TextureRepository.Add("armorBonus", TextureCreator.CreateArmorBonus());
            TextureRepository.Add("damageBonus", TextureCreator.CreateDamageBonus());
            TextureRepository.Add("fuelBonus", TextureCreator.CreateFuelBonus());
            TextureRepository.Add("addBulletsBonus", TextureCreator.CreateNewBulletsBonus());

            int[] outerWall = TextureRepository.Get("outerWall");
            int[] interWall = TextureRepository.Get("interWall");

            _objects = new Level()
            {
                new Wall(outerWall){Location = new Point(25, 25)},
                new Wall(outerWall){Location = new Point(25,75)},
                new Wall(outerWall){Location = new Point(25,125)},
                new Wall(outerWall){Location = new Point(25,175)},
                new Wall(outerWall){Location = new Point(25,225)},
                new Wall(outerWall){Location = new Point(25,275)},
                new Wall(outerWall){Location = new Point(25,325)},
                new Wall(outerWall){Location = new Point(25,375)},
                new Wall(outerWall){Location = new Point(25,425)},
                new Wall(outerWall){Location = new Point(25,475)},
                new Wall(outerWall){Location = new Point(25,525)},
                new Wall(outerWall){Location = new Point(25,575)},
                new Wall(outerWall){Location = new Point(25,625)},
                new Wall(outerWall){Location = new Point(25,675)},
                new Wall(outerWall){Location = new Point(25,725)},
                new Wall(outerWall){Location = new Point(25,775)},
                new Wall(outerWall){Location = new Point(25,825)},
                new Wall(outerWall){Location = new Point(25,875)},
                new Wall(outerWall){Location = new Point(25,925)},
                new Wall(outerWall){Location = new Point(25,975)},
                new Wall(outerWall){Location = new Point(975,75)},
                new Wall(outerWall){Location = new Point(975,125)},
                new Wall(outerWall){Location = new Point(975,175)},
                new Wall(outerWall){Location = new Point(975,225)},
                new Wall(outerWall){Location = new Point(975,275)},
                new Wall(outerWall){Location = new Point(975,325)},
                new Wall(outerWall){Location = new Point(975,375)},
                new Wall(outerWall){Location = new Point(975,425)},
                new Wall(outerWall){Location = new Point(975,475)},
                new Wall(outerWall){Location = new Point(975,525)},
                new Wall(outerWall){Location = new Point(975,575)},
                new Wall(outerWall){Location = new Point(975,625)},
                new Wall(outerWall){Location = new Point(975,675)},
                new Wall(outerWall){Location = new Point(975,725)},
                new Wall(outerWall){Location = new Point(975,775)},
                new Wall(outerWall){Location = new Point(975,825)},
                new Wall(outerWall){Location = new Point(975,875)},
                new Wall(outerWall){Location = new Point(975,925)},
                new Wall(outerWall){Location = new Point(975,975)},
                new Wall(outerWall){Location = new Point(75,25)},
                new Wall(outerWall){Location = new Point(125,25)},
                new Wall(outerWall){Location = new Point(175,25)},
                new Wall(outerWall){Location = new Point(225,25)},
                new Wall(outerWall){Location = new Point(275,25)},
                new Wall(outerWall){Location = new Point(325,25)},
                new Wall(outerWall){Location = new Point(375,25)},
                new Wall(outerWall){Location = new Point(425,25)},
                new Wall(outerWall){Location = new Point(475,25)},
                new Wall(outerWall){Location = new Point(525,25)},
                new Wall(outerWall){Location = new Point(575,25)},
                new Wall(outerWall){Location = new Point(625,25)},
                new Wall(outerWall){Location = new Point(675,25)},
                new Wall(outerWall){Location = new Point(725,25)},
                new Wall(outerWall){Location = new Point(775,25)},
                new Wall(outerWall){Location = new Point(825,25)},
                new Wall(outerWall){Location = new Point(875,25)},
                new Wall(outerWall){Location = new Point(925,25)},
                new Wall(outerWall){Location = new Point(975,25)},
                new Wall(outerWall){Location = new Point(75,975)},
                new Wall(outerWall){Location = new Point(125,975)},
                new Wall(outerWall){Location = new Point(175,975)},
                new Wall(outerWall){Location = new Point(225,975)},
                new Wall(outerWall){Location = new Point(275,975)},
                new Wall(outerWall){Location = new Point(325,975)},
                new Wall(outerWall){Location = new Point(375,975)},
                new Wall(outerWall){Location = new Point(425,975)},
                new Wall(outerWall){Location = new Point(475,975)},
                new Wall(outerWall){Location = new Point(525,975)},
                new Wall(outerWall){Location = new Point(575,975)},
                new Wall(outerWall){Location = new Point(625,975)},
                new Wall(outerWall){Location = new Point(675,975)},
                new Wall(outerWall){Location = new Point(725,975)},
                new Wall(outerWall){Location = new Point(775,975)},
                new Wall(outerWall){Location = new Point(825,975)},
                new Wall(outerWall){Location = new Point(875,975)},
                new Wall(outerWall){Location = new Point(925,975)},

                new Wall(interWall){Location = new Point(75,150)},
                new Wall(interWall){Location = new Point(125,150)},
                new Wall(interWall){Location = new Point(175,150)},
                new Wall(interWall){Location = new Point(225,150)},
                new Wall(interWall){Location = new Point(275,150)},
                new Wall(interWall){Location = new Point(325,150)},
                new Wall(interWall){Location = new Point(375,150)},
                new Wall(interWall){Location = new Point(425,150)},
                new Wall(interWall){Location = new Point(475,150)},
                new Wall(interWall){Location = new Point(475,200)},
                new Wall(interWall){Location = new Point(475,250)},
                new Wall(interWall){Location = new Point(475,300)},
                new Wall(interWall){Location = new Point(475,350)},
                new Wall(interWall){Location = new Point(475,400)},
                new Wall(interWall){Location = new Point(475,450)},
                new Wall(interWall){Location = new Point(925,300)},
                new Wall(interWall){Location = new Point(875,300)},
                new Wall(interWall){Location = new Point(825,300)},
                new Wall(interWall){Location = new Point(775,300)},
                new Wall(interWall){Location = new Point(725,300)},
                new Wall(interWall){Location = new Point(675,300)},
                new Wall(interWall){Location = new Point(625,300)},
                new Wall(interWall){Location = new Point(625,350)},
                new Wall(interWall){Location = new Point(625,400)},
                new Wall(interWall){Location = new Point(625,450)},
                new Wall(interWall){Location = new Point(625,500)},
                new Wall(interWall){Location = new Point(625,550)},
                new Wall(interWall){Location = new Point(625,600)},
                new Wall(interWall){Location = new Point(575,600)},
                new Wall(interWall){Location = new Point(525,600)},
                new Wall(interWall){Location = new Point(475,600)},
                new Wall(interWall){Location = new Point(425,600)},
                new Wall(interWall){Location = new Point(375,600)},
                new Wall(interWall){Location = new Point(325,600)},
                new Wall(interWall){Location = new Point(275,600)},
                new Wall(interWall){Location = new Point(225,600)},
                new Wall(interWall){Location = new Point(175,600)},
                new Wall(interWall){Location = new Point(175,650)},
                new Wall(interWall){Location = new Point(175,700)},
                new Wall(interWall){Location = new Point(175,750)},
                new Wall(interWall){Location = new Point(175,800)},
                new Wall(interWall){Location = new Point(800,925)},
                new Wall(interWall){Location = new Point(800,875)},
                new Wall(interWall){Location = new Point(800,825)},

                new Dirt(){Location = new Point(375,925)},
                new Dirt(){Location = new Point(425,925)},
                new Dirt(){Location = new Point(475,925)},
                new Dirt(){Location = new Point(525,925)},
                new Dirt(){Location = new Point(575,925)},
                new Dirt(){Location = new Point(375,875)},
                new Dirt(){Location = new Point(425,875)},
                new Dirt(){Location = new Point(475,875)},
                new Dirt(){Location = new Point(525,875)},
                new Dirt(){Location = new Point(575,875)},
                new Dirt(){Location = new Point(375,825)},
                new Dirt(){Location = new Point(425,825)},
                new Dirt(){Location = new Point(475,825)},
                new Dirt(){Location = new Point(525,825)},
                new Dirt(){Location = new Point(575,825)},
                new Dirt(){Location = new Point(75,325)},
                new Dirt(){Location = new Point(75,375)},
                new Dirt(){Location = new Point(75,425)},
                new Dirt(){Location = new Point(75,475)},
                new Dirt(){Location = new Point(75,525)},
                new Dirt(){Location = new Point(125,275)},
                new Dirt(){Location = new Point(125,325)},
                new Dirt(){Location = new Point(125,375)},
                new Dirt(){Location = new Point(125,425)},
                new Dirt(){Location = new Point(125,475)},
                new Dirt(){Location = new Point(300,300)},
                new Dirt(){Location = new Point(525,325)},
                new Dirt(){Location = new Point(575,325)},
                new Dirt(){Location = new Point(725,375)},
                new Dirt(){Location = new Point(725,425)},
                new Dirt(){Location = new Point(725,475)},
                new Dirt(){Location = new Point(725,525)},
                new Dirt(){Location = new Point(725,575)},
                new Dirt(){Location = new Point(725,625)},
            };
        }

        /// <summary>
        /// Обработчики изменения танка игрока
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerTank_Changed(object sender, EventArgs e)
        {
            Tank player = (Tank)sender;
            HealthPlayer = player.Health;
            ArmorPlayer = player.Armor;
            FuelPlayer = player.Fuel;
            BulletPlayer = player.Ammunition;
        }

        /// <summary>
        /// Обработчики изменения танка врага
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnemyTank_Changed(object sender, EventArgs e)
        {
            Tank enemy = (Tank)sender;
            HealthEnemy = enemy.Health;
            ArmorEnemy = enemy.Armor;
            FuelEnemy = enemy.Fuel;
            BulletEnemy = enemy.Ammunition;
        }

        /// <summary>
        /// Обработчик нажатия клавиши на клавиатуре
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (!_gameStarted)
                return;

            if (_myController != null)
                HandlePlayerInput(_myController, e);
        }

        /// <summary>
        /// Помощник обработчика ввода клавиш
        /// </summary>
        private void HandlePlayerInput(KeyboardController controller, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    controller.StartMove(Direction.Up);
                    break;
                case Key.S:
                    controller.StartMove(Direction.Down);
                    break;
                case Key.A:
                    controller.StartMove(Direction.Left);
                    break;
                case Key.D:
                    controller.StartMove(Direction.Right);
                    break;

                case Key.Space:
                    controller.Shoot();
                    break;
            }
        }

        /// <summary>
        /// Обработчик отпускания клавиши на клавиатуре
        /// </summary>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (!_gameStarted)
                return;

            switch (e.Key)
            {
                case Key.W:
                case Key.S:
                case Key.A:
                case Key.D:
                    if (_myController != null)
                        _myController.StopMove();
                        
                    break;
            }
        }

        /// <summary>
        /// Обработчик тика таймера
        /// </summary>
        private void OnAnimationTick(object sender, EventArgs e)
        {
            if (_gameField.GameStatus != GameStatus.InGame)
            {
                _gameStarted = false;
                _animationTimer.Stop();
                _animationTimer.Tick -= OnAnimationTick;
                switch (_gameField.GameStatus)
                {
                    case GameStatus.PlayerWins:
                        GameOver = "Вы одержали победу!";
                        break;
                    case GameStatus.EnemyWins:
                        GameOver = "Вы проиграли :(";
                        break;
                    case GameStatus.Standoff:
                        GameOver = "Победила дружба =)";
                        break;
                }
                GameScreen.Visibility = Visibility.Hidden;
                EndGameScreen.Visibility = Visibility.Visible;
            }

            renderCanvas.Invalidate();
        }

        /// <summary>
        /// Обработчик инициализации хоста элементов WinForms
        /// </summary>
        private void WindowsFormsHost_Initialized(object sender, EventArgs e)
        {
            renderCanvas.MakeCurrent();
        }

        /// <summary>
        /// Обработчик события загрузки холста
        /// </summary>
        private void renderCanvas_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.White);

            GL.Viewport(0, 0, renderCanvas.Width, renderCanvas.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(0, 1000, 1000, 0, 0.0, 1.0);
            GL.Enable(EnableCap.Texture2D);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        /// <summary>
        /// Обработчик события рисования холста
        /// </summary>
        private void renderCanvas_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            int x = (renderCanvas.Width - 800) / 2;
            int y = (renderCanvas.Height - 800) / 2;

            GL.Viewport(x, y, 800, 800);

            if (_gameField != null)
                _gameField.Draw();

            GL.Flush();
            renderCanvas.SwapBuffers();
        }

        /// <summary>
        /// Обработчик события изменения танка игрока
        /// </summary>
        private void GameField_PlayerChanged(object sender, EventArgs e)
        {
            _gameField.PlayerTank.Changed += PlayerTank_Changed;
        }
        /// <summary>
        /// Обработчик события изменения танка врага
        /// </summary>
        private void GameField_EnemyChanged(object sender, EventArgs e)
        {
            _gameField.EnemyTank.Changed += EnemyTank_Changed;
        }

        /// <summary>
        /// Точка входа в игру
        /// </summary>
        private void StartPoint()
        {
            StartScreen.Visibility = Visibility.Hidden;

            _myController = new KeyboardController(_gameField);
            _enemyController = new KeyboardController(_gameField);

            player = new NormalTank(_myController, TextureRepository.Get("playerTank"));
            enemy = new NormalTank(_enemyController, TextureRepository.Get("enemyTank"));

            player.Changed += PlayerTank_Changed;
            player.Weapons = new BulletWeapon(player);

            enemy.Changed += EnemyTank_Changed;
            enemy.Weapons = new BulletWeapon(enemy);

            _gameField = new GameField(_objects, player, enemy);
            _gameField.PlayerTankChanged += GameField_PlayerChanged;
            _gameField.EnemyTankChanged += GameField_EnemyChanged;

            _myController.GameField = _gameField;
            _enemyController.GameField = _gameField;

            HealthPlayer = player.Health;
            HealthEnemy = enemy.Health;
            ArmorPlayer = player.Armor;
            ArmorEnemy = enemy.Armor;
            FuelPlayer = player.Fuel;
            FuelEnemy = enemy.Fuel;
            BulletPlayer = player.Ammunition;
            BulletEnemy = enemy.Ammunition;

            GameScreen.Visibility = Visibility.Visible;

            _animationTimer.Tick += OnAnimationTick;
            _animationTimer.Start();

            _gameStarted = true;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "выход"
        /// </summary>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Вернуться в меню"
        /// </summary>
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            EndGameScreen.Visibility = Visibility.Hidden;
            StartScreen.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Отмена"
        /// </summary>
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            InputServerScreen.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Создать сервер"
        /// </summary>
        private async void btnStartServer_Click(object sender, RoutedEventArgs e)
        {
            //const string HOST = "127.0.0.1";
             string host = GetLocalIPv4();

            if (IsPortAvailable(host, port))
            {
                var endpoint = new IPEndPoint(IPAddress.Parse(host), port);
                Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    serverSocket.Bind(endpoint);
                    serverSocket.Listen(10);

                    Debug.WriteLine("Сервер запущен");

                    IpAdress = GetLocalIPv4();
                    Port = port.ToString();
                    AdressServerScreen.Visibility = Visibility.Visible;

                    while (true)
                    {
                        var clientSocket = await AcceptClientAsync(serverSocket);
                        ClientSocket = clientSocket;

                        AdressServerScreen.Visibility = Visibility.Hidden;

                        StartPoint();
                        player.Location = new Point(900, 900);
                        enemy.Location = new Point(95, 95);

                        var playerPosJson = $"{enemy.Location.X} {enemy.Location.Y}";
                        var playerPositionMsg = new Message(MessageType.Location, playerPosJson);
                        using (NetworkStream networkStream = new NetworkStream(ClientSocket))
                        {
                            var messageBytes = Encoding.UTF8.GetBytes(playerPositionMsg.ToString() + "*");
                            networkStream.Write(messageBytes, 0, messageBytes.Length);
                        }

                        var enemyPosJson = $"{player.Location.X} {player.Location.Y}";
                        var enemyPositionMsg = new Message(MessageType.Location, enemyPosJson);
                        using (NetworkStream networkStream = new NetworkStream(ClientSocket))
                        {
                            var messageBytes = Encoding.UTF8.GetBytes(enemyPositionMsg.ToString() + "*");
                            networkStream.Write(messageBytes, 0, messageBytes.Length);
                        }

                        new ControllerReactor(_myController, ClientSocket);

                        _thread = new Thread(() => HandleReceivingClient(ClientSocket));
                        _thread.Start();

                        _ = Task.Run(BonusSpawner);
                    }
                }
                catch (SocketException ex)
                {
                    Debug.WriteLine($"Ошибка создания сервера: {ex.Message}");
                    MessageBox.Show($"Сервер не был создан", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Debug.WriteLine($"Порт {port} занят.");
                port++;
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Подключиться к серверу"
        /// </summary>
        private void btnStartClient_Click(object sender, RoutedEventArgs e)
        {
            InputServerScreen.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Подключиться"
        /// </summary>
        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            string host = IpAdressText.Text;

            if (!IPAddress.TryParse(host, out _))
            {
                MessageBox.Show("Неправильный формат ip", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(PortText.Text, out int port) || port < 0 || port > 65535)
            {
                MessageBox.Show("Неправильный порт", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var endpoint = new IPEndPoint(IPAddress.Parse(host), port);
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ClientSocket.Connect(endpoint);

                Debug.WriteLine("Подключение к серверу " + ClientSocket.Handle);
                InputServerScreen.Visibility = Visibility.Hidden;

                StartPoint();

                new ControllerReactor(_myController, ClientSocket);

                _thread = new Thread(() => GetUsersPositions(ClientSocket));
                _thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение позиции танков от сервера
        /// </summary>
        private void GetUsersPositions(Socket client)
        {
            Socket clientSocket = client;

            byte[] buffer = new byte[1024 * 8];
            int bytesRead = clientSocket.Receive(buffer);
            string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            var receivedMessage = Message.FromJson(dataReceived).First();

            if (receivedMessage?.Type == MessageType.Location)
            {
                var point = receivedMessage.Content.Split();
                player.Location = new Point(int.Parse(point[0]), int.Parse(point[1]));
                Debug.WriteLine($"Получены данные: {dataReceived}");
            }

            buffer = new byte[1024 * 8];
            bytesRead = clientSocket.Receive(buffer);
            dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            receivedMessage = Message.FromJson(dataReceived).First();
            if (receivedMessage?.Type == MessageType.Location)
            {
                var point = receivedMessage.Content.Split();
                enemy.Location = new Point(int.Parse(point[0]), int.Parse(point[1]));
                Debug.WriteLine($"Получены данные: {dataReceived}");
            }
            HandleReceivingClient(clientSocket);
        }

        /// <summary>
        /// Получение ip-адреса
        /// </summary>
        static string GetLocalIPv4()
        {
            try
            {
                string hostName = Dns.GetHostName();
                IPAddress[] addresses = Dns.GetHostAddresses(hostName);

                foreach (IPAddress address in addresses)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return address.ToString();
                    }
                }

                return "IPv4 адрес не найден";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка получения IPv4 адреса: {ex.Message}");
                return "Ошибка";
            }
        }

        /// <summary>
        /// Получение данных
        /// </summary>
        private void HandleReceivingClient(Socket client)
        {
            Socket clientSocket = client;
            byte[] buffer = new byte[1024 * 8];
            while (true)
            {
                if (clientSocket != null)
                {
                    int bytesRead = clientSocket.Receive(buffer);
                    if (bytesRead > 0)
                    {
                        var dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        var receivedMessages = Message.FromJson(dataReceived);
                        foreach (var receivedMessage in receivedMessages)
                        {
                            if (receivedMessage == null) continue;
                            switch (receivedMessage.Type)
                            {
                                case MessageType.Location:
                                    var point = receivedMessage.Content.Split();
                                    enemy.Location = new Point(int.Parse(point[0]), int.Parse(point[1]));
                                    Debug.WriteLine($"Получены данные: {dataReceived}");
                                    break;
                                case MessageType.StartMoving:
                                    var receivedData = receivedMessage.Content.Split(';');
                                    var receivedLocationString = receivedData[0].Trim('{', '}');
                                    var receivedLocationParts = receivedLocationString.Split(',');
                                    var receivedX = int.Parse(receivedLocationParts[0].Split('=')[1].Trim());
                                    var receivedY = int.Parse(receivedLocationParts[1].Split('=')[1].Trim());
                                    var receivedDirection = (Direction)Enum.Parse(typeof(Direction), receivedData[1]);
                                    _enemyController.CurrentDirection = receivedDirection;
                                    enemy._controller_StartMoving(null, null);
                                    if (_enemyController.Tank.Location != new Point(receivedX, receivedY))
                                        _enemyController.Tank.Location = new Point(receivedX, receivedY);
                                    Debug.WriteLine("Враг начал двиsжение");
                                    break;
                                case MessageType.StopMoving:
                                    enemy._controller_StopMoving(null, null);
                                    Debug.WriteLine($"Враг закончил движение");
                                    break;
                                case MessageType.Shooting:
                                    enemy._controller_Shooting(null, null);
                                    Debug.WriteLine($"Враг стреляет");
                                    break;
                                case MessageType.Bonus:
                                    var bonus = JsonConvert.DeserializeObject<Bonus>(receivedMessage.Content, new JsonSerializerSettings
                                    {
                                        Converters = new List<JsonConverter> { new BonusConverter() }
                                    });
                                    bonus.GameField = _gameField;
                                    _gameField.Bonuses.Add(bonus);
                                    Debug.WriteLine($"Бонус заспавнился");
                                    break;
                                default:
                                    Debug.WriteLine($"Неверный тип сообщения: {receivedMessage.Type}");
                                    break;
                            }
                        }

                    }
                }

            }

        }

        /// <summary>
        /// Асинхронно жду клиента
        /// </summary>
        private Task<Socket> AcceptClientAsync(Socket serverSocket)
        {
            return Task.Factory.FromAsync(
                (callback, state) => serverSocket.BeginAccept(callback, state),
                asyncResult => serverSocket.EndAccept(asyncResult),
                null);
        }

        /// <summary>
        /// Проверка не занят ли порт
        /// </summary>
        private bool IsPortAvailable(string host, int port)
        {
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Bind(new IPEndPoint(IPAddress.Parse(host), port));
                    return true;
                }
            }
            catch (SocketException)
            {
                return false;
            }
        }

        /// <summary>
        /// Спавнер бонусов
        /// </summary>
        private void BonusSpawner()
        {
            Thread.Sleep(TimeSpan.FromSeconds(5));

            const int bonusSpawnDelaySecs = 10;
            var factories = new List<BaseBonusFactory>
            {
                new AmmunitionBonusFactory(_gameField, TextureRepository.Get("addBulletsBonus")),
                new ArmorBonusFactory(_gameField, TextureRepository.Get("armorBonus")),
                new DamageBonusFactory(_gameField, TextureRepository.Get("damageBonus")),
                new FuelBonusFactory(_gameField, TextureRepository.Get("fuelBonus")),
                new SpeedBonusFactory(_gameField, TextureRepository.Get("speedBonus")),
            };

            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(bonusSpawnDelaySecs));
                Point randomPosition;
                do
                {
                    randomPosition = GetRandomPosition();
                }
                while (IntersectsWithWalls(randomPosition));

                var randomFactory = factories[new Random((int)DateTime.UtcNow.Ticks).Next(factories.Count)];
                var randomBonus = randomFactory.Spawn(randomPosition);
                _gameField.Bonuses.Add(randomBonus);

                try
                {
                    var json = JsonConvert.SerializeObject(randomBonus, new JsonSerializerSettings
                    {
                        MaxDepth = 1,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    var msg = new Message(MessageType.Bonus, json);

                    using (NetworkStream networkStream = new NetworkStream(ClientSocket))
                    {
                        var messageBytes = Encoding.UTF8.GetBytes(msg.ToString() + "*");
                        networkStream.Write(messageBytes, 0, messageBytes.Length);
                    }
                    Debug.WriteLine("Бонус создан");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            }

            Point GetRandomPosition()
            {
                Random random = new Random();

                var randomX = random.Next(_gameField.ViewportSize.Width);
                var randomY = random.Next(_gameField.ViewportSize.Height);

                var randomPosition = new Point(randomX, randomY);
                return randomPosition;
            }


            bool IntersectsWithWalls(Point randomPosition)
            {
                var bonusRect = new Rectangle(randomPosition, new Size(50, 50));
                return _gameField.Walls.Any(obj => obj.Bounds.IntersectsWith(bonusRect));
            }
        }
    }
}
