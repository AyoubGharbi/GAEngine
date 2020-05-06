using Assimp;
using Assimp.Configs;
using GAEngine.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace GAEngine.AssimpData
{
    class AssimpLoader
    {
        private List<Scene> _models = new List<Scene>();
        public List<Scene> AllModels => _models;

        public void LoadModel(string modelPath)
        {
            AssimpContext importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

            _models.Add(importer.ImportFile(modelPath, PostProcessPreset.TargetRealTimeMaximumQuality));
        }

        public Scene FirstModel()
        {
            if (_models.Count <= 0)
                return null;

            return _models[0];
        }
    }
}
