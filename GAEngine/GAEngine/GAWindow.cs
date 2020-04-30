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

        public GAWindow() : base(400, 400, new GraphicsMode(32, 8, 0, 32), "GAEngine")
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
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // clear and append a color
            GL.ClearColor(Color.DarkGray);
            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // projection matrix to define our screen area's coordinates
            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, Width, Height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);

            // model view matrix to define scale, rotation and translation of our "object"
            Matrix4 modelViewMatrix =
                Matrix4.CreateScale(0.5f, 0.5f, 1f) *
                Matrix4.CreateRotationZ(0) *
                Matrix4.CreateTranslation(0f, 0f, 0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelViewMatrix);

            // draw function
            DrawCat();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
        }

        #region Test Methods

        void DrawCat()
        {
            GL.BindTexture(TextureTarget.Texture2D, _catTex.ID);

            // draw cat texture
            GL.Begin(PrimitiveType.Triangles);

            GL.TexCoord2(0, 0); GL.Vertex2(0, 0);
            GL.TexCoord2(1, 1); GL.Vertex2(256, 256);
            GL.TexCoord2(0, 1); GL.Vertex2(0, 256);

            GL.TexCoord2(0, 0); GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0); GL.Vertex2(256, 0);
            GL.TexCoord2(1, 1); GL.Vertex2(256, 256);

            GL.End();
        }
        #endregion
    }
}
