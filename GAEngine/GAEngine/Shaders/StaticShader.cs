using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Shaders
{
    class StaticShader : ShaderProgram
    {
        private const string VERTEX_FILE = "Shaders/VertexShader.txt";
        private const string FRAGMENT_FILE = "Shaders/FragmentShader.txt";

        public StaticShader() : base(VERTEX_FILE, FRAGMENT_FILE) { }

        protected override void BindAttributes()
        {
            base.BindAttribute(0, "position");
        }
    }
}
