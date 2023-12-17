using GameEngine.Game;
using GameEngine.Objects;
using GameLibrary.Bonuses;
using System.Drawing;

namespace GameLibrary.Factories
{
    /// <summary>
    /// Класс фабрики защитного бонуса
    /// </summary>
    public class ArmorBonusFactory : BaseBonusFactory
    {
        private readonly GameField gameField;
        private readonly int[] armorTextures;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ArmorBonusFactory(GameField gameField, int[] armorTextures)
        {
            this.gameField = gameField;
            this.armorTextures = armorTextures;
        }

        public override Bonus Spawn(Point position)
        {
            return new ArmorBonus(position, gameField, armorTextures);
        }
    }
}
