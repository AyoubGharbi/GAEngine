using OpenTK;
using OpenTK.Graphics.ES30;
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
            matrix = Matrix4.Identity;
            matrix = Matrix4.CreateScale(scale) *
                     Matrix4.CreateRotationX(rx) *
                     Matrix4.CreateRotationY(ry) *
                     Matrix4.CreateRotationZ(rz) *
                     Matrix4.CreateTranslation(translation);
            return matrix;
        }
    }
}
