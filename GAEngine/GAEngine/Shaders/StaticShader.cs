using GAEngine.Lights;
using OpenTK;
using System;

namespace GAEngine.Shaders
{
    class StaticShader : ShaderProgram
    {
        public enum AttributeTypes
        {
            position = 0,
            texCoords = 1,
            normal = 2
        }

        private const string VERTEX_FILE = "shaders/VertexShader.txt";
        private const string FRAGMENT_FILE = "shaders/FragmentShader.txt";

        private int _locationViewMatrix;
        private int _locationTransformMatrix;
        private int _locationProjectionMatrix;
        private int _locationLightColor;
        private int _locationLightPosition;
        private int _locationShineDamper;
        private int _locationReflectivity;

        public StaticShader() : base(VERTEX_FILE, FRAGMENT_FILE)
        {
        }

        protected override void BindAttributes()
        {
            base.BindAttribute(GetAttributeTypeID(AttributeTypes.position), AttributeTypes.position.ToString());
            base.BindAttribute(GetAttributeTypeID(AttributeTypes.texCoords), AttributeTypes.texCoords.ToString());
            base.BindAttribute(GetAttributeTypeID(AttributeTypes.normal), AttributeTypes.normal.ToString());
        }

        protected override void GetAllUniformLocations()
        {
            _locationTransformMatrix = base.GetUniformLocation("transformMatrix");
            _locationProjectionMatrix = base.GetUniformLocation("projectionMatrix");
            _locationViewMatrix = base.GetUniformLocation("viewMatrix");
            _locationLightColor = base.GetUniformLocation("lightColor");
            _locationLightPosition = base.GetUniformLocation("lightPos");
            _locationShineDamper = base.GetUniformLocation("shineDamper");
            _locationReflectivity = base.GetUniformLocation("reflectivity");
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

        public void LoadLight(Light light)
        {
            base.LoadVector(_locationLightColor, light.Color);
            base.LoadVector(_locationLightPosition, light.Position);
        }

        public void LoadShine(float damper, float reflectivity)
        {
            base.LoadFloat(_locationShineDamper, damper);
            base.LoadFloat(_locationReflectivity, reflectivity);
        }

        public static int GetAttributeTypeID(AttributeTypes attType)
        {
            return (int)attType;
        }
    }
}
