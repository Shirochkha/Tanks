using System;
using GameEngine.Game;
using GameEngine.Objects;

namespace GameEngine.Input
{
    /// <summary>
    /// Интерфейс контроллера управления
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Событие начала движения
        /// </summary>
        event EventHandler StartMoving;
        /// <summary>
        /// Событие остановки движения
        /// </summary>
        event EventHandler StopMoving;
        /// <summary>
        /// Событие стрельбы
        /// </summary>
        event EventHandler Shooting;
        /// <summary>
        /// Текущее направление движения
        /// </summary>
        Direction CurrentDirection { get; set; }
        /// <summary>
        /// Ссылка на управляемый танк
        /// </summary>
        Tank Tank { get; set; }
        /// <summary>
        /// Ссылка на игровое поле
        /// </summary>
        GameField GameField { get; set; }
    }

}
