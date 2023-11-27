using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace TanksDuel
{
    public class TextureRenderer
    {
        public static void Draw(Texture2D texture, Vector2 position, Vector2 scale)
        {
            if (texture == null) return;

            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture.Id);

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0f, 0f);
            GL.Vertex2(position.X, position.Y);

            GL.TexCoord2(1f, 0f);
            GL.Vertex2(position.X + scale.X, position.Y);

            GL.TexCoord2(1f, 1f);
            GL.Vertex2(position.X + scale.X, position.Y + scale.Y);

            GL.TexCoord2(0f, 1f);
            GL.Vertex2(position.X, position.Y + scale.Y);

            GL.End();
            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
        }

        public static void Begin(int screenWidth, int screenHeight)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.LightGreen);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, screenWidth, screenHeight, 0, -1, 1);
        }
    }
}
