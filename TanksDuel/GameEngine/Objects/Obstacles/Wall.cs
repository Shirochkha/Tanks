using System.Drawing;

using OpenTK.Graphics.OpenGL;

namespace GameEngine.Objects
{
    /// <summary>
    /// Класс стены
    /// </summary>
    public class Wall : Obstacle
    {
        /// <summary>
        /// Ширина
        /// </summary>
        public override int Width { get { return 50; } }
        /// <summary>
        /// Высота
        /// </summary>
        public override int Height { get { return 50; } }

        /// <summary>
        /// Констрктор класса стены
        /// </summary>
        public Wall(int[] textures)
        {
            Textures = textures;
        }

        /// <summary>
        /// Метод отрисовки стены
        /// </summary>
        public override void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, Textures[current_texture]);

            GL.Begin(BeginMode.Polygon);

            GL.Color3(Color.White);

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
