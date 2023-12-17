using GameEngine.Game;
using GameEngine.Input;
using GameEngine.Objects;
using GameLibrary.Bonuses;
using GameLibrary.Weapons;
using System.Drawing;
using System.Linq;

namespace GameLibrary.Decorators
{
    /// <summary>
    /// Класс обычного танка
    /// </summary>
    public class NormalTank : Tank
    {
        public override int Speed { get; }

        public override int Health { get; set; } = 250;

        public override int Armor { get; set; } = 0;

        public override int Fuel { get; set; } = 1000;

        public override IController Controller { get; }

        public override bool IsMoving { get; set; }

        /// <summary>
        /// Конструктор класса обычного танка
        /// </summary>
        public NormalTank(IController controller, int[] textures)
        {
            Speed = 6;
            Ammunition = 5;
            Controller = controller;
            Controller.Tank = this;

            Textures = textures;
            Weapons = new BulletWeapon();

            Controller.StartMoving += _controller_StartMoving;
            Controller.StopMoving += _controller_StopMoving;
            Controller.Shooting += _controller_Shooting;
        }

        /// <summary>
        /// Пустой конструктор класса обычного танка(для json)
        /// </summary>
        public NormalTank()
        {
        }

        /// <summary>
        /// Метод перемещения обычного танка
        /// </summary>
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

            if (InDirt())
            {
                if (GameField.PlayerTank == this)
                    GameField.PlayerTank = new SlowTank(this);
                else
                    GameField.EnemyTank = new SlowTank(this);
            }

            if (GotBonus())
            {
                var bonus = GameField.Bonuses.FirstOrDefault(obj => obj.Bounds.IntersectsWith(Bounds));
                if (bonus is AmmunitionBonus)
                {
                    if (GameField.PlayerTank == this)
                        GameField.PlayerTank = new BulletTank(this);
                    else
                        GameField.EnemyTank = new BulletTank(this);
                    GameField.Bonuses.Remove(bonus);
                    return;
                }

                if (bonus is ArmorBonus)
                {
                    if (GameField.PlayerTank == this)
                        GameField.PlayerTank = new ArmorTank(this);
                    else
                        GameField.EnemyTank = new ArmorTank(this);
                    GameField.Bonuses.Remove(bonus);
                    return;
                }

                if (bonus is DamageBonus)
                {
                    if (GameField.PlayerTank == this)
                        GameField.PlayerTank = new DamageBoostTank(this);
                    else
                        GameField.EnemyTank = new DamageBoostTank(this);
                    GameField.Bonuses.Remove(bonus);
                    return;
                }

                if (bonus is FuelBonus)
                {
                    if (GameField.PlayerTank == this)
                        GameField.PlayerTank = new FuelTank(this);
                    else
                        GameField.EnemyTank = new FuelTank(this);
                    GameField.Bonuses.Remove(bonus);
                    return;
                }

                if (bonus is SpeedBonus)
                {
                    if (GameField.PlayerTank == this) 
                        GameField.PlayerTank = new FastTank(this);
                    else
                        GameField.EnemyTank = new FastTank(this);
                    GameField.Bonuses.Remove(bonus);
                    return;
                }
            }
        }

        public override int[] Textures { get; set; }
        public override int Ammunition { get; set; }
        public override GameEngine.Objects.Weapons Weapons { get; set; }
        public override GameField GameField => Controller.GameField;
        public override Direction Direction => Controller.CurrentDirection;
    }
}