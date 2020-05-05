using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Entities
{
    public class Camera
    {
        private float _fov = 60.0f;
        public float FOV { get => _fov; set => _fov = value; }

        private float _yaw;
        private float _roll;
        private float _pitch;
        private Vector3 _position = new Vector3(0f,0f,0f);

        public float Yaw => _yaw;
        public float Roll => _roll;
        public float Pitch => _pitch;
        public Vector3 Position => _position;

        public Camera()
        {
        }

        public void Move()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Key.Down))
            {
                Console.WriteLine("Move (-Z)");
                _position.Z -= 0.02f;
            }

            if (state.IsKeyDown(Key.Up))
            {
                Console.WriteLine("Move (Z)");
                _position.Z += 0.02f;
            }

            if (state.IsKeyDown(Key.D))
            {
                Console.WriteLine("Move (X)");
                _position.X -= 0.02f;
            }

            if (state.IsKeyDown(Key.A))
            {
                Console.WriteLine("Move (-X)");
                _position.X += 0.02f;
            }
        }

        public void Move(float x, float y, float z)
        {
            _position.X = x;
            _position.Y = y;
            _position.Z = z;
        }
    }
}
