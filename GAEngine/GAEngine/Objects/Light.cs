using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Lights
{
    class Light
    {
        private Vector3 _color;
        private Vector3 _position;

        public Light(Vector3 color, Vector3 pos)
        {
            _color = color;
            _position = pos;
        }

        public Vector3 Color { get => _color; set => _color = value; }
        public Vector3 Position { get => _position; set => _position = value; }
    }
}
