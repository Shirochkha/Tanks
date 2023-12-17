using GameEngine.Game;
using GameEngine.Input;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Linq;

namespace GameEngine.Objects
{
    /// <summary>
    /// Класс абстрактного боеприпаса
    /// </summary>
    public abstract class Ammo : GameObject, IExplodable
    {
        /// <summary>
        /// Коллекция текстур боеприпасов
        /// </summary>
        public override int[] Textures { get; set; }
        /// <summary>
        /// Дальность выстрела
        /// </summary>
        public abstract int Range { get; }
        /// <summary>
        /// Наносимый урон
        /// </summary>
        public abstract int Damage { get; }
        /// <summary>
        /// Зона поражения
        /// </summary>
        public abstract int DamageArea { get; }
        /// <summary>
        /// Направление выстрела
        /// </summary>
        public abstract Direction ShotDirection { get; set; }
        /// <summary>
        /// Ссылка на игровое поле
        /// </summary>
        public abstract GameField GameField { get; set; }
        /// <summary>
        /// Ссылка на родительский объект
        /// </summary>
        public abstract Tank Parent { get; set; }
        /// <summary>
        /// Скорость
        /// </summary>
        public abstract int Speed { get; }

        /// <summary>
        /// Метод установки текстуры
        /// </summary>
        public abstract void SetTexture();
        /// <summary>
        /// Текущая дальность
        /// </summary>
        private int _currentRange;

        /// <summary>
        /// Ширинатекстуры боеприпаса
        /// </summary>
        public override int Width =>
            ShotDirection == Direction.Up ||
            ShotDirection == Direction.Down ? 10 : 20;

        /// <summary>
        /// Высота текстуры боеприпаса
        /// </summary>
        public override int Height =>
            ShotDirection == Direction.Up ||
            ShotDirection == Direction.Down ? 20 : 10;

        /// <summary>
        /// Функция перемещения боеприпаса
        /// </summary>
        private void Move()
        {
            switch (ShotDirection)
            {
                case Direction.Up:
                    Location = new Point(Location.X, Location.Y - Speed);
                    break;
                case Direction.Down:
                    Location = new Point(Location.X, Location.Y + Speed);
                    break;
                case Direction.Left:
                    Location = new Point(Location.X - Speed, Location.Y);
                    break;
                case Direction.Right:
                    Location = new Point(Location.X + Speed, Location.Y);
                    break;
            }

            if (Location.X < 0 || Location.X > GameField.ViewportSize.Width ||
               Location.Y < 0 || Location.Y > GameField.ViewportSize.Height)
            {
                GameField.Shots.Remove(this);
                return;
            }

            var objects = GameField.Walls.ToArray();
            foreach (var obj in objects)
            {
                if (obj != Parent && CheckCollision(obj))
                    Explode();
            }

            _currentRange += Speed;
            if (_currentRange > Range)
            {
                GameField.Shots.Remove(this);
                return;
            }
        }
        /// <summary>
        /// Взрыв боеприпаса
        /// </summary>
        public void Explode()
        {
            Explosion explosion = new Explosion(this);
            GameField.Shots.Remove(this);
            GameField.Shots.Add(explosion);
        }
        /// <summary>
        /// Функция отрисовки боеприпаса
        /// </summary>
        public override void Draw()
        {
            Move();

            GL.BindTexture(TextureTarget.Texture2D, Textures[current_texture]);

            GL.Begin(BeginMode.Polygon);

            Point textureX = getStartCoord(ShotDirection);
            GL.TexCoord2(textureX.X, textureX.Y);
            GL.Vertex3(Bounds.X, Bounds.Y, 0);

            textureX = getNextTextureCoord(textureX);
            GL.TexCoord2(textureX.X, textureX.Y);
            GL.Vertex3(Bounds.X + Bounds.Width, Bounds.Y, 0);

            textureX = getNextTextureCoord(textureX);
            GL.TexCoord2(textureX.X, textureX.Y);
            GL.Vertex3(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height, 0);

            textureX = getNextTextureCoord(textureX);
            GL.TexCoord2(textureX.X, textureX.Y);
            GL.Vertex3(Bounds.X, Bounds.Y + Bounds.Height, 0);

            GL.End();
        }
        /// <summary>
        /// Функция получения начальных координат текстуры
        /// </summary>
        private static Point getStartCoord(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Point(1, 0);
                case Direction.Right:
                    return new Point(0, 0);
                case Direction.Down:
                    return new Point(0, 1);
                default:
                    return new Point(1, 1);
            }
        }
        /// <summary>
        /// Функция получения последующих текстурных координат
        /// </summary>
        private static Point getNextTextureCoord(Point coord)
        {
            if (coord.X == 0 && coord.Y == 0)
            {
                coord.X = 1;
                return coord;
            }

            if (coord.X == 1 && coord.Y == 0)
            {
                coord.Y = 1;
                return coord;
            }

            if (coord.X == 1 && coord.Y == 1)
            {
                coord.X = 0;
                return coord;
            }

            coord.X = 0;
            coord.Y = 0;
            return coord;
        }
    }
}
