using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine
{
    class ModelTexture
    {
        private int _id;
        private int _width;
        private int _height;

        public int ID => _id;
        public int Width => _width;
        public int Height => _height;

        public ModelTexture(int id, int width, int height)
        {
            _id = id;
            _width = width;
            _height = height;
        }
    }
}
