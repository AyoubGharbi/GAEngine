using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.RenderEngine
{
    class Loader
    {
        private readonly List<int> _vaos = new List<int>();
        private readonly List<int> _vbos = new List<int>();

        public RawModel LoadToVAO(float[] positions)
        {
            int vaoID = CreateVAO();
            StoreDataToAttributeList(0, positions);
            UnbindVAO();

            return new RawModel(vaoID, positions.Length / 3);
        }

        private int CreateVAO()
        {
            GL.GenVertexArrays(1, out int vaoID);
            _vaos.Add(vaoID);
            GL.BindVertexArray(vaoID);
            return vaoID;
        }

        private void StoreDataToAttributeList(int attributeNumber, float[] data)
        {
            GL.GenBuffers(1, out int vboID);
            _vbos.Add(vboID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * sizeof(float)), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeNumber, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void UnbindVAO()
        {
            GL.BindVertexArray(0);
        }

        public void CleanUp()
        {
            foreach (var vao in _vaos)
                GL.DeleteVertexArray(vao);

            foreach (var vbo in _vbos)
                GL.DeleteBuffer(vbo);
        }

    }
}
