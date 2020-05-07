using Assimp;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace GAEngine.Utils
{
    public static class UtilsMath
    {
        public static Matrix4 CreateTransformMatrix(Vector3 translation, Vector3 rotation, Vector3 scale)
        {
            Matrix4 matrix = new Matrix4();
            matrix = Matrix4.Identity;
            matrix = Matrix4.CreateScale(scale) *
                     Matrix4.CreateRotationX(rotation.X) *
                     Matrix4.CreateRotationY(rotation.Y) *
                     Matrix4.CreateRotationZ(rotation.Z) *
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

        public static Vector3 FromVector(this Vector3D vec)
        {
            Vector3 v;
            v.X = vec.X;
            v.Y = vec.Y;
            v.Z = vec.Z;
            return v;
        }

        public static Color4 FromColor(this Color4D color)
        {
            Color4 c;
            c.R = color.R;
            c.G = color.G;
            c.B = color.B;
            c.A = color.A;
            return c;
        }

        public static Matrix4 FromMatrix(this Matrix4x4 mat)
        {
            Matrix4 m = new Matrix4();
            m.M11 = mat.A1;
            m.M12 = mat.A2;
            m.M13 = mat.A3;
            m.M14 = mat.A4;
            m.M21 = mat.B1;
            m.M22 = mat.B2;
            m.M23 = mat.B3;
            m.M24 = mat.B4;
            m.M31 = mat.C1;
            m.M32 = mat.C2;
            m.M33 = mat.C3;
            m.M34 = mat.C4;
            m.M41 = mat.D1;
            m.M42 = mat.D2;
            m.M43 = mat.D3;
            m.M44 = mat.D4;
            return m;
        }
    }
}
