using Assimp;
using GAEngine.Entities;
using OpenTK;
using System.Collections.Generic;

namespace GAEngine.Utils
{
    public static class UtilsMath
    {
        public static Matrix4 CreateTransformMatrix(Vector3 translation, float rx, float ry, float rz, float scale)
        {
            Matrix4 matrix = new Matrix4();
            matrix = Matrix4.Identity;
            matrix = Matrix4.CreateScale(scale) *
                     Matrix4.CreateRotationX(rx) *
                     Matrix4.CreateRotationY(ry) *
                     Matrix4.CreateRotationZ(rz) *
                     Matrix4.CreateTranslation(translation);
            return matrix;
        }

        public static Matrix4 CreateViewMatrix(Entities.Camera camera)
        {
            Matrix4 viewMatrix = new Matrix4();

            Vector3 cameraPos = camera.Position;
            Vector3 negativeCameraPos = new Vector3(-cameraPos.X, -cameraPos.Y, -cameraPos.Z);

            viewMatrix = Matrix4.Identity;
            viewMatrix = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(camera.Pitch)) *
                         Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(camera.Yaw)) *
                         Matrix4.CreateTranslation(negativeCameraPos);

            return viewMatrix;
        }

        public static float[] Vector3ToFloat(this List<Vector3D> vertices)
        {
            float[] floatVertices = new float[vertices.Count * 3];

            var floatIndex = 0;

            for (int i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                floatVertices[floatIndex] = vertex.X;
                floatVertices[floatIndex + 1] = vertex.Y;
                floatVertices[floatIndex + 2] = vertex.Z;
                floatIndex += 3;
            }

            return floatVertices;
        }

        public static float[] Vector2ToFloat(this List<Vector3D> coords)
        {
            float[] floatVertices = new float[coords.Count * 2];

            var floatIndex = 0;

            for (int i = 0; i < coords.Count; i++)
            {
                var vertex = coords[i];
                floatVertices[floatIndex] = vertex.X;
                floatVertices[floatIndex + 1] = 1 - vertex.Y;
                floatIndex += 2;
            }

            return floatVertices;
        }


        public static System.Numerics.Vector3 Vector3ToV3(this Vector3 vec)
        {
            return new System.Numerics.Vector3(vec.X, vec.Y, vec.Z);
        }

        public static Vector3 Vector3ToV3(this System.Numerics.Vector3 vec)
        {
            return new Vector3(vec.X, vec.Y, vec.Z);
        }
    }
}
