﻿using OpenTK;
using OpenTK.Input;
using System;

namespace GAEngine.Entities
{
    public class Camera
    {
        // initial field of view
        private float _fov = 60.0f;
        public float FOV { get => _fov; set => _fov = value; }

        // rotation on the y axis
        private float _yaw;
        public float Yaw { get => _yaw; set => _yaw = value; }

        // rotation on the z axis
        private float _roll;
        public float Roll { get => _roll; set => _roll = value; }

        // rotation on the x axis
        private float _pitch = 0.0f;
        public float Pitch { get => _pitch; set => _pitch = value; }

        public Vector3 Position => _position;

        private Vector3 _position = Vector3.Zero;

        public void Move()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Key.W))
            {
                _position.Z -= 0.5f;
            }

            if (state.IsKeyDown(Key.S))
            {
                _position.Z += 0.5f;
            }

            if (state.IsKeyDown(Key.A))
            {
                _position.X -= 0.5f;
            }

            if (state.IsKeyDown(Key.D))
            {
                _position.X += 0.5f;
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
