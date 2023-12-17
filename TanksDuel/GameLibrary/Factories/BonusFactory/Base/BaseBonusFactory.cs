using GameEngine.Objects;
using System.Drawing;

namespace GameLibrary.Factories
{
    /// <summary>
    /// Абстрактный класс фабрики базового бонуса
    /// </summary>
    public abstract class BaseBonusFactory
    {
        /// <summary>
        /// Метод спавна бонуса
        /// </summary>
        public abstract Bonus Spawn(Point position);
    }
}
