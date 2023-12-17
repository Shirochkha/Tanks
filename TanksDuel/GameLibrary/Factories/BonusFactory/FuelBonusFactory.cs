using GameEngine.Game;
using GameEngine.Objects;
using GameLibrary.Bonuses;
using System.Drawing;

namespace GameLibrary.Factories
{
    /// <summary>
    /// Класс фабрики бонуса с топливом
    /// </summary>
    public class FuelBonusFactory : BaseBonusFactory
    {
        private readonly GameField gameField;
        private readonly int[] fuelTextures;

        /// <summary>
        /// Конструктор
        /// </summary>
        public FuelBonusFactory(GameField gameField, int[] fuelTextures)
        {
            this.gameField = gameField;
            this.fuelTextures = fuelTextures;
        }

        public override Bonus Spawn(Point position)
        {
            return new FuelBonus(position, gameField, fuelTextures);
        }
    }
}
