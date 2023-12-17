using GameEngine.Game;
using GameEngine.Objects;
using GameLibrary.Bonuses;
using System.Drawing;

namespace GameLibrary.Factories
{
    /// <summary>
    /// Класс фабрики бонуса скорости
    /// </summary>
    public class SpeedBonusFactory : BaseBonusFactory
    {
        private readonly GameField gameField;
        private readonly int[] speedTextures;

        /// <summary>
        /// Конструктор
        /// </summary>
        public SpeedBonusFactory(GameField gameField, int[] speedTextures)
        {
            this.gameField = gameField;
            this.speedTextures = speedTextures;
        }

        public override Bonus Spawn(Point position)
        {
            return new SpeedBonus(position, gameField, speedTextures);
        }
    }
}
