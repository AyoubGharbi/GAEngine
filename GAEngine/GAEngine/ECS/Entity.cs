using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.ECS
{
    class Entity
    {
        public static uint IDS = 0;


        private uint _id;

        public uint ID => _id;

        public Entity()
        {
            _id = ++IDS;
        }
    }
}
