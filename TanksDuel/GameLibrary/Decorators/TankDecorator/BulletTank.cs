using GameEngine.Input;
using GameLibrary.Bonuses;
using System.Drawing;
using System.Linq;

namespace GameLibrary.Decorators
{
    /// <summary>
    /// Класс танка с дополнительными патронами
    /// </summary>
    public class BulletTank : TankDecorator
    {
        private const int _boostValue = 10;
        private const int _maxBulletValue = 20;

        /// <summary>
        /// Конструктор класса танка с дополнительными патронами
        /// </summary>
        public BulletTank(NormalTank tank) : base(tank) 
        {
            tank.Ammunition += _boostValue;
        }

        public override Point Location
        {
            get => tank.Location;
            set => tank.Location = value;
        }

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

            Fuel -= _fuelLoss;

            if (GotBonus())
            {
                var playerTank = GameField.PlayerTank == this ? GameField.PlayerTank : GameField.EnemyTank;
                var bonus = GameField.Bonuses.FirstOrDefault(obj => obj.Bounds.IntersectsWith(Bounds));
                if (bonus is AmmunitionBonus)
                {
                    playerTank = new BulletTank(tank);
                    GameField.Bonuses.Remove(bonus);
                    return;
                }

                if (bonus is ArmorBonus)
                {
                    playerTank = new ArmorTank(tank);
                    GameField.Bonuses.Remove(bonus);
                    return;
                }

                if (bonus is DamageBonus)
                {
                    playerTank = new DamageBoostTank(tank);
                    GameField.Bonuses.Remove(bonus);
                    return;
                }

                if (bonus is FuelBonus)
                {
                    playerTank = new FuelTank(tank);
                    GameField.Bonuses.Remove(bonus);
                    return;
                }

                if (bonus is SpeedBonus)
                {
                    playerTank = new FastTank(tank);
                    GameField.Bonuses.Remove(bonus);
                    return;
                }
            }
        }
    }
}
