using GameEngine.Game;
using GameEngine.Input;
using GameEngine.Objects;

namespace GameLibrary.Decorators
{
    /// <summary>
    /// Класс декоратора танка
    /// </summary>
    public abstract class TankDecorator : Tank
    {
        /// <summary>
        /// Ссылка на обычный танк
        /// </summary>
        protected NormalTank tank;
        /// <summary>
        /// Конструктор класса декоратора танка
        /// </summary>
        public TankDecorator(NormalTank tank) => this.tank = tank; 

        public override IController Controller => tank.Controller;

        public override bool IsMoving
        {
            get => tank.IsMoving;
            set => tank.IsMoving = value;
        }

        public override int Speed => tank.Speed;

        public override int Health
        {
            get => tank.Health;
            set => tank.Health = value;
        }

        public override int Armor
        {
            get => tank.Armor;
            set => tank.Armor = value;
        }

        public override int Fuel
        {
            get => tank.Fuel;
            set => tank.Fuel = value;
        }

        public override int Ammunition
        {
            get => tank.Ammunition;
            set => tank.Ammunition = value;
        }

        public override GameEngine.Objects.Weapons Weapons
        {
            get => tank.Weapons;
            set => tank.Weapons = value;
        }
        public override GameField GameField => tank.GameField;

        public override Direction Direction => tank.Direction;

        public override int[] Textures
        {
            get => tank.Textures;
            set => tank.Textures = value;
        }
    }
}
