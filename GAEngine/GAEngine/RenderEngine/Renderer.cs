using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.RenderEngine
{
    class Renderer
    {
        public void Prepare()
        {
            GL.ClearColor(1f, 1f, 1f, 1f);
        }
        
        public void Render(RawModel model)
        {
            GL.BindVertexArray(model.VAOID);
            GL.EnableVertexAttribArray(0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, model.VertexCount);
            GL.BindVertexArray(0);
        }
    }
}
