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

        public RawModel Raw => _rawModel;
        public ModelTexture Texture => _modelTexture;

        public TexturedModel(RawModel model, ModelTexture modelTexture)
        {
            _rawModel = model;
            _modelTexture = modelTexture;
        }
    }
}
