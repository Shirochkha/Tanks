using GameEngine.Helpers;
using System.Drawing;

namespace TanksDuel
{
    public static class TextureCreator
    {
        public static int[] CreateExplosion()
        {
            const int SPRITE_WIDTH = 192;
            const int SPRITE_HEIGHT = 192;

            int[] textures = new int[20];
            Bitmap tex = Resources.explosionSprites;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Bitmap sprite = new Bitmap(SPRITE_WIDTH, SPRITE_HEIGHT);
                    Graphics g = Graphics.FromImage(sprite);
                    g.DrawImage(tex, new Rectangle(0, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(j * SPRITE_WIDTH, i * SPRITE_HEIGHT, SPRITE_WIDTH, SPRITE_HEIGHT), GraphicsUnit.Pixel);
                    textures[i * 5 + j] = GLTexture.LoadTexture(sprite);
                }
            }
            return textures;
        }

        public static int[] CreateTank(Bitmap tex, params Rectangle[] rectangles)
        {
            int[] textures = new int[rectangles.Length];

            for (int i = 0; i < rectangles.Length; i++)
            {
                Bitmap sprite = new Bitmap(rectangles[i].Width, rectangles[i].Height);
                using (Graphics g = Graphics.FromImage(sprite))
                {
                    g.DrawImage(tex, new Rectangle(0, 0, sprite.Width, sprite.Height),
                        new Rectangle(rectangles[i].X, rectangles[i].Y, sprite.Width, sprite.Height), GraphicsUnit.Pixel);
                }

                textures[i] = GLTexture.LoadTexture(sprite);
            }

            return textures;
        }
        public static int[] CreatePlayerTank()
        {
            Bitmap tex = Resources.tankFirst;

            return CreateTank(tex,
                new Rectangle(0, 0, 126, 224),
                new Rectangle(126, 50, 310 - 126, 160 - 50),
                new Rectangle(337, 50, 526 - 337, 160 - 50),
                new Rectangle(550, 0, 126, 224)
            );
        }
        public static int[] CreateEnemyTank()
        {
            Bitmap tex = Resources.tankSecond;

            return CreateTank(tex,
                new Rectangle(0, 0, 126, 224),
                new Rectangle(126, 50, 310 - 126, 160 - 50),
                new Rectangle(337, 50, 526 - 337, 160 - 50),
                new Rectangle(550, 0, 126, 224)
            );
        }

        public static int[] CreateTextureArray(params Bitmap[] textures)
        {
            int[] textureIds = new int[textures.Length];

            for (int i = 0; i < textures.Length; i++)
            {
                textureIds[i] = GLTexture.LoadTexture(textures[i]);
            }

            return textureIds;
        }
        public static int[] CreateAmmo() => CreateTextureArray(Resources.bullet);
        public static int[] CreateBackground() => CreateTextureArray(Resources.grass);
        public static int[] CreateOuterWall() => CreateTextureArray(Resources.outerWall);
        public static int[] CreateInterWall() => CreateTextureArray(Resources.interWall);
        public static int[] CreateShallow() => CreateTextureArray(Resources.dirt);
        public static int[] CreateSpeedBonus() => CreateTextureArray(Resources.Speed);
        public static int[] CreateArmorBonus() => CreateTextureArray(Resources.Armor);
        public static int[] CreateDamageBonus() => CreateTextureArray(Resources.bulletDamage);
        public static int[] CreateFuelBonus() => CreateTextureArray(Resources.fuel);
        public static int[] CreateNewBulletsBonus() => CreateTextureArray(Resources.bulletNums);


    }
}
