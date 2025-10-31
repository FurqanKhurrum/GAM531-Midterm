using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SpacePodGame.Graphics
{
    /// <summary>
    /// Represents a 3D mesh with vertices, normals, and texture coordinates
    /// Uses VAO/VBO/EBO for efficient rendering
    /// </summary>
    public class Mesh : IDisposable
    {
        private int _vao; // Vertex Array Object
        private int _vbo; // Vertex Buffer Object
        private int _ebo; // Element Buffer Object
        private int _indexCount;
        private bool _disposed = false;

        public Mesh(float[] vertices, uint[] indices)
        {
            _indexCount = indices.Length;

            // Generate VAO, VBO, EBO
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            // Bind VAO first
            GL.BindVertexArray(_vao);

            // Upload vertex data to VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Upload index data to EBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // Set vertex attribute pointers
            // Position attribute (location = 0)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Normal attribute (location = 1)
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Texture coordinate attribute (location = 2)
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            // Unbind
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Render this mesh
        /// </summary>
        public void Draw()
        {
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, _indexCount, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Create a cube mesh
        /// </summary>
        public static Mesh CreateCube()
        {
            float[] vertices = {
                // Positions          // Normals           // Texture Coords
                // Front face
                -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
                -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f,
                // Back face
                -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
                 0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
                 0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
                -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
                // Left face
                -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
                -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
                -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
                -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
                // Right face
                 0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
                 0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
                 0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
                // Top face
                -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
                 0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
                 0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
                -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
                // Bottom face
                -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
                 0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
            };

            uint[] indices = {
                0, 1, 2, 2, 3, 0,       // Front
                4, 5, 6, 6, 7, 4,       // Back
                8, 9, 10, 10, 11, 8,    // Left
                12, 13, 14, 14, 15, 12, // Right
                16, 17, 18, 18, 19, 16, // Top
                20, 21, 22, 22, 23, 20  // Bottom
            };

            return new Mesh(vertices, indices);
        }

        /// <summary>
        /// Create a sphere mesh (approximated with subdivisions)
        /// </summary>
        public static Mesh CreateSphere(float radius = 0.5f, int segments = 32, int rings = 16)
        {
            List<float> vertices = new List<float>();
            List<uint> indices = new List<uint>();

            for (int ring = 0; ring <= rings; ring++)
            {
                float phi = MathF.PI * ring / rings;
                for (int seg = 0; seg <= segments; seg++)
                {
                    float theta = 2.0f * MathF.PI * seg / segments;

                    float x = radius * MathF.Sin(phi) * MathF.Cos(theta);
                    float y = radius * MathF.Cos(phi);
                    float z = radius * MathF.Sin(phi) * MathF.Sin(theta);

                    // Position
                    vertices.Add(x);
                    vertices.Add(y);
                    vertices.Add(z);

                    // Normal (normalized position for a sphere)
                    Vector3 normal = Vector3.Normalize(new Vector3(x, y, z));
                    vertices.Add(normal.X);
                    vertices.Add(normal.Y);
                    vertices.Add(normal.Z);

                    // Texture coordinates
                    vertices.Add((float)seg / segments);
                    vertices.Add((float)ring / rings);
                }
            }

            // Generate indices
            for (int ring = 0; ring < rings; ring++)
            {
                for (int seg = 0; seg < segments; seg++)
                {
                    uint current = (uint)(ring * (segments + 1) + seg);
                    uint next = current + (uint)(segments + 1);

                    indices.Add(current);
                    indices.Add(next);
                    indices.Add(current + 1);

                    indices.Add(current + 1);
                    indices.Add(next);
                    indices.Add(next + 1);
                }
            }

            return new Mesh(vertices.ToArray(), indices.ToArray());
        }

        /// <summary>
        /// Create a plane mesh (for floor/ground)
        /// </summary>
        public static Mesh CreatePlane(float size = 10.0f)
        {
            float halfSize = size / 2.0f;
            float[] vertices = {
                // Positions              // Normals          // Texture Coords
                -halfSize, 0.0f, -halfSize, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
                 halfSize, 0.0f, -halfSize, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
                 halfSize, 0.0f,  halfSize, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
                -halfSize, 0.0f,  halfSize, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            };

            uint[] indices = {
                0, 1, 2,
                2, 3, 0
            };

            return new Mesh(vertices, indices);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                GL.DeleteVertexArray(_vao);
                GL.DeleteBuffer(_vbo);
                GL.DeleteBuffer(_ebo);
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
