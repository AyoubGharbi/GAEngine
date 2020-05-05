using GAEngine.Models;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Entities
{
    class Entity
    {
        private string _entityName;
        private TexturedModel _texturedModel;
        private Vector3 _position;
        private float _rotationX, _rotationY, _rotationZ;
        private float _scale;

        public string EntityName => _entityName;

        public Entity(string name, TexturedModel model, Vector3 position, float rx, float ry, float rz, float scale)
        {
            _entityName = name;
            _texturedModel = model;
            _position = position;
            _rotationX = rx;
            _rotationY = ry;
            _rotationZ = rz;
            _scale = scale;
        }

        public TexturedModel Model { get => _texturedModel; set => _texturedModel = value; }
        public Vector3 Position { get => _position; set => _position = value; }
        public float RotationX { get => _rotationX; set => _rotationX = value; }
        public float RotationY { get => _rotationY; set => _rotationY = value; }
        public float RotationZ { get => _rotationZ; set => _rotationZ = value; }
        public float Scaling { get => _scale; set => _scale = value; }

        public void Move(float x, float y, float z)
        {
            _position.X += x;
            _position.Y += y;
            _position.Z += z;
        }

        public void Rotate(float x, float y, float z)
        {
            _rotationX += x;
            _rotationY += y;
            _rotationZ += z;
        }

        public void Scale(float scale)
        {
            _scale = scale;
        }
    }
}
