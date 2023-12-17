using System;

using GameEngine.Game;
using GameEngine.Objects;

namespace GameEngine.Input
{
    /// <summary>
    /// Класс контроллера клавиатуры
    /// </summary>
    public class KeyboardController : IController
    {
        /// <summary>
        /// Текущее направление
        /// </summary>
        public Direction CurrentDirection { get; set; } = Direction.Up;
        /// <summary>
        /// Ссылка на танк
        /// </summary>
        public Tank Tank { get; set; }
        /// <summary>
        /// Ссылка на поле
        /// </summary>
        public GameField GameField { get; set; }
        /// <summary>
        /// Событие начала движения
        /// </summary>
        public event EventHandler StartMoving;
        /// <summary>
        /// Событие остановки движения
        /// </summary>
        public event EventHandler StopMoving;
        /// <summary>
        /// Событие стрельбы
        /// </summary>
        public event EventHandler Shooting;
        /// <summary>
        /// Конструктор класса контроллера клавиатуры
        /// </summary>
        public KeyboardController(GameField field)
        {
            GameField = field;
        }
        /// <summary>
        /// Метод начала движения
        /// </summary>
        public void StartMove(Direction direction)
        {
            CurrentDirection = direction;
            StartMoving?.Invoke(this, null);
        }
        /// <summary>
        /// Метод остановки движения
        /// </summary>
        public void StopMove()
        {
            StopMoving?.Invoke(this, null);
        }
        /// <summary>
        /// Метод стрельбы
        /// </summary>
        public void Shoot()
        {
            Shooting?.Invoke(this, null);
        }
    }
}
