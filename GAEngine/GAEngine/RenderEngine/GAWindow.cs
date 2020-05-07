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
using System.Collections.Generic;
using GAEngine.Utils;
using System.IO;
using OpenTK.Input;
using GAEngine.Inputs;

namespace GAEngine
{
    public class GAWindow : GameWindow
    {
        public GAWindow()
            : base(1280, 720, new GraphicsMode(32, 8, 0, 32), "GAEngine", GameWindowFlags.FixedWindow)
        {
        }
    }
}
