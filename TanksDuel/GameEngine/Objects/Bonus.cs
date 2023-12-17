using GameEngine.Game;
using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.Objects
{
    /// <summary>
    /// Класс абстрактного бонуса
    /// </summary>
    public abstract class Bonus : GameObject
    {
        /// <summary>
        /// тип бонуса
        /// </summary>
        public abstract string Type { get; }
        /// <summary>
        /// Ссылка на игровое поле
        /// </summary>
        [JsonIgnore]
        public abstract GameField GameField { get; set; }
        /// <summary>
        /// Высота текстуры бонуса
        /// </summary>
        public override int Height => 50;
        /// <summary>
        /// Ширина текстуры бонуса
        /// </summary>
        public override int Width => 50;

        /// <summary>
        /// Функция отрисовки бонуса
        /// </summary>
        public override void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, Textures[current_texture]);

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
    }
}
