using GameEngine.Game;
using GameEngine.Objects;
using System.Drawing;

namespace GameLibrary.Bonuses
{
    /// <summary>
    /// Класс бонуса количества боеприпасов
    /// </summary>
    public class AmmunitionBonus : Bonus
    {
        private Point point;

        /// <summary>
        /// Конструктор
        /// </summary>
        public AmmunitionBonus(Point point, GameField gameField, int[] textures)
        {
            this.point = point;
            GameField = gameField;
            Textures = textures;
        }


        public override GameField GameField { get; set; }
        public override int[] Textures { get; set; }
        public override Point Location { get => point; set => point = value; }
        public override string Type => nameof(AmmunitionBonus);
    }
}
