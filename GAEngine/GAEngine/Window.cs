using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace GAEngine
{
    public class GAWindow : GameWindow
    {
        public GAWindow() : base(900, 600, GraphicsMode.Default, "GAEngine")
        {
            VSync = VSyncMode.On;
            TargetRenderFrequency = 60;
            TargetUpdateFrequency = 30;
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0f, 0f, 0f, 1f);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, 50.0, 0.0, 50.0, -1.0, 1.0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // draw triangle
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex2(1.0, 1.0);
            GL.Vertex2(25.0, 49.0);
            GL.Vertex2(49.0, 1.0);
            GL.End();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
        }
    }
}
