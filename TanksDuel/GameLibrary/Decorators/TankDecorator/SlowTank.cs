using GameEngine.Input;
using GameLibrary.Bonuses;
using System.Drawing;
using System.Linq;

namespace GameLibrary.Decorators
{
    /// <summary>
    /// Класс медленного танка
    /// </summary>
    public class SlowTank : TankDecorator
    {
        private const int _slowValue = 4;

        /// <summary>
        /// Конструктор класса медленного танка
        /// </summary>
        public SlowTank(NormalTank tank) : base(tank) { }

        public override Point Location
        {
            get => tank.Location;
            set => tank.Location = value;
        }

        public override int Speed => tank.Speed - _slowValue;

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

            if (!InDirt())
            {
                if (GameField.PlayerTank == this)
                    GameField.PlayerTank = base.tank;
                else
                    GameField.EnemyTank = base.tank;
            }

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
