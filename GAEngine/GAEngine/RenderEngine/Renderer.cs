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
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(0f, 0f, 0f, 1f);
        }

        public void Render(RawModel model)
        {
            GL.BindVertexArray(model.VAOID);
            GL.EnableVertexAttribArray(0);
            GL.DrawElements(PrimitiveType.Triangles, model.VertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }
    }
}
