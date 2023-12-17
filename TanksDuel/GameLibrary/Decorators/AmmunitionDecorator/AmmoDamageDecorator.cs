using GameEngine.Objects;

namespace GameLibrary.AmmunitionDecorator
{
    /// <summary>
    /// Класс декоратора урона боеприпаса
    /// </summary>
    public class AmmoDamageDecorator : AmmoDecorator
    {
        private const int _boostDamage = 20;
        public override int Damage => base.Damage + _boostDamage;

        /// <summary>
        /// Конструктор класса декоратора урона боеприпаса
        /// </summary>
        public AmmoDamageDecorator(Ammo ammo, int[] textures) : base(ammo)
        {
            Textures = textures;
        }

    }
}
