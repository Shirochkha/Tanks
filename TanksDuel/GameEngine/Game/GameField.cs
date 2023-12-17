using GameEngine.Objects;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameEngine.Game
{
    /// <summary>
    /// Класс игрового поля
    /// </summary>
    public class GameField
    {
        /// <summary>
        /// Текущий уровень
        /// </summary>
        private Level _currentLevel;
        /// <summary>
        /// Текстура заднего фона
        /// </summary>
        private int _background_texture;
        /// <summary>
        /// Размеры вьюпорта
        /// </summary>
        public Size ViewportSize { get; }
        /// <summary>
        /// Статус игры
        /// </summary>
        public GameStatus GameStatus
        {
            get
            {
                if ((_playerTank.Health > 0 && _enemyTank.Health > 0 &&
                     _playerTank.Ammunition <= 0 && _enemyTank.Ammunition <= 0 &&
                     Shots.Count() == 0) ||
                     (_playerTank.Health <= 0 && _enemyTank.Health <= 0) ||
                     (_playerTank.Health > 0 && _enemyTank.Health > 0 && _playerTank.Fuel <= 0 && _enemyTank.Fuel <= 0))
                    return GameStatus.Standoff;

                if (_playerTank.Health <= 0 && _enemyTank.Health > 0 || (_playerTank.Ammunition <= 0 && Shots.Count() == 0))
                    return GameStatus.EnemyWins;

                if (_playerTank.Health > 0 && _enemyTank.Health <= 0 || (_enemyTank.Ammunition <= 0 && Shots.Count() == 0))
                    return GameStatus.PlayerWins;

                return GameStatus.InGame;
            }
        }
        /// <summary>
        /// Танк игрока
        /// </summary>
        private Tank _playerTank;
        /// <summary>
        /// Танк врага
        /// </summary>
        private Tank _enemyTank;

        /// <summary>
        /// Событие игрока
        /// </summary>
        public event EventHandler PlayerTankChanged;
        /// <summary>
        /// Событие врага
        /// </summary>
        public event EventHandler EnemyTankChanged;

        /// <summary>
        /// Свойтво танка игрока
        /// </summary>
        public Tank PlayerTank
        {
            get => _playerTank;
            set
            {
                _playerTank = value;
                _playerTank.Weapons.Parent = _playerTank;
                PlayerTankChanged?.Invoke(_playerTank, null);
            }
        }
        /// <summary>
        /// Свойтво танка врага
        /// </summary>
        public Tank EnemyTank
        {
            get => _enemyTank;
            set
            {
                _enemyTank = value;
                _enemyTank.Weapons.Parent = _enemyTank;
                EnemyTankChanged?.Invoke(_enemyTank, null);
            }
        }

        /// <summary>
        /// Коллекция всех выстрелов
        /// </summary>
        public List<GameObject> Shots { get; }

        /// <summary>
        /// Коллекция всех бонусов
        /// </summary>
        public List<Bonus> Bonuses { get; } = new List<Bonus>();

        /// <summary>
        /// Коллекция стен
        /// </summary>
        public IEnumerable<GameObject> Walls =>
            _currentLevel.OfType<Wall>()
                    .Union(new List<GameObject>() { _playerTank, _enemyTank });

        /// <summary>
        /// Коллекция танков
        /// </summary>
        public IEnumerable<Tank> Tanks => new List<Tank>() { _playerTank, _enemyTank };


        /// <summary>
        /// Коллекция полей грязи
        /// </summary>
        public IEnumerable<Dirt> Dirts => _currentLevel.OfType<Dirt>();

        /// <summary>
        /// Конструктор класса игрового поля
        /// </summary>
        public GameField(Level level, Tank playerTank, Tank enemyTank)
        {
            ViewportSize = new Size(1000, 1000);
            _currentLevel = level;

            Shots = new List<GameObject>();

            _playerTank = playerTank;
            _enemyTank = enemyTank;

            _background_texture = TextureRepository.Get("background")[0];
        }

        /// <summary>
        /// Пустой конструктор
        /// </summary>
        public GameField()
        {
        }

        /// <summary>
        /// Функция рисования игрового поля
        /// </summary>
        public void Draw()
        {
            DrawBackground();

            foreach (var objects in _currentLevel)
                objects.Draw();

            var _shots = Shots.ToArray();
            foreach (var shot in _shots)
                shot.Draw();

            _playerTank.Draw();
            _enemyTank.Draw();

            for (int i = 0; i < Bonuses.Count; i++)
            {
                Bonuses[i].Draw();
            }
        }

        /// <summary>
        /// Функция отрисовки заднего фона
        /// </summary>
        private void DrawBackground()
        {
            GL.BindTexture(TextureTarget.Texture2D, _background_texture);

            GL.Begin(BeginMode.Polygon);

            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);

            GL.TexCoord2(1, 0);
            GL.Vertex2(ViewportSize.Width, 0);

            GL.TexCoord2(1, 1);
            GL.Vertex2(ViewportSize.Width, ViewportSize.Height);

            GL.TexCoord2(0, 1);
            GL.Vertex2(0, ViewportSize.Height);

            GL.End();
        }
    }
}
