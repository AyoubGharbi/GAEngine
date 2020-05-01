using GAEngine.Entities;
using GAEngine.Models;
using GAEngine.Shaders;
using GAEngine.Utils;
using OpenTK;
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

        public void Render(Entity entity, StaticShader shader)
        {
            TexturedModel texturedModel = entity.Model;
            RawModel rawModel = texturedModel.Raw;
            GL.BindVertexArray(rawModel.VAOID);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            Matrix4 transformMatrix = UtilsMath.CreateTransformMatrix(entity.Position,
                                                                      entity.RotationX,
                                                                      entity.RotationY,
                                                                      entity.RotationZ,
                                                                      entity.Scale);
            shader.LoadTransformMatrix(transformMatrix);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturedModel.Texture.ID);
            GL.DrawElements(PrimitiveType.Triangles, rawModel.VertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindVertexArray(0);
        }
    }
}
