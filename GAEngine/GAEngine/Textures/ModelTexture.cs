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

        private float _shineDamper = 1f;
        private float _reflectivity = 0f;

        public int ID => _id;
        public int Width => _width;
        public int Height => _height;

        public float ShineDamper { get => _shineDamper; set => _shineDamper = value; }
        public float Reflectivity { get => _reflectivity; set => _reflectivity = value; }

        public ModelTexture(int id, int width, int height)
        {
            _id = id;
            _width = width;
            _height = height;
        }
    }
}
