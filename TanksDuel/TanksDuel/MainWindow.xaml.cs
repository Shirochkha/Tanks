using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
using System;
using System.Drawing;
using System.Windows;

namespace TanksDuel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int screenWidth;
        private int screenHeight;

        public MainWindow()
        {
            InitializeComponent();
            var settings = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 1
            };
            OpenTkControl.Start(settings);
            OpenTkControl.Render += OpenTkControl_Render;
            //OpenTkControl.SizeChanged += OpenTkControl_SizeChanged;

            screenWidth = (int)this.Width;
            screenHeight = (int)this.Height;

        }

        //private void OpenTkControl_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    screenWidth = (int)e.NewSize.Width;
        //    screenHeight = (int)e.NewSize.Height;

        //    GL.Viewport(0, 0, screenWidth, screenHeight);
        //}

        private void OpenTkControl_Render(TimeSpan obj)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.LightGreen);


            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Viewport(0, 0, screenWidth, screenHeight);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, screenWidth, screenHeight, 0, -1, 1);

            //TextureRenderer.Begin(screenWidth, screenHeight);

            float xOffset = 0f;
            float yOffset = 0f;
            float zoom = 0.3f;

            Texture2D sprite = TextureProcessing.LoadTexture("C:/Users/Shiro/Downloads/tank.png");
            TextureRenderer.Draw(sprite,
                                 new Vector2(10 * xOffset, 5 * yOffset),
                                 new Vector2(sprite.Width * zoom, sprite.Height * zoom));

            GL.Flush();
            GraphicsContext.CurrentContext.SwapBuffers();
            OpenTkControl.InvalidateVisual();
        }
    }
}
