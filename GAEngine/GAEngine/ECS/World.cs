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
        private const string DemoModel = "res/demo/cube.obj";
        private const string DemoTexture = "res/demo/checker.png";

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
            _gaWindow.KeyPress += (sender, e) => _imGuiController?.PressChar(e.KeyChar);

            _gaWindow.Closed += ClosedCallback;
        }

        public void StartGame()
        {
            _gaWindow.VSync = VSyncMode.Off;
            _gaWindow.Run(60.0, 30.0);
        }

        private void LoadCallback(object sender, EventArgs e)
        {
            var width = _gaWindow.Width;
            var height = _gaWindow.Height;

            _camera = new Camera();
            _camera.Move(0f, 0f, 6f);
            _renderer = new Renderer();
            _staticShader = new StaticShader();
            _inputsHandler = new InputsHandler();
            _meshes = new ComponentHandler<MeshComponent>();
            _transforms = new ComponentHandler<TransformComponent>();
            _light = new Lights.Light(Vector3.One, new Vector3(4f, 6f, 5f));

            _imGuiController = new ImGuiController(width, height);

            // A bundled, UV-mapped cube exercises the same Assimp import path as other models.
            var entity = new Entity();
            var demoMesh = new MeshComponent(ContentPaths.Resolve(DemoModel), ContentPaths.Resolve(DemoTexture));
            _meshes.CreateEntity(entity, demoMesh);
            _transforms.CreateEntity(entity,
                new TransformComponent(Vector3.Zero, new Vector3(0.3f, -0.5f, 0f), Vector3.One));
        }

        private void ResizeCallback(object sender, EventArgs e)
        {
            var width = _gaWindow.Width;
            var height = _gaWindow.Height;

            GL.Viewport(0, 0, width, height);

            _imGuiController?.WindowResized(width, height);
        }

        byte[] buffer = Encoding.ASCII.GetBytes("This is just a text");
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

        private void UpdateCallback(object sender, FrameEventArgs e)
        {
            _inputsHandler.Update();

            #region IMGUI
            _imGuiController.Update(_gaWindow, (float)e.Time);

            ImGui.ShowMetricsWindow();

            if (ImGui.Begin("Entities"))
            {
                if (ImGui.Button("Refresh texture"))
                {
                    for (int i = 0; i < _meshes.Count; i++)
                    {
                        var data = _meshes[i];

                        data.Item1.UpdateMeshTexture(ContentPaths.Resolve(DemoTexture));
                    }
                }

                if (ImGui.BeginTabBar("SceneControls"))
                {
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
            }
            ImGui.End();

            #endregion

            #region Tests

            //for (int i = 0; i < _transforms.Count; i++)
            //{
            //    var data = _transforms[i];
            //    data.Item1.Position += new Vector3(0.05f, 0f, 0f);
            //}

            if (_gaWindow.Focused && !ImGui.GetIO().WantCaptureKeyboard)
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
