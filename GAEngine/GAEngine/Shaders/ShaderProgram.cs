using OpenTK;
using OpenTK.Graphics.ES30;
using System;
using System.IO;
using System.Text;

namespace GAEngine.Shaders
{
    abstract class ShaderProgram
    {
        private int _programID;
        private int _vertexShaderID;
        private int _fragmentShaderID;

        public ShaderProgram(string vertexFile, string fragmentFile)
        {
            _vertexShaderID = LoadShader(vertexFile, ShaderType.VertexShader);
            _fragmentShaderID = LoadShader(fragmentFile, ShaderType.FragmentShader);

            _programID = GL.CreateProgram();
            GL.AttachShader(_programID, _vertexShaderID);
            GL.AttachShader(_programID, _fragmentShaderID);
            BindAttributes();
            GL.LinkProgram(_programID);
            GL.ValidateProgram(_programID);
            GetAllUniformLocations();
        }

        public void Start()
        {
            GL.UseProgram(_programID);
        }

        public void Stop()
        {
            GL.UseProgram(0);
        }

        public void CleanUp()
        {
            Stop();
            GL.DetachShader(_programID, _vertexShaderID);
            GL.DetachShader(_programID, _fragmentShaderID);
            GL.DeleteShader(_vertexShaderID);
            GL.DeleteShader(_fragmentShaderID);
            GL.DeleteProgram(_programID);
        }

        protected abstract void BindAttributes();

        protected void BindAttribute(int attribute, string variableName)
        {
            GL.BindAttribLocation(_programID, attribute, variableName);
        }

        protected void LoadFloat(int location, float value)
        {
            GL.Uniform1(location, value);
        }

        protected void LoadVector(int location, OpenTK.Vector3 value)
        {
            GL.Uniform3(location, value);
        }

        protected void LoadBoolean(int location, bool value)
        {   
            GL.Uniform1(location, value ? 1 : 0);
        }

        protected void LoadMatrix(int location, Matrix4 matrix)
        {
            GL.UniformMatrix4(location, false, ref matrix);
        }

        protected abstract void GetAllUniformLocations();

        protected int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(_programID, uniformName);
        }

        private static int LoadShader(string filePath, ShaderType type)
        {
            StringBuilder shaderSource = new StringBuilder();

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                    shaderSource.Append(line).Append("\n");
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message);
            }

            int shaderID = GL.CreateShader(type);
            GL.ShaderSource(shaderID, shaderSource.ToString());
            GL.CompileShader(shaderID);

            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out int status);

            if (status != 1)
                throw new ApplicationException();

            return shaderID;
        }
    }
}
