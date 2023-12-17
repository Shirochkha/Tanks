using System.Drawing;

using GameEngine.Game;
using GameEngine.Input;
using GameEngine.Objects;

namespace GameLibrary.AmmunitionDecorator
{
    /// <summary>
    /// Класс пули
    /// </summary>
    public class Bullet : Ammo
    {
        public override int Range => 500;
        public override int Damage => 50;
        public override int DamageArea => 100;
        public override int Speed { get; } = 10;
        public override GameField GameField { get; set; }
        public override Point Location { get; set; }
        public override Tank Parent { get; set; }
        public override Direction ShotDirection { get; set; }

        /// <summary>
        /// Конструктор класса пули
        /// </summary>
        public Bullet(GameField field, Point startLocation, Tank parent, Direction direction)
        {
            Location = startLocation;
            GameField = field;
            Parent = parent;
            ShotDirection = direction;
        }

        public override void SetTexture()
        {
            Textures = TextureRepository.Get("bullet");
        }
    }
}
