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
    class Camera
    {
        private float _yaw;
        private float _roll;
        private float _pitch;
        private Vector3 _position = Vector3.Zero;

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

            if (state.IsKeyDown(Key.W))
            {
                _position.Z -= 0.02f;
            }
            
            if (state.IsKeyDown(Key.D))
            {
                _position.X -= 0.02f;
            }
            
            if (state.IsKeyDown(Key.A))
            {
                _position.X += 0.02f;
            }
        }
    }
}
