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
        public static int ENTITY_GLOBAL_ID = -1;

        private int _entityID;
        private float _scale;
        private Vector3 _position;
        private string _entityName;
        private TexturedModel _texturedModel;
        private float _rotationX, _rotationY, _rotationZ;

        public string EntityName => _entityName;
        public int ID => _entityID;

        public Entity(string name, TexturedModel model, Vector3 position, float rx, float ry, float rz, float scale)
        {
            _entityName = name;
            _texturedModel = model;
            _position = position;
            _rotationX = rx;
            _rotationY = ry;
            _rotationZ = rz;
            _scale = scale;

            _entityID = ++ENTITY_GLOBAL_ID;
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
