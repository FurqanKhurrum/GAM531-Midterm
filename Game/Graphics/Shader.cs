using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SpacePodGame.Graphics
{
    /// <summary>
    /// Handles loading, compiling, and using GLSL shaders
    /// </summary>
    public class Shader : IDisposable
    {
        public int Handle { get; private set; }
        private bool _disposed = false;

        public Shader(string vertexPath, string fragmentPath)
        {
            // Load shader source code
            string vertexShaderSource = File.ReadAllText(vertexPath);
            string fragmentShaderSource = File.ReadAllText(fragmentPath);

            // Compile vertex shader
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);
            CheckCompileErrors(vertexShader, "VERTEX");

            // Compile fragment shader
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);
            CheckCompileErrors(fragmentShader, "FRAGMENT");

            // Link shaders into a program
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);
            CheckCompileErrors(Handle, "PROGRAM");

            // Clean up shaders (they're linked into the program now)
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        /// <summary>
        /// Activate this shader program
        /// </summary>
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        /// <summary>
        /// Set a uniform mat4 in the shader
        /// </summary>
        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(location, false, ref matrix);
        }

        /// <summary>
        /// Set a uniform vec3 in the shader
        /// </summary>
        public void SetVector3(string name, Vector3 vector)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform3(location, vector);
        }

        /// <summary>
        /// Set a uniform int in the shader
        /// </summary>
        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(location, value);
        }

        /// <summary>
        /// Set a uniform bool in the shader
        /// </summary>
        public void SetBool(string name, bool value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(location, value ? 1 : 0);
        }

        /// <summary>
        /// Check for shader compilation/linking errors
        /// </summary>
        private void CheckCompileErrors(int shader, string type)
        {
            if (type != "PROGRAM")
            {
                GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
                if (success == 0)
                {
                    string infoLog = GL.GetShaderInfoLog(shader);
                    Console.WriteLine($"ERROR::SHADER_COMPILATION_ERROR of type: {type}\n{infoLog}");
                }
            }
            else
            {
                GL.GetProgram(shader, GetProgramParameterName.LinkStatus, out int success);
                if (success == 0)
                {
                    string infoLog = GL.GetProgramInfoLog(shader);
                    Console.WriteLine($"ERROR::PROGRAM_LINKING_ERROR\n{infoLog}");
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                GL.DeleteProgram(Handle);
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
