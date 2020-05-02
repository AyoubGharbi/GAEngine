using GAEngine.Entities;
using GAEngine.Models;
using GAEngine.RenderEngine;
using GAEngine.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace GAEngine
{
    public class GAWindow : GameWindow
    {
        private Loader _loader;
        private Renderer _renderer;
        private RawModel _rawModel;
        private StaticShader _staticShader;
        private ModelTexture _modelTexture;
        private TexturedModel _texturedModel;
        private Entity _entity;
        private Camera _camera;

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
            _staticShader = new StaticShader();
            _camera = new Camera();

            float[] vertices ={
                -0.5f,  0.5f, 0f,// V0
                -0.5f, -0.5f, 0f,// V1
                 0.5f, -0.5f, 0f,// V2
                 0.5f,  0.5f, 0f,// V3
            };

            int[] indices ={
                0, 1, 3,
                3, 1, 2
            };

            float[] texCoords ={
                0,0,
                0,1,
                1,1,
                1,0
            };

            _rawModel = _loader.LoadToVAO(vertices, texCoords, indices);
            _modelTexture = ContentPipe.LoadTexture2D("Textures/cat.png");
            _texturedModel = new TexturedModel(_rawModel, _modelTexture);

            _entity = new Entity(_texturedModel, new Vector3(0, 0, -5f), 0, 0, 0f, 1f);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _renderer.Prepare();
            _staticShader.Start();
            _renderer.Render(this, _entity, _staticShader,_camera);
            _staticShader.Stop();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _camera.Move();
            //_entity.Move(0f, 0, -.2f);
            //_entity.Rotate(0f, .1f, 0f);
        }

        protected override void OnClosed(EventArgs e)
        {
            _loader.CleanUp();
            _staticShader.CleanUp();

            base.OnClosed(e);
        }
    }
}
