using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine
{
    class Texture2D
    {
        private int _id;
        private int _width;
        private int _height;

        public int ID => _id;
        public int Width => _width;
        public int Height => _height;

        public Texture2D(int id, int width, int height)
        {
            _id = id;
            _width = width;
            _height = height;
        }
    }
}
