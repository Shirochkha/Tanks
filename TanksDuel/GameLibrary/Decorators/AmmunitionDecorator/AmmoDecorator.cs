using System.Drawing;

using GameEngine.Game;
using GameEngine.Input;
using GameEngine.Objects;

namespace GameLibrary.AmmunitionDecorator
{
    /// <summary>
    /// Класс декоратора боеприпасов
    /// </summary>
    public class AmmoDecorator : Ammo
    {
        private readonly Ammo ammo;
        public override int Range => ammo.Range;
        public override int Damage => ammo.Damage;
        public override int DamageArea => ammo.DamageArea;
        public override int Speed => ammo.Speed;
        public override Direction ShotDirection { get => ammo.ShotDirection; set => ammo.ShotDirection = value; }
        public override GameField GameField { get => ammo.GameField; set => ammo.GameField = value; }
        public override Tank Parent { get => ammo.Parent; set => ammo.Parent = value; }
        public override Point Location { get => ammo.Location; set => ammo.Location = value; }

        /// <summary>
        /// Конструктор класс декоратора боеприпасов
        /// </summary>
        public AmmoDecorator(Ammo ammo)
        {
            this.ammo = ammo;
        }

        public override void SetTexture()
        {
            ammo.SetTexture();
        }
    }
}
