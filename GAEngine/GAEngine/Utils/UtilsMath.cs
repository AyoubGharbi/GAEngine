using OpenTK;
using OpenTK.Graphics.ES10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Utils
{
    class UtilsMath
    {
        public static Matrix4 CreateTransformMatrix(Vector3 translation, float rx, float ry, float rz, float scale)
        {
            Matrix4 matrix = new Matrix4();
            GL.LoadIdentity();
            matrix = Matrix4.CreateTranslation(translation);
            matrix = Matrix4.CreateRotationX(rx);
            matrix = Matrix4.CreateRotationX(ry);
            matrix = Matrix4.CreateRotationX(rz);
            matrix = Matrix4.CreateScale(scale);
            return matrix;
        }
    }
}
