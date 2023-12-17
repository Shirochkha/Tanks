using GameEngine.Game;
using GameEngine.Input;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Linq;


namespace GameEngine.Objects
{
    public abstract class Tank : GameObject
    {
        /// <summary>
        /// Событие изменения параметров танка
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Метод для вызова события извне
        /// </summary>
        public void OnChanged()
        {
            Changed?.Invoke(this, null);
        }

        /// <summary>
        /// Находится ли танк в движении
        /// </summary>
        public abstract bool IsMoving { get; set; }

        /// <summary>
        /// Контроллер управления
        /// </summary>
        public abstract IController Controller { get; }

        /// <summary>
        /// Оружие
        /// </summary>
        public abstract Weapons Weapons { get; set; }

        /// <summary>
        /// Ссылка на игровое поле
        /// </summary>
        public abstract GameField GameField { get; }

        /// <summary>
        /// Направление движения
        /// </summary>
        public abstract Direction Direction { get; }

        /// <summary>
        /// Скорость танка
        /// </summary>
        public abstract int Speed { get; }

        /// <summary>
        /// Патроны
        /// </summary>
        public abstract int Ammunition { get; set; }

        /// <summary>
        /// Текущая текстура танка
        /// </summary>
        private int _current_texture
        {
            get
            {
                switch (Controller.CurrentDirection)
                {
                    case Direction.Down:
                        return 0;
                    case Direction.Left:
                        return 1;
                    case Direction.Right:
                        return 2;
                    default:
                        return 3;
                }
            }
        }

        /// <summary>
        /// Ширина
        /// </summary>
        public override int Width
        {
            get
            {
                return Controller.CurrentDirection == Direction.Up ||
                       Controller.CurrentDirection == Direction.Down ? 50 : 100;
            }
        }

        /// <summary>
        /// Высота
        /// </summary>
        public override int Height
        {
            get
            {
                return Controller.CurrentDirection == Direction.Up ||
                       Controller.CurrentDirection == Direction.Down ? 100 : 50;
            }
        }

        /// <summary>
        /// Здоровье
        /// </summary>
        public abstract int Health { get; set; }
        /// <summary>
        /// Защита
        /// </summary>
        public abstract int Armor { get; set; }
        /// <summary>
        /// Топливо
        /// </summary>
        public abstract int Fuel { get; set; }
        /// <summary>
        /// Потеря топлива
        /// </summary>
        protected readonly int _fuelLoss = 1;
        /// <summary>
        /// Местоположение
        /// </summary>
        public override Point Location { get; set; }

        /// <summary>
        /// Метод движения танка
        /// </summary>
        protected abstract void Move();

        /// <summary>
        /// Отрисовка танка
        /// </summary>
        public override void Draw()
        {
            Move();

            GL.BindTexture(TextureTarget.Texture2D, Textures[_current_texture]);

            GL.Begin(BeginMode.Polygon);

            GL.TexCoord2(0, 0);
            GL.Vertex3(Bounds.X, Bounds.Y, 0);

            GL.TexCoord2(1, 0);
            GL.Vertex3(Bounds.X + Bounds.Width, Bounds.Y, 0);

            GL.TexCoord2(1, 1);
            GL.Vertex3(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height, 0);

            GL.TexCoord2(0, 1);
            GL.Vertex3(Bounds.X, Bounds.Y + Bounds.Height, 0);

            GL.End();
        }

        /// <summary>
        /// Определение доступных направлений
        /// </summary>
        protected Direction FindAllowedDirections()
        {
            Direction allowedDirections = Direction.All;
            Rectangle inflatedBounds = Bounds;
            inflatedBounds.Inflate(Speed, Speed);

            if (inflatedBounds.X <= 0)
                allowedDirections &= ~Direction.Left;

            if (inflatedBounds.Y <= 0)
                allowedDirections &= ~Direction.Up;

            if (inflatedBounds.X + inflatedBounds.Width >= Controller.GameField.ViewportSize.Width)
                allowedDirections &= ~Direction.Right;

            if (inflatedBounds.Y + inflatedBounds.Height >= Controller.GameField.ViewportSize.Height)
                allowedDirections &= ~Direction.Down;

            var collisions = Controller.GameField.Walls
                .Where(obj => obj.Bounds.IntersectsWith(Bounds) && obj.Bounds != Bounds);

            foreach (var collision in collisions)
            {
                Rectangle intersect = Rectangle.Intersect(inflatedBounds, collision.Bounds);

                if (inflatedBounds.X == intersect.X && intersect.Width < intersect.Height)
                    allowedDirections &= ~Direction.Left;
                else if (inflatedBounds.Y == intersect.Y && intersect.Width > intersect.Height)
                    allowedDirections &= ~Direction.Up;
                else if (inflatedBounds.X + inflatedBounds.Width == intersect.Right && intersect.Width < intersect.Height)
                    allowedDirections &= ~Direction.Right;
                else if (inflatedBounds.Y + inflatedBounds.Height == intersect.Bottom && intersect.Width > intersect.Height)
                    allowedDirections &= ~Direction.Down;
            }

            return allowedDirections;
        }

        /// <summary>
        /// Получение бонуса
        /// </summary>
        /// <returns></returns>
        public bool GotBonus()
        {
            return GameField.Bonuses.FirstOrDefault(obj => obj.Bounds.IntersectsWith(Bounds)) != null;
        }

        /// <summary>
        /// В грязи танк или нет
        /// </summary>
        /// <returns></returns>
        protected bool InDirt()
        {
            return GameField.Dirts.FirstOrDefault(obj => obj.Bounds.IntersectsWith(Bounds)) != null;
        }

        /// <summary>
        /// Обработчик стрельбы
        /// </summary>
        public virtual void _controller_Shooting(object sender, EventArgs e)
        {
            Ammo ammo = Weapons.Shot();
            if (ammo != null)
                GameField.Shots.Add(ammo);
        }

        /// <summary>
        /// Обработчик остановки движения
        /// </summary>
        public void _controller_StopMoving(object sender, EventArgs e)
        {
            IsMoving = false;
        }

        /// <summary>
        /// Обработчик начала движения
        /// </summary>
        public void _controller_StartMoving(object sender, EventArgs e)
        {
            IsMoving = true;
        }

    }

}
