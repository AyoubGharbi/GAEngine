using GAEngine.RenderEngine;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace GAEngine
{
    public class GAWindow : GameWindow
    {
        private Loader _loader;
        private Renderer _renderer;
        private RawModel _rawModel;


        private Texture2D _catTex;
        public GAWindow() : base(800, 800, new GraphicsMode(32, 8, 0, 32), "GAEngine")
        {
            VSync = VSyncMode.On;
            TargetRenderFrequency = 60;
            TargetUpdateFrequency = 30;
        }

        protected override void OnLoad(EventArgs e)
        {
            _loader = new Loader();
            _renderer = new Renderer();

            float[] vertices = {
                -0.5f,  0.5f, 0f,
                -0.5f, -0.5f, 0f,
                 0.5f, -0.5f, 0f,
                 0.5f, -0.5f, 0f,
                 0.5f,  0.5f, 0f,
                -0.5f,  0.5f, 0f
              };

            _rawModel = _loader.LoadToVAO(vertices);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _renderer.Prepare();

            _renderer.Render(_rawModel);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {

        }

        protected override void OnClosed(EventArgs e)
        {
            _loader.CleanUp();


            base.OnClosed(e);
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

        void ManipulateMatrices()
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

            // model view matrix to define scale, rotation and translation of our "object"
            modelViewMatrix =
                Matrix4.CreateScale(1.2f, 1.2f, 1f) *
                Matrix4.CreateRotationZ(25f) *
                Matrix4.CreateTranslation(256f, 256f, 0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelViewMatrix);

            // draw function
            DrawCat();

            // model view matrix to define scale, rotation and translation of our "object"
            modelViewMatrix =
                Matrix4.CreateScale(1.2f, 1.2f, 1f) *
                Matrix4.CreateRotationZ(-25f) *
                Matrix4.CreateTranslation(200f, 256f, 0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelViewMatrix);

            // draw function
            DrawCat();

        }

        void LoadStuff()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, 0.7f);

            _catTex = ContentPipe.LoadTexture2D("Content/cat.png");
        }
        #endregion
    }
}
