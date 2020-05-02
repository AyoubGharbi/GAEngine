using GAEngine.Entities;
using GAEngine.Models;
using GAEngine.Shaders;
using GAEngine.Utils;
using OpenTK;
using OpenTK.Graphics.ES30;
using System;
using System.Drawing;

namespace GAEngine.RenderEngine
{
    class Renderer
    {
        private float _fov = 45.0f;
        public float FOV { get => _fov; set => _fov = value; }

        public void Prepare()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            //GL.ClearColor(Color.CornflowerBlue);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        }

        public void Render(GAWindow window, Entity entity, StaticShader shader)
        {
            TexturedModel texturedModel = entity.Model;
            RawModel rawModel = texturedModel.Raw;
            GL.BindVertexArray(rawModel.VAOID);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fov),
                                                                           (float)window.Width / (float)window.Height, 0.1f, 1000.0f);

            shader.LoadProjectionMatrix(projectionMatrix);

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
