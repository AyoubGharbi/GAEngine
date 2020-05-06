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
        public List<Scene> Models => _models;

        public Scene LoadModel(string modelPath)
        {
            AssimpContext importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

            var model = importer.ImportFile(modelPath, PostProcessPreset.TargetRealTimeMaximumQuality);

            _models.Add(model);

            return model;
        }
    }
}
