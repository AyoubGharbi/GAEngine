using GAEngine.Components;
using GAEngine.ECS;
using GAEngine.Entities;
using GAEngine.Lights;
using GAEngine.Models;
using GAEngine.Shaders;
using GAEngine.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using static GAEngine.Shaders.StaticShader;

namespace GAEngine.RenderEngine
{
    class Renderer
    {
        public void Prepare()
        {
            // color buffer and depth buffer
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
        }

        public void Render(GAWindow window, MeshComponent mesh,
                           TransformComponent transform, StaticShader shader,
                           Camera camera, Light light)
        {
            // back-facing polygons can be culled.
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            TexturedModel texturedModel = mesh.TexturedModel;
            RawModel rawModel = texturedModel.Raw;

            // bind
            GL.BindVertexArray(rawModel.VAOID);
            // enable vertex attributes
            GL.EnableVertexAttribArray(GetAttributeTypeID(AttributeTypes.position));
            GL.EnableVertexAttribArray(GetAttributeTypeID(AttributeTypes.texCoords));
            GL.EnableVertexAttribArray(GetAttributeTypeID(AttributeTypes.normal));

            // lights
            shader.LoadLight(light);

            // camera (view matrix)
            Matrix4 viewMatrix = UtilsMath.CreateViewMatrix(camera);
            shader.LoadViewMatrix(viewMatrix);

            Matrix4 projectionMatrix =
                Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.FOV),
                                                                           (float)window.Width / (float)window.Height,
                                                                           0.1f, 10000.0f);

            shader.LoadProjectionMatrix(projectionMatrix);

            var scale = transform.Scale;
            var position = transform.Position;
            var rotation = transform.Rotation;

            Matrix4 transformMatrix = UtilsMath.CreateTransformMatrix(position, rotation, scale);

            shader.LoadTransformMatrix(transformMatrix);

            shader.LoadShine(texturedModel.Texture.ShineDamper, texturedModel.Texture.Reflectivity);

            // draw model
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturedModel.Texture.ID);
            GL.DrawElements(PrimitiveType.Triangles, rawModel.VertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);

            // disable vertex attributes
            GL.DisableVertexAttribArray(GetAttributeTypeID(AttributeTypes.position));
            GL.DisableVertexAttribArray(GetAttributeTypeID(AttributeTypes.texCoords));
            GL.DisableVertexAttribArray(GetAttributeTypeID(AttributeTypes.normal));
            // unbind
            GL.BindVertexArray(0);
        }
    }
}
