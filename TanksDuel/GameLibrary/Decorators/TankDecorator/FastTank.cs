using GameEngine.Input;
using GameEngine.Objects;
using GameLibrary.Bonuses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;

namespace GameLibrary.Decorators
{
    /// <summary>
    /// Класс быстрого танка
    /// </summary>
    public class FastTank : TankDecorator
    {
        private const int _boostValue = 10;
        private const int _boostDurationSeconds = 5;

        private readonly DateTime _endOfBoost;
        private readonly Timer _boostTimer;

        /// <summary>
        /// Конструктор класса быстрого танка
        /// </summary>
        public FastTank(NormalTank tank) : base(tank)
        {
            _endOfBoost = DateTime.UtcNow.AddSeconds(_boostDurationSeconds);

            _boostTimer = new Timer(1000);
            _boostTimer.Elapsed += BoostTimer_Elapsed;
            _boostTimer.Start();
        }

        public override Point Location
        {
            get => tank.Location;
            set => tank.Location = value;
        }

        public override int Speed => tank.Speed + _boostValue;

        protected override void Move()
        {
            if (!IsMoving || Fuel <= 0)
                return;

            this.OnChanged();

            Direction _allowedDirections = FindAllowedDirections();
            if (!_allowedDirections.HasFlag(Controller.CurrentDirection))
            {
                IsMoving = false;
                return;
            }

            switch (Controller.CurrentDirection)
            {
                case Direction.Up:
                    Location = new Point(Location.X, Location.Y - Speed);
                    break;
                case Direction.Down:
                    Location = new Point(Location.X, Location.Y + Speed);
                    break;
                case Direction.Left:
                    Location = new Point(Location.X - Speed, Location.Y);
                    break;
                case Direction.Right:
                    Location = new Point(Location.X + Speed, Location.Y);
                    break;
            }

            if (EndOfBoost)
            {
                if (GameField.PlayerTank == this)
                    GameField.PlayerTank = base.tank;
                else
                    GameField.EnemyTank = base.tank;
            }

            if (GotBonus())
            {
                var playerTank = GameField.PlayerTank == this ? GameField.PlayerTank : GameField.EnemyTank;
                var bonusesCopy = new List<Bonus>(GameField.Bonuses);

                foreach (var bonus in bonusesCopy.Where(obj => obj.Bounds.IntersectsWith(Bounds)))
                {
                    if (bonus is AmmunitionBonus)
                    {
                        playerTank = new BulletTank(tank);
                    }
                    else if (bonus is ArmorBonus)
                    {
                        playerTank = new ArmorTank(tank);
                    }
                    else if (bonus is DamageBonus)
                    {
                        playerTank = new DamageBoostTank(tank);
                    }
                    else if (bonus is FuelBonus)
                    {
                        playerTank = new FuelTank(tank);
                    }
                    else if (bonus is SpeedBonus)
                    {
                        playerTank = new FastTank(tank);
                    }

                    GameField.Bonuses.Remove(bonus);
                }
            }
        }

        private bool EndOfBoost => DateTime.UtcNow > _endOfBoost;
        private void BoostTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (EndOfBoost)
            {
                RemoveBoost();
            }
        }

        private void RemoveBoost()
        {
            if (GameField.PlayerTank == this)
            {
                GameField.PlayerTank = base.tank;
                GameField.PlayerTank.OnChanged();
            }
            else
            {
                GameField.EnemyTank = base.tank;
                GameField.EnemyTank.OnChanged();
            }

            _boostTimer.Stop();
        }
    }
}
