using GAEngine.RenderEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Models
{
    class TexturedModel
    {
        private RawModel _rawModel;
        private ModelTexture _modelTexture;

        public RawModel Raw { get => _rawModel; set => _rawModel = value; }

        public ModelTexture Texture { get => _modelTexture; set => _modelTexture = value; }

        public TexturedModel(RawModel model, ModelTexture modelTexture)
        {
            _rawModel = model;
            _modelTexture = modelTexture;
        }
    }
}
