using GAEngine.Entities;
using GAEngine.Models;
using GAEngine.RenderEngine;
using GAEngine.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using GAEngine.IMGUI;
using ImGuiNET;

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

        private ImGuiController _imGuiController;

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

            _imGuiController = new ImGuiController(Width, Height);

            float[] vertices = {
                -0.5f,0.5f,-0.5f,
                -0.5f,-0.5f,-0.5f,
                0.5f,-0.5f,-0.5f,
                0.5f,0.5f,-0.5f,

                -0.5f,0.5f,0.5f,
                -0.5f,-0.5f,0.5f,
                0.5f,-0.5f,0.5f,
                0.5f,0.5f,0.5f,

                0.5f,0.5f,-0.5f,
                0.5f,-0.5f,-0.5f,
                0.5f,-0.5f,0.5f,
                0.5f,0.5f,0.5f,

                -0.5f,0.5f,-0.5f,
                -0.5f,-0.5f,-0.5f,
                -0.5f,-0.5f,0.5f,
                -0.5f,0.5f,0.5f,

                -0.5f,0.5f,0.5f,
                -0.5f,0.5f,-0.5f,
                0.5f,0.5f,-0.5f,
                0.5f,0.5f,0.5f,

                -0.5f,-0.5f,0.5f,
                -0.5f,-0.5f,-0.5f,
                0.5f,-0.5f,-0.5f,
                0.5f,-0.5f,0.5f

            };

            int[] indices = {
                0,1,3,
                3,1,2,
                4,5,7,
                7,5,6,
                8,9,11,
                11,9,10,
                12,13,15,
                15,13,14,
                16,17,19,
                19,17,18,
                20,21,23,
                23,21,22

            };

            float[] texCoords = {
                0,0,
                0,1,
                1,1,
                1,0,
                0,0,
                0,1,
                1,1,
                1,0,
                0,0,
                0,1,
                1,1,
                1,0,
                0,0,
                0,1,
                1,1,
                1,0,
                0,0,
                0,1,
                1,1,
                1,0,
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

            _imGuiController.WindowResized(Width, Height);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _renderer.Prepare();
            _staticShader.Start();
            _renderer.Render(this, _entity, _staticShader, _camera);
            _imGuiController.Render();
            _staticShader.Stop();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            #region IMGUI
            _imGuiController.Update(this, (float)e.Time);

            ImGui.Begin("Entities");

            // cube
            ImGui.BeginChild("Cube");
            var scale = _entity.Scaling;
            ImGui.SliderFloat("Scale", ref scale, 0.1f, 5.0f);
            
            // camera
            ImGui.BeginChild("Camera");
            var x = _camera.Position.X;
            ImGui.SliderFloat("X", ref x, 1,-1);
            var y = _camera.Position.Y;
            ImGui.SliderFloat("Y", ref y, 1, -1);
            var z = _camera.Position.Z;
            ImGui.SliderFloat("Z", ref z, -1, 1);

            ImGui.End();
            #endregion
            _camera.Move(x, y, z);
            _entity.Rotate(0.02f, .02f, 0f);
            _entity.Scale(scale);
        }

        protected override void OnClosed(EventArgs e)
        {
            _loader.CleanUp();
            _staticShader.CleanUp();

            base.OnClosed(e);
        }
    }
}
