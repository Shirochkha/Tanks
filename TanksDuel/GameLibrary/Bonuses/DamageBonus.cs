using GameEngine.Game;
using GameEngine.Objects;
using System.Drawing;

namespace GameLibrary.Bonuses
{
    /// <summary>
    /// Класс бонуса урона
    /// </summary>
    public class DamageBonus : Bonus
    {
        private Point point;

        /// <summary>
        /// Конструктор
        /// </summary>
        public DamageBonus(Point point, GameField gameField, int[] textures)
        {
            this.point = point;
            GameField = gameField;
            Textures = textures;
        }

        public override string Type => nameof(DamageBonus);
        public override GameField GameField { get; set; }
        public override int[] Textures { get; set; }
        public override Point Location { get => point; set => point = value; }
    }
}
