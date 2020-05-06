using GAEngine.AssimpData;
using GAEngine.Entities;
using GAEngine.IMGUI;
using GAEngine.Inputs;
using GAEngine.Models;
using GAEngine.RenderEngine;
using GAEngine.Shaders;
using GAEngine.Utils;
using ImGuiNET;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GAEngine
{
    public class Game
    {
        // openTK window wrapper
        private GAWindow _gaWindow;

        // ImGui wrapper (GUI stuff)
        private ImGuiController _imGuiController;

        // TODO: move this to a proper entity manager
        private List<RawModel> _specialRaw = new List<RawModel>();
        private readonly List<Entity> _entities = new List<Entity>();
        private List<ModelTexture> _specialTexture = new List<ModelTexture>();
        private List<TexturedModel> _specialTextured = new List<TexturedModel>();

        private Loader _loader;
        private Camera _camera;
        private Renderer _renderer;
        private Lights.Light _light;
        private AssimpLoader _assimpLoader;
        private StaticShader _staticShader;
        private InputsHandler _inputsHandler;

        public Game()
        {
            _gaWindow = new GAWindow();

            _gaWindow.Load += LoadCallback;
            _gaWindow.Resize += ResizeCallback;
            _gaWindow.RenderFrame += RenderCallback;
            _gaWindow.UpdateFrame += UpdateCallback;

            _gaWindow.Closed += ClosedCallback;
        }

        public void StartGame()
        {
            _gaWindow.Run();
        }

        private void LoadCallback(object sender, EventArgs e)
        {
            var width = _gaWindow.Width;
            var height = _gaWindow.Height;

            _inputsHandler = new InputsHandler();

            _loader = new Loader();
            _renderer = new Renderer();
            _staticShader = new StaticShader();
            _camera = new GAEngine.Entities.Camera();
            _light = new Lights.Light(new Vector3(1, 1f, 1f), new Vector3(0f, 0f, 0f));

            _imGuiController = new ImGuiController(width, height);

            _assimpLoader = new AssimpLoader();

            for (int i = 0; i < 5; i++)
            {
                AddEntity("Head", "res/head.fbx", "res/head.png", new Vector3(i * 5f - 0.2f, 0f, -35f));

                //AddEntity("JetPack", "res/jetpack.fbx", "res/jetpack.png");
            }
        }

        private void ResizeCallback(object sender, EventArgs e)
        {
            var width = _gaWindow.Width;
            var height = _gaWindow.Height;

            GL.Viewport(0, 0, width, height);

            _imGuiController.WindowResized(width, height);
        }

        private void RenderCallback(object sender, FrameEventArgs e)
        {
            _renderer.Prepare();
            _staticShader.Start();

            foreach (var entity in _entities)
            {
                _renderer.Render(_gaWindow, entity, _staticShader, _camera, _light);
            }

            _imGuiController.Render();

            _staticShader.Stop();

            _gaWindow.SwapBuffers();
        }

        private void UpdateCallback(object sender, FrameEventArgs e)
        {
            _inputsHandler.Update();

            #region IMGUI
            _imGuiController.Update(_gaWindow, (float)e.Time);

            if (ImGui.Begin("Entities"))
            {
                ImGui.Text(string.Format("FPS : {0}", (int)_gaWindow.RenderFrequency));

                if (ImGui.BeginTabBar(""))
                {
                    // entities
                    for (int i = 0; i < _entities.Count; i++)
                    {
                        ImGui.PushID(i);

                        var entity = _entities[i];
                        if (ImGui.BeginTabItem(entity.ID.ToString()))
                        {
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

                            if (ImGui.Button("Change Entity"))
                            {
                                UpdateEntity(entity.ID, "res/terrain.fbx", "res/terrainTex.png");
                            }

                            ImGui.EndTabItem();
                        }
                        ImGui.PopID();
                    }

                    // camera
                    if (ImGui.BeginTabItem("Camera"))
                    {
                        var fov = _camera.FOV;
                        ImGui.SliderFloat("FOV", ref fov, 0.1f, 100f);
                        _camera.FOV = fov;

                        var position = _camera.Position.Vector3ToV3();
                        ImGui.SliderFloat3("Position", ref position, -50f, 50f);
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

            #region Tests
            _camera.Move();

            foreach (var entity in _entities)
            {
                entity.Rotate(0f, 0.01f, 0f);
            }

            // popup entity
            if (_inputsHandler.Data.Space)
            {
                //_entities.Remove(_entities.Last());
                //UpdateEntity(0, "res/terrain.fbx", "res/terrainTex.png");
            }

            // exit game
            if (_inputsHandler.Data.Escape)
            {
                _gaWindow.Exit();
            }
            #endregion
        }

        private void ClosedCallback(object sender, EventArgs e)
        {
            _loader.CleanUp();
            _staticShader.CleanUp();
        }

        void AddEntity(string name, string modelPath, string texturePath, Vector3 position)
        {
            var model = _assimpLoader.LoadModel(modelPath);

            var mesh = model.Meshes[0];
            var raw = _loader.LoadToVAO(positions: mesh.Vertices.Vector3ToFloat(),
                                            texCoords: mesh.TextureCoordinateChannels[0].Vector2ToFloat(),
                                            normals: mesh.Normals.Vector3ToFloat(),
                                            indices: mesh.GetIndices());
            _specialRaw.Add(raw);

            var texture = ContentPipe.LoadTexture2D(texturePath);
            _specialTexture.Add(texture);

            var rawTextured = new TexturedModel(raw, texture);

            _specialTextured.Add(rawTextured);

            texture.ShineDamper = 10;
            texture.Reflectivity = 1;

            _entities.Add(new Entity(name, rawTextured, position, 0, 0, 0f, .2f));
        }

        void UpdateEntity(int entityID, string newModel, string newTexture)
        {
            var arrayID = entityID;
            var entity = _entities[arrayID];

            var model = _assimpLoader.LoadModel(newModel);

            var mesh = model.Meshes[0];
            var raw = _loader.LoadToVAO(positions: mesh.Vertices.Vector3ToFloat(),
                                            texCoords: mesh.TextureCoordinateChannels[0].Vector2ToFloat(),
                                            normals: mesh.Normals.Vector3ToFloat(),
                                            indices: mesh.GetIndices());


            var texture = ContentPipe.LoadTexture2D(newTexture);

            _specialRaw[arrayID] = raw;
            _specialTexture[arrayID] = texture;

            entity.Model.Raw = raw;
            entity.Model.Texture = texture;

            entity.Scale(5f);
        }
    }
}
