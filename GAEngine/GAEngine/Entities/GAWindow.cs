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
using GAEngine.AssimpData;
using System.Collections.Generic;
using GAEngine.Utils;
using System.IO;
using OpenTK.Input;
using GAEngine.Inputs;

namespace GAEngine
{
    public class GAWindow : GameWindow
    {
        private ImGuiController _imGuiController;

        private RawModel _specialRaw;
        private ModelTexture _specialTexture;
        private TexturedModel _specialTextured;

        private Loader _loader;
        private Renderer _renderer;
        private Lights.Light _light;
        private AssimpLoader _assimpLoader;
        private StaticShader _staticShader;
        private GAEngine.Entities.Camera _camera;
        private readonly List<Entity> _entities = new List<Entity>();

        private InputsHandler _inputsHandler;

        public GAWindow()
            : base(800, 800, new GraphicsMode(32, 8, 0, 32), "GAEngine", GameWindowFlags.FixedWindow)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            _inputsHandler = new InputsHandler();

            _loader = new Loader();
            _renderer = new Renderer();
            _staticShader = new StaticShader();
            _camera = new GAEngine.Entities.Camera();
            _light = new Lights.Light(new Vector3(1, .5f, .5f), new Vector3(0, 0f, -15f));

            _imGuiController = new ImGuiController(Width, Height);

            _assimpLoader = new AssimpLoader();
            _assimpLoader.LoadModel("res/terrain.fbx");

            var mesh = _assimpLoader.FirstModel().Meshes[0];
            _specialRaw = _loader.LoadToVAO(positions: mesh.Vertices.Vector3ToFloat(),
                                            texCoords: mesh.TextureCoordinateChannels[0].Vector2ToFloat(),
                                            normals: mesh.Normals.Vector3ToFloat(),
                                            indices: mesh.GetIndices());


            _specialTexture = ContentPipe.LoadTexture2D("res/terrainTex.png");
            _specialTextured = new TexturedModel(_specialRaw, _specialTexture);

            _entities.Add(new Entity("Terrain", _specialTextured, new Vector3(0, 0, -2f), 0, 0, 0f, 1f));
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

            foreach (var entity in _entities)
            {
                _renderer.Render(this, entity, _staticShader, _camera, _light);
            }

            _imGuiController.Render();

            _staticShader.Stop();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _inputsHandler.Update();

            #region IMGUI
            _imGuiController.Update(this, (float)e.Time);

            if (ImGui.Begin("Entities"))
            {
                ImGui.Text(string.Format("FPS : {0}", (int)RenderFrequency));

                if (ImGui.BeginTabBar(""))
                {
                    // entities
                    for (int i = 0; i < _entities.Count; i++)
                    {
                        var entity = _entities[i];

                        if (ImGui.BeginTabItem(entity.EntityName))
                        {
                            var position = entity.Position.Vector3ToV3();
                            ImGui.SliderFloat3("Position", ref position, -10, 10);

                            var rotationX = entity.RotationX;
                            ImGui.SliderFloat("Rotation X", ref rotationX, -5, 5f);

                            var rotationY = entity.RotationY;
                            ImGui.SliderFloat("Rotation Y", ref rotationY, -5, 5f);

                            var scale = entity.Scaling;
                            ImGui.SliderFloat("Scale", ref scale, 0.1f, 5.0f);

                            entity.Scale(scale);
                            entity.RotationX = rotationX;
                            entity.RotationY = rotationY;
                            entity.Position = position.Vector3ToV3();

                            ImGui.EndTabItem();
                        }
                    }

                    // camera
                    if (ImGui.BeginTabItem("Camera"))
                    {
                        var fov = _camera.FOV;
                        ImGui.SliderFloat("FOV", ref fov, 0.1f, 100f);
                        _camera.FOV = fov;

                        var position = _camera.Position.Vector3ToV3();
                        ImGui.SliderFloat3("Position", ref position, -15f, 15f);
                        _camera.Move(position.X, position.Y, position.Z);

                        var rotationX = _camera.Yaw;
                        var rotationY = _camera.Pitch;
                        ImGui.SliderFloat("Rotation X", ref rotationX, -15f, 15f);
                        ImGui.SliderFloat("Rotation Y", ref rotationY, -15f, 15f);

                        _camera.Yaw = rotationX;
                        _camera.Pitch = rotationY;

                        ImGui.EndTabItem();
                    }

                    //light
                    if (ImGui.BeginTabItem("Light"))
                    {
                        var position = _light.Position.Vector3ToV3();
                        ImGui.SliderFloat3("Position", ref position, -50f, 50f);
                        _light.Position = position.Vector3ToV3();
                        ImGui.EndTabItem();
                    }

                    ImGui.EndTabBar();
                }

                ImGui.End();
            }
            #endregion

            if (_inputsHandler.Data.Escape)
            {
                Exit();
            }
            for (int i = 0; i < _entities.Count; i++)
            {
                var entity = _entities[i];
                entity.RotationY += _inputsHandler.Data.MouseDeltaX;
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            _loader.CleanUp();
            _staticShader.CleanUp();

            base.OnClosed(e);
        }
    }
}
