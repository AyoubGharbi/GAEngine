using GAEngine.Entities;
using GAEngine.Lights;
using GAEngine.Models;
using GAEngine.Shaders;
using GAEngine.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace GAEngine.RenderEngine
{
    class Renderer
    {
        public void Prepare()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
        }

        public void Render(GAWindow window, Entity entity, StaticShader shader, Camera camera, Light light)
        {
            TexturedModel texturedModel = entity.Model;
            RawModel rawModel = texturedModel.Raw;
            GL.BindVertexArray(rawModel.VAOID);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            shader.LoadLight(light);

            Matrix4 viewMatrix = UtilsMath.CreateViewMatrix(camera);
            shader.LoadViewMatrix(viewMatrix);

            Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.FOV),
                                                                           (float)window.Width / (float)window.Height, 0.1f, 1000.0f);

            shader.LoadProjectionMatrix(projectionMatrix);



            Matrix4 transformMatrix = UtilsMath.CreateTransformMatrix(entity.Position,
                                                                      entity.RotationX,
                                                                      entity.RotationY,
                                                                      entity.RotationZ,
                                                                      entity.Scaling);
            shader.LoadTransformMatrix(transformMatrix);

            shader.LoadShine(texturedModel.Texture.ShineDamper, texturedModel.Texture.Reflectivity);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturedModel.Texture.ID);
            GL.DrawElements(PrimitiveType.Triangles, rawModel.VertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
            GL.BindVertexArray(0);
        }
    }
}
