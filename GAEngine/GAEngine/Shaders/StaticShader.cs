using OpenTK;

namespace GAEngine.Shaders
{
    class StaticShader : ShaderProgram
    {
        private const string VERTEX_FILE = "Shaders/VertexShader.txt";
        private const string FRAGMENT_FILE = "Shaders/FragmentShader.txt";

        private int _locationViewMatrix;
        private int _locationTransformMatrix;
        private int _locationProjectionMatrix;

        public StaticShader() : base(VERTEX_FILE, FRAGMENT_FILE) { }

        protected override void BindAttributes()
        {
            base.BindAttribute(0, "position");
            base.BindAttribute(1, "texCoords");
        }

        protected override void GetAllUniformLocations()
        {
            _locationTransformMatrix = base.GetUniformLocation("transformMatrix");
            _locationProjectionMatrix = base.GetUniformLocation("projectionMatrix");
            _locationViewMatrix = base.GetUniformLocation("viewMatrix");
        }

        public void LoadTransformMatrix(Matrix4 matrix)
        {
            base.LoadMatrix(_locationTransformMatrix, matrix);
        }

        public void LoadProjectionMatrix(Matrix4 matrix)
        {
            base.LoadMatrix(_locationProjectionMatrix, matrix);
        }

        public void LoadViewMatrix(Matrix4 matrix)
        {
            base.LoadMatrix(_locationViewMatrix, matrix);
        }
    }
}
