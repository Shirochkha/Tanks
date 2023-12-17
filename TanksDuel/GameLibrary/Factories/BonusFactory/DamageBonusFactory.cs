using GameEngine.Game;
using GameEngine.Objects;
using GameLibrary.Bonuses;
using System.Drawing;

namespace GameLibrary.Factories
{
    /// <summary>
    /// Класс фабрики бонуса с добавлением урона
    /// </summary>
    public class DamageBonusFactory : BaseBonusFactory
    {
        private readonly GameField gameField;
        private readonly int[] damageTextures;

        /// <summary>
        /// Конструктор
        /// </summary>
        public DamageBonusFactory(GameField gameField, int[] damageTextures)
        {
            this.gameField = gameField;
            this.damageTextures = damageTextures;
        }

        public override Bonus Spawn(Point position)
        {
            return new DamageBonus(position, gameField, damageTextures);
        }
    }
}
