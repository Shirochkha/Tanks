using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameEngine.Game;

namespace GameEngine.Objects
{
    /// <summary>
    /// Интерфейс взрывающегося объекта
    /// </summary>
    public interface IExplodable
    {
        /// <summary>
        /// Дальность выстрела
        /// </summary>
        int Range { get; }
        /// <summary>
        /// Наносимый урон
        /// </summary>
        int Damage { get; }
        /// <summary>
        /// Зона поражения
        /// </summary>
        int DamageArea { get; }
        /// <summary>
        /// Ссылка на игровое поле
        /// </summary>
        GameField GameField { get; }
        /// <summary>
        /// Ссылка на родительский объект
        /// </summary>
        Tank Parent { get; }
        /// <summary>
        /// Взрыв
        /// </summary>
        void Explode();
    }
}
