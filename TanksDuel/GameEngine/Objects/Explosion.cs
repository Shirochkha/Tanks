using System.Drawing;
using OpenTK.Graphics.OpenGL;
using GameEngine.Game;

namespace GameEngine.Objects
{
    /// <summary>
    /// Класс взрыва
    /// </summary>
    public class Explosion : GameObject
    {
        /// <summary>
        /// Коллекция текстур взрыва
        /// </summary>
        public override int[] Textures { get; set; }
        /// <summary>
        /// Размер
        /// </summary>
        private int _size;
        /// <summary>
        /// Ссылка на игровое поле
        /// </summary>
        private GameField _gameField;
        /// <summary>
        /// Ширина текстуры
        /// </summary>
        public override int Width { get { return _size; } }
        /// <summary>
        /// Высота текстуры
        /// </summary>
        public override int Height { get { return _size; } }
        /// <summary>
        /// Местоположение взрыва
        /// </summary>
        public override Point Location { get; set; }

        /// <summary>
        /// Конструктор класса взрыва
        /// </summary>
        public Explosion(IExplodable ammo)
        {
            _size = ammo.DamageArea;
            Location = ((GameObject)ammo).Location;
            _gameField = ammo.GameField;

            foreach (var tank in _gameField.Tanks)
            {
                if (CheckCollision(tank))
                {
                    if (tank.Armor > 0)
                    {
                        tank.Armor -= ammo.Damage;

                        if (tank.Armor < 0)
                        {
                            tank.Health += tank.Armor;
                            tank.Armor = 0;
                        }
                    }
                    else
                    {
                        tank.Health -= ammo.Damage;
                    }

                    tank.OnChanged();
                }
            }

            Textures = TextureRepository.Get("explosion");
        }

        /// <summary>
        /// Функция рисования взрыва
        /// </summary>
        public override void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, Textures[current_texture++]);

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
            if(current_texture >= Textures.Length)
            {
                _gameField.Shots.Remove(this);
            }
        }
    }
}
