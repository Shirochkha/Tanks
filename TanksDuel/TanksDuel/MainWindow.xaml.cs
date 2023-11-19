using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TanksDuel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameWindow window;
        private DispatcherTimer timer;
        private WriteableBitmap bitmap;

        public MainWindow()
        {
            InitializeComponent();

            SetupViewPort();
        }

        public void SetupViewPort()
        {
            this.window = new GameWindow(500, 500, GraphicsMode.Default, "OpenGL Hidden Window");
            this.MakeContextCurrent(true);

            this.window.Size = new System.Drawing.Size(500, 500);
            GL.Viewport(0, 0, 500, 500);

            /*GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, 500, 0, 500, -1d, 1d);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();*/

            this.MakeContextCurrent(false);

            this.bitmap = new WriteableBitmap(500, 500, 96, 96, PixelFormats.Rgb24, null);
            this.ViewPort.Source = this.bitmap;

            this.timer = new DispatcherTimer(DispatcherPriority.Render);
            this.timer.Interval = TimeSpan.FromMilliseconds(1000d / 30d); //30fps
            this.timer.Tick += this.Timer_Tick;
            this.timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.MakeContextCurrent(true);

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            //render

            GL.PushMatrix();
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(-0.5, 0.5, 0);

            GL.Color3(0f, 1f, 0f);
            GL.Vertex3(0.5, 0.5, 0);

            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(0.5, -0.5, 0);

            GL.Color3(1f, 1f, 1f);
            GL.Vertex3(-0.5, -0.5, 0);

            GL.End();
            GL.PopMatrix();

            //Transfer pixels from OpenGL to WPF

            GL.ReadBuffer(ReadBufferMode.Back);
            this.bitmap.Lock();
            GL.ReadPixels(0, 0, 500, 500, OpenTK.Graphics.OpenGL.PixelFormat.Rgb, PixelType.UnsignedByte,
                this.bitmap.BackBuffer);
            this.bitmap.AddDirtyRect(new Int32Rect(0, 0, 500, 500));
            this.bitmap.Unlock();

            this.MakeContextCurrent(false);

        }

        public void MakeContextCurrent(bool valid)
        {
            if (valid)
            {
                this.window.MakeCurrent();
            }
            else
            {
                this.window.Context.MakeCurrent(null);
            }
        }
    }
}
