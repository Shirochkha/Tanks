using System.Drawing;

namespace GameEngine.Game
{
    /// <summary>
    /// Базовый класс всех объектов на игровом поле
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// Коллекция текстур
        /// </summary>
        public abstract int[] Textures{ get; set; }
        /// <summary>
        /// Текущая текстура
        /// </summary>
        protected int current_texture = 0;
        /// <summary>
        /// Ширина текстуры
        /// </summary>
        public abstract int Width { get; }
        /// <summary>
        /// Высота текстуры
        /// </summary>
        public abstract int Height { get; }
        /// <summary>
        /// Координаты на игровом поле
        /// </summary>
        public abstract Point Location { get; set; }

        /// <summary>
        /// Границы объекта
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(Location.X - Width / 2, Location.Y - Height / 2, Width, Height);
            }
        }

        /// <summary>
        /// Функция для коллизий
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CheckCollision(GameObject obj)
        {
            if (obj == this)
                return false;

            return Bounds.IntersectsWith(obj.Bounds);
        }
        /// <summary>
        /// Функция рисования
        /// </summary>
        public abstract void Draw();
    }
}
