﻿namespace GAEngine.RenderEngine
{
    class RawModel
    {
        private int _vaoID;
        private int _vertexCount;

        public int VAOID => _vaoID;
        public int VertexCount => _vertexCount;

        public RawModel(int vaoID, int vertexCount)
        {
            _vaoID = vaoID;
            _vertexCount = vertexCount;
        }
    }
}
