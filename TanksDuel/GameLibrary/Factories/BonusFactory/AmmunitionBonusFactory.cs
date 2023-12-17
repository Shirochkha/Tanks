using GameEngine.Game;
using GameEngine.Objects;
using GameLibrary.Bonuses;
using System.Drawing;

namespace GameLibrary.Factories
{
    /// <summary>
    /// Класс фабрики бонуса с дополнительными боеприпасами
    /// </summary>
    public class AmmunitionBonusFactory : BaseBonusFactory
    {
        private readonly GameField gameField;
        private readonly int[] ammunitionTextures;

        /// <summary>
        /// Конструктор
        /// </summary>
        public AmmunitionBonusFactory(GameField gameField, int[] ammunitionTextures)
        {
            this.gameField = gameField;
            this.ammunitionTextures = ammunitionTextures;
        }

        public override Bonus Spawn(Point position)
        {
            return new AmmunitionBonus(position, gameField, ammunitionTextures);
        }
    }

}
