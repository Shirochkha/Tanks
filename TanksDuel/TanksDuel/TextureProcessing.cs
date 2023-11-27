using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TanksDuel
{
    public class TextureProcessing
    {
        public static Texture2D LoadTexture(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Файл не найден, проверьте путь {path}");
            }

            var id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            var bmp = new Bitmap(path);

            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                    ImageLockMode.ReadOnly,
                                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(target: TextureTarget.Texture2D,
                          level: 0,
                          internalformat: PixelInternalFormat.Rgba,
                          width: data.Width,
                          height: data.Height,
                          border: 0,
                          format: OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                          type: PixelType.UnsignedByte,
                          pixels: data.Scan0);

            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return new Texture2D(id, bmp.Width, bmp.Height);
        }
    }
}
