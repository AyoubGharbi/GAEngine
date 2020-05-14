using GAEngine.ECS;
using GAEngine.Models;
using GAEngine.RenderEngine;
using GAEngine.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Components
{
    class MeshComponent : IComponent
    {
        private Loader _loader;
        private TexturedModel _texturedModel;

        public TexturedModel TexturedModel => _texturedModel;

        public MeshComponent(string modelPath, string texturePath)
        {
            _loader = new Loader();

            var model = _loader.LoadModel(modelPath);

            var mesh = model.Meshes[0];
            var raw = _loader.LoadToVAO(positions: mesh.Vertices.Vector3ToFloat(),
                                            texCoords: mesh.TextureCoordinateChannels[0].Vector2ToFloat(),
                                            normals: mesh.Normals.Vector3ToFloat(),
                                            indices: mesh.GetIndices());

            var texture = ContentPipe.LoadTexture2D(texturePath);

            texture.ShineDamper = 10f;
            texture.Reflectivity = 0.01f;

            _texturedModel = new TexturedModel(raw, texture);
        }

        public void UpdateMeshTexture(string texturePath)
        {
            var texture = ContentPipe.LoadTexture2D(texturePath);

            _texturedModel.Texture = texture;
        }

        public override void CleanUp()
        {
            _loader.CleanUp();
        }
    }

    class TransformComponent : IComponent
    {
        private Vector3 _scale;
        private Vector3 _position;
        private Vector3 _rotation;

        public Vector3 Scale { get => _scale; set => _scale = value; }
        public Vector3 Position { get => _position; set => _position = value; }
        public Vector3 Rotation { get => _rotation; set => _rotation = value; }

        public TransformComponent(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            _scale = scale;
            _position = position;
            _rotation = rotation;
        }

        public override void CleanUp()
        {
        }
    }
}
