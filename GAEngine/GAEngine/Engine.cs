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
    public class Engine
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
        private ComponentManager<MeshComponent> _meshes;
        private ComponentManager<TransformComponent> _transforms;

        public Engine()
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

            _camera = new Camera();
            _renderer = new Renderer();
            _staticShader = new StaticShader();
            _inputsHandler = new InputsHandler();
            _meshes = new ComponentManager<MeshComponent>();
            _transforms = new ComponentManager<TransformComponent>();
            _light = new Lights.Light(new Vector3(1, 1f, 1f), new Vector3(0f, 0f, 0f));

            _imGuiController = new ImGuiController(width, height);

            for (int x = 0; x < 20; x++)
            {
                for (int z = 0; z < 20; z++)
                {
                    var entity = new Entity();

                    _meshes.CreateEntity(entity, new MeshComponent("res/head.fbx", "res/head.png"));
                    _transforms.CreateEntity(entity,
                        new TransformComponent(
                            new Vector3(-15f * x, 0f, -100f * z),
                            Vector3.Zero,
                            new Vector3(.8f, .8f, .8f)));
                }
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

            if (ImGui.Begin("Entities"))
            {
                ImGui.Text(string.Format("FPS : {0}", (int)_gaWindow.RenderFrequency));

                if (ImGui.BeginTabBar(""))
                {
                    //// entities
                    //for (int i = 0; i < _entities.Count; i++)
                    //{
                    //    ImGui.PushID(i);

                    //    var entity = _entities[i];

                    //    if (ImGui.BeginTabItem(entity.ID.ToString()))
                    //    {
                    //        var position = entity.Position.Vector3ToV3();
                    //        ImGui.SliderFloat3("Position", ref position, -10, 10);

                    //        var rotationX = entity.RotationX;
                    //        ImGui.SliderFloat("Rotation X", ref rotationX, -5, 5f);

                    //        var rotationY = entity.RotationY;
                    //        ImGui.SliderFloat("Rotation Y", ref rotationY, -5, 5f);

                    //        var scale = entity.Scaling;
                    //        ImGui.SliderFloat("Scale", ref scale, 0.1f, 5.0f);

                    //        var shineDamping = entity.Model.Texture.ShineDamper;
                    //        ImGui.SliderFloat("Shine Damp", ref shineDamping, 0, 3f);
                    //        entity.Model.Texture.ShineDamper = shineDamping;

                    //        var reflectivity = entity.Model.Texture.Reflectivity;
                    //        ImGui.SliderFloat("Reflectivity", ref reflectivity, 0, 5f);
                    //        entity.Model.Texture.Reflectivity = reflectivity;

                    //        entity.Scale(scale);
                    //        entity.RotationX = rotationX;
                    //        entity.RotationY = rotationY;
                    //        entity.Position = position.Vector3ToV3();

                    //        ImGui.EndTabItem();
                    //    }

                    //    ImGui.PopID();
                    //}

                    // camera
                    if (ImGui.BeginTabItem("Camera"))
                    {
                        var fov = _camera.FOV;
                        ImGui.SliderFloat("FOV", ref fov, 0.1f, 100f);
                        _camera.FOV = fov;

                        var position = _camera.Position.Vector3ToV3();
                        ImGui.SliderFloat3("Position", ref position, -150f, 150f);
                        _camera.Move(position.X, position.Y, position.Z);

                        var rotationX = _camera.Yaw;
                        var rotationY = _camera.Pitch;
                        ImGui.SliderFloat("Rotation X", ref rotationX, -15f, 15f);
                        ImGui.SliderFloat("Rotation Y", ref rotationY, -50f, 50f);

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
