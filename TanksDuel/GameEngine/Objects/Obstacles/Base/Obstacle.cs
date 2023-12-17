using System.Drawing;

using GameEngine.Helpers;
using GameEngine.Game;

namespace GameEngine.Objects
{
    /// <summary>
    /// Класс абстрактного препятствия
    /// </summary>
    public abstract class Obstacle : GameObject
    {
        /// <summary>
        /// Коллекция текстур препятствий
        /// </summary>
        public override int[] Textures { get; set; }
        /// <summary>
        /// Позиция препятствия
        /// </summary>
        public override Point Location { get; set; }
    }
}
