using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace GAEngine
{
    public class Window
    {
        private readonly GameWindow _gameWindow;

        public Window(string title, int width, int height)
        {
            _gameWindow = new GameWindow(width, height, GraphicsMode.Default, title);
        }

        public void Start()
        {
            _gameWindow.Resize += OnResize;
            _gameWindow.Load += OnLoadWindow;
            _gameWindow.RenderFrame += OnRender;
            _gameWindow.Run(1.0 / 60.0);
        }

        // resize callback
        private void OnResize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, _gameWindow.Width, _gameWindow.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, 50.0, 0.0, 50.0, -1.0, 1.0);
        }

        // frame render callback
        private void OnRender(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            // draw triangle
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex2(1.0, 1.0);
            GL.Vertex2(25.0, 49.0);
            GL.Vertex2(49.0, 1.0);
            GL.End();
            
            _gameWindow.SwapBuffers();
        }

        // window's loaded callback
        private void OnLoadWindow(object sender, EventArgs e)
        {
            GL.ClearColor(0f, 0f, 0f, 1f);
        }
    }
}
