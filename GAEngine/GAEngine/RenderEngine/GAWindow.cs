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
            : base(800, 600, new GraphicsMode(32, 24, 0, 0), "GAEngine", GameWindowFlags.FixedWindow,
                   DisplayDevice.Default, 4, 6, GraphicsContextFlags.ForwardCompatible)
        {
        }
    }
}
