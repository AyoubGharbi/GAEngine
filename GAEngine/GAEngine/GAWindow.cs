using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace GAEngine
{
    public class GAWindow : GameWindow
    {
        private Texture2D _catTex;

        public GAWindow() : base(900, 600, new GraphicsMode(32, 8, 0, 32), "GAEngine")
        {
            VSync = VSyncMode.On;
            TargetRenderFrequency = 60;
            TargetUpdateFrequency = 30;
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Texture2D);

            _catTex = ContentPipe.LoadTexture2D("Content/cat.png");
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.Ortho(0.0, 50.0, 0.0, 50.0, -1.0, 1.0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.CornflowerBlue);
            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindTexture(TextureTarget.Texture2D, _catTex.ID);

            // draw cat texture
            // texture coordinate: left/top : 0 && bottom/right : 1
            // vertex coordinate : bottom/left : 0 && top/right : 1
            GL.Begin(PrimitiveType.Triangles);
            GL.TexCoord2(0, 0); GL.Vertex2(0, 1);
            GL.TexCoord2(1, 1); GL.Vertex2(1, 0);
            GL.TexCoord2(0, 1); GL.Vertex2(0, 0);

            GL.TexCoord2(0, 0); GL.Vertex2(0, 1);
            GL.TexCoord2(1, 0); GL.Vertex2(1, 1);
            GL.TexCoord2(1, 1); GL.Vertex2(1, 0);
            GL.End();

            // draw triangle
            //GL.Begin(PrimitiveType.Triangles);
            //GL.Color3(Color.Red);
            //GL.Vertex3(0, 0, 0.5f);
            //GL.Color3(Color.Green);
            //GL.Vertex3(1, 0, 0.5f);
            //GL.Color3(Color.Yellow);
            //GL.Vertex3(0, 1, 0.5f);

            //GL.Color4(1f, 1f, 1f, .5f);
            //GL.Vertex3(-.25f, 1, .8f);
            //GL.Vertex3(1, -.25f, .8f);
            //GL.Vertex3(-.25f, -.25f, .8f);
            //GL.End();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
        }
    }
}
