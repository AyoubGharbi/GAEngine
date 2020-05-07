using GAEngine.Components;
using GAEngine.ECS;
using GAEngine.Entities;
using GAEngine.IMGUI;
using GAEngine.Inputs;
using GAEngine.Managers;
using GAEngine.Models;
using GAEngine.RenderEngine;
using GAEngine.Shaders;
using GAEngine.Utils;
using ImGuiNET;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GAEngine
{
    public class World
    {
        // OpenTK window wrapper
        private GAWindow _gaWindow;

        // ImGui wrapper (GUI stuff)
        private ImGuiController _imGuiController;

        private Camera _camera;
        private Renderer _renderer;
        private Lights.Light _light;
        private StaticShader _staticShader;
        private InputsHandler _inputsHandler;

        // ecs system
        private ComponentHandler<MeshComponent> _meshes;
        private ComponentHandler<TransformComponent> _transforms;

        public World()
        {
            _gaWindow = new GAWindow();

            // window callbacks
            _gaWindow.Load += LoadCallback;
            _gaWindow.Resize += ResizeCallback;
            _gaWindow.RenderFrame += RenderCallback;
            _gaWindow.UpdateFrame += UpdateCallback;

            _gaWindow.Closed += ClosedCallback;
        }

        public void StartGame()
        {
            _gaWindow.VSync = VSyncMode.Off;
            _gaWindow.Run(120.0, 120.0);
        }

        private void LoadCallback(object sender, EventArgs e)
        {
            var width = _gaWindow.Width;
            var height = _gaWindow.Height;

            _camera = new Camera();
            _renderer = new Renderer();
            _staticShader = new StaticShader();
            _inputsHandler = new InputsHandler();
            _meshes = new ComponentHandler<MeshComponent>();
            _transforms = new ComponentHandler<TransformComponent>();
            _light = new Lights.Light(new Vector3(1, 1f, 1f), new Vector3(0f, 0f, 0f));

            _imGuiController = new ImGuiController(width, height);

            /// Optimize more this
            // 2500 entities => ~30 FPS
            var entity = new Entity();
            var headMesh = new MeshComponent("res/terrain.fbx", "res/color_hex_00.png");

            _meshes.CreateEntity(entity, headMesh);
            _transforms.CreateEntity(entity,
                new TransformComponent(
                    new Vector3(0, 0f, -50f),
                    Vector3.Zero,
                    Vector3.One));
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

            for (int i = 0; i < _meshes.Count; i++)
            {
                var data = _meshes[i];
                var transformData = _transforms[i];
                _renderer.Render(_gaWindow, data.Item1, transformData.Item1, _staticShader, _camera, _light);
            }


            _imGuiController.Render();

            _staticShader.Stop();

            _gaWindow.SwapBuffers();
        }
        string input = "jetpack";

        private void UpdateCallback(object sender, FrameEventArgs e)
        {
            _inputsHandler.Update();

            #region IMGUI
            _imGuiController.Update(_gaWindow, (float)e.Time);

            ImGui.ShowMetricsWindow();

            if (ImGui.Begin("Entities"))
            {
                ImGui.Text(string.Format("FPS : {0}", (int)_gaWindow.RenderFrequency));

                if (ImGui.BeginTabBar(""))
                {
                    // entities
                    for (int i = 0; i < _meshes.Count; i++)
                    {
                        ImGui.PushID(i);
                        var data = _meshes[i];
                        var entity = data.Item2;
                        var entityMesh = data.Item1;

                        var transformData = _transforms[i];
                        var transformEntity = transformData.Item1;

                        if (ImGui.BeginTabItem(entity.ID.ToString()))
                        {
                            var transScale = transformEntity.Scale.Vector3ToV3();
                            float scale = transScale.X;
                            ImGui.SliderFloat("Scale", ref scale, 0.1f, 5.0f);
                            transformEntity.Scale = new Vector3(scale, scale, scale);

                            var position = transformEntity.Position.Vector3ToV3();
                            ImGui.SliderFloat3("Position", ref position, -500f, 500f);
                            transformEntity.Position = position.Vector3ToV3();

                            var meshModel = entityMesh.TexturedModel;
                            var shineDamping = meshModel.Texture.ShineDamper;
                            ImGui.SliderFloat("Shine Damp", ref shineDamping, 0, 3f);
                            meshModel.Texture.ShineDamper = shineDamping;

                            var reflectivity = meshModel.Texture.Reflectivity;
                            ImGui.SliderFloat("Reflectivity", ref reflectivity, 0, 5f);
                            meshModel.Texture.Reflectivity = reflectivity;

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
                        ImGui.SliderFloat3("Position", ref position, -500f, 500f);
                        _camera.Move(position.X, position.Y, position.Z);

                        var rotationX = _camera.Yaw;
                        var rotationY = _camera.Pitch;
                        ImGui.SliderFloat("Rotation X", ref rotationX, -500f, 500f);
                        ImGui.SliderFloat("Rotation Y", ref rotationY, -500f, 500f);

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

            //for (int i = 0; i < _transforms.Count; i++)
            //{
            //    var data = _transforms[i];
            //    data.Item1.Position += new Vector3(0.05f, 0f, 0f);
            //}

            _camera.Move();

            // exit game
            if (_inputsHandler.Data.Escape)
            {
                _gaWindow.Exit();
            }
            #endregion
        }

        private void ClosedCallback(object sender, EventArgs e)
        {
            _meshes.CleanUp();
            _transforms.CleanUp();
            _staticShader.CleanUp();
        }
    }
}
