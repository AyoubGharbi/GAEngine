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

        private List<RawModel> _specialRaw = new List<RawModel>();
        private List<ModelTexture> _specialTexture = new List<ModelTexture>();
        private List<TexturedModel> _specialTextured = new List<TexturedModel>();

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
            _light = new Lights.Light(new Vector3(1, 1f, 1f), new Vector3(0f, 0f, 0f));

            _imGuiController = new ImGuiController(Width, Height);

            _assimpLoader = new AssimpLoader();


            _camera.Move(0, 5f, 0);

            for (int i = 0; i < 2; i++)
            {
                _assimpLoader.LoadModel("res/head.fbx");

                var mesh = _assimpLoader.FirstModel().Meshes[0];
                var raw = _loader.LoadToVAO(positions: mesh.Vertices.Vector3ToFloat(),
                                                texCoords: mesh.TextureCoordinateChannels[0].Vector2ToFloat(),
                                                normals: mesh.Normals.Vector3ToFloat(),
                                                indices: mesh.GetIndices());
                _specialRaw.Add(raw);

                var texture = ContentPipe.LoadTexture2D("res/head.png");
                _specialTexture.Add(texture);

                var rawTextured = new TexturedModel(raw, texture);

                _specialTextured.Add(rawTextured);

                texture.ShineDamper = 10;
                texture.Reflectivity = 1;

                _entities.Add(new Entity("Head", rawTextured, new Vector3(i, 0, -35f), 0, 0, 0f, .5f));
            }
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
                            ImGui.PushID(i);
                            var position = entity.Position.Vector3ToV3();
                            ImGui.SliderFloat3("Position", ref position, -10, 10);

                            var rotationX = entity.RotationX;
                            ImGui.SliderFloat("Rotation X", ref rotationX, -5, 5f);

                            var rotationY = entity.RotationY;
                            ImGui.SliderFloat("Rotation Y", ref rotationY, -5, 5f);

                            var scale = entity.Scaling;
                            ImGui.SliderFloat("Scale", ref scale, 0.1f, 5.0f);

                            var shineDamping = entity.Model.Texture.ShineDamper;
                            ImGui.SliderFloat("Shine Damp", ref shineDamping, 0, 3f);
                            entity.Model.Texture.ShineDamper = shineDamping;

                            var reflectivity = entity.Model.Texture.Reflectivity;
                            ImGui.SliderFloat("Reflectivity", ref reflectivity, 0, 5f);
                            entity.Model.Texture.Reflectivity = reflectivity;

                            entity.Scale(scale);
                            entity.RotationX = rotationX;
                            entity.RotationY = rotationY;
                            entity.Position = position.Vector3ToV3();

                            ImGui.PopID();

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

            _camera.Move();

            foreach (var entity in _entities)
            {
                entity.Rotate(0f, 0.01f, 0f);
            }

            if (_inputsHandler.Data.Escape)
            {
                Exit();
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
