using GAEngine.Models;
using OpenTK.Graphics.ES30;
using System;

namespace GAEngine.RenderEngine
{
    class Renderer
    {
        public void Prepare()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(0f, 0f, 0f, 1f);
        }

        public void Render(TexturedModel texturedModel)
        {
            RawModel rawModel = texturedModel.Raw;
            GL.BindVertexArray(rawModel.VAOID);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturedModel.Texture.ID);
            GL.DrawElements(PrimitiveType.Triangles, rawModel.VertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindVertexArray(0);
        }
    }
}
