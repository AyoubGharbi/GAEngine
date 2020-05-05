﻿using GAEngine.Entities;
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

        public GAWindow() : base(800, 800, new GraphicsMode(32, 8, 0, 32), "GAEngine")
        {
            TargetRenderFrequency = 60;
        }

        protected override void OnLoad(EventArgs e)
        {
            _loader = new Loader();
            _renderer = new Renderer();
            _staticShader = new StaticShader();
            _camera = new GAEngine.Entities.Camera();
            _light = new Lights.Light(new Vector3(1, 1, 1), new Vector3(0, 0f, -15f));

            //testing purpose
            //_camera.Move(-4.3f, 4.3f, 21f);
            //_light.Position = new Vector3(0f, 15f, 3.5f);

            _imGuiController = new ImGuiController(Width, Height);

            _assimpLoader = new AssimpLoader();

            var mesh = _assimpLoader.Model.Meshes[0];
            _specialRaw = _loader.LoadToVAO(positions: mesh.Vertices.Vector3ToFloat(),
                                            texCoords: mesh.TextureCoordinateChannels[0].Vector2ToFloat(),
                                            normals: mesh.Normals.Vector3ToFloat(),
                                            indices: mesh.GetIndices());


            _specialTexture = ContentPipe.LoadTexture2D("Res/terrainTex.png");
            _specialTextured = new TexturedModel(_specialRaw, _specialTexture);

            _entities.Add(new Entity("Terrain", _specialTextured, new Vector3(0, 0, -10f), 0, 0, 0f, 1f));
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            _assimpLoader.Resize(Width, Height);
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
                            var rotationX = _entities[i].RotationX;
                            ImGui.SliderFloat("Rotation X", ref rotationX, -5, 5f);

                            var rotationY = _entities[i].RotationY;
                            ImGui.SliderFloat("Rotation Y", ref rotationY, -5, 5f);

                            var scale = _entities[i].Scaling;
                            ImGui.SliderFloat("Scale", ref scale, 0.1f, 5.0f);

                            _entities[i].Scale(scale);
                            _entities[i].RotationX = rotationX;
                            _entities[i].RotationY = rotationY;

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
                        ImGui.SliderFloat3("Position", ref position, -50, 50);
                        _camera.Move(position.X, position.Y, position.Z);

                        var rotationX = _camera.Yaw;
                        var rotationY = _camera.Pitch;
                        ImGui.SliderFloat("Rotation X", ref rotationX, -50f,50f);
                        ImGui.SliderFloat("Rotation Y", ref rotationY, -50, 50f);

                        _camera.Yaw = rotationX;
                        _camera.Pitch = rotationY;

                        ImGui.EndTabItem();
                    }

                    //light
                    if (ImGui.BeginTabItem("Light"))
                    {
                        var position = _light.Position.Vector3ToV3();
                        ImGui.SliderFloat3("Position", ref position, -180f, 180f);
                        _light.Position = position.Vector3ToV3();
                        ImGui.EndTabItem();
                    }

                    ImGui.EndTabBar();
                }

                ImGui.End();
            }
            #endregion

            foreach (var entity in _entities)
            {
                entity.Rotate(0f, 0.0001f, 0f);
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            _assimpLoader.Unload();
        }

        protected override void OnClosed(EventArgs e)
        {
            _loader.CleanUp();
            _staticShader.CleanUp();

            base.OnClosed(e);
        }
    }
}
