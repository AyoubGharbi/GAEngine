using GAEngine.Entities;
using OpenTK;


namespace GAEngine.Utils
{
    class UtilsMath
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

        public static Matrix4 CreateViewMatrix(Camera camera)
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
    }
}
