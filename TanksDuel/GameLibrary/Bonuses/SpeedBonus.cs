using GameEngine.Game;
using GameEngine.Objects;
using System.Drawing;

namespace GameLibrary.Bonuses
{
    /// <summary>
    /// Класс бонуса скорости
    /// </summary>
    public class SpeedBonus : Bonus
    {
        private Point point;

        /// <summary>
        /// Конструктор
        /// </summary>
        public SpeedBonus(Point point, GameField gameField, int[] textures)
        {
            this.point = point;
            GameField = gameField;
            Textures = textures;
        }

        public override string Type => nameof(SpeedBonus);
        public override GameField GameField { get; set; }
        public override int[] Textures { get; set; }
        public override Point Location { get => point; set => point = value; }
    }
}
