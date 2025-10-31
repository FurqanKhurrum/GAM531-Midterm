using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SpacePodGame.Graphics;

namespace SpacePodGame
{
    /// <summary>
    /// Main game class - Space Observation Pod
    /// Player sits in a space pod and can look around at planets and stars
    /// Press L to toggle the pod's interior light
    /// </summary>
    public class Game : GameWindow
    {
        // Rendering components
        private Shader? _shader;
        private Camera? _camera;

        // Meshes (3+ different objects as required)
        private Mesh? _planetMesh;
        private Mesh? _moonMesh;
        private Mesh? _podInteriorMesh;
        private Mesh? _starFieldMesh;
        private Mesh? _telescopeMesh;

        // Textures
        private Texture? _planetTexture;
        private Texture? _moonTexture;
        private Texture? _metalTexture;
        private Texture? _starsTexture;

        // Game state
        private bool _lightEnabled = true;
        private float _planetRotation = 0.0f;
        private float _moonOrbitAngle = 0.0f;
        private bool _telescopeActive = false;
        private float _currentZoom = 45.0f;
        private float _normalZoom = 45.0f;
        private float _telescopeZoom = 20.0f;

        // Mouse control
        private bool _firstMove = true;
        private Vector2 _lastMousePos;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        /// <summary>
        /// Called when the window is created - initialize OpenGL resources
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();

            // Set clear color to black (space!)
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Enable depth testing for 3D rendering
            GL.Enable(EnableCap.DepthTest);

            // Capture mouse cursor for FPS-style controls
            CursorState = CursorState.Grabbed;

            // Load shader program
            _shader = new Shader("Shaders/vertex.glsl", "Shaders/fragment.glsl");

            // Create camera (positioned inside the space pod)
            _camera = new Camera(new Vector3(0.0f, 0.0f, 3.0f), Vector3.UnitY);

            // Create meshes for different objects
            _planetMesh = Mesh.CreateSphere(1.5f, 64, 32); // Large detailed planet
            _moonMesh = Mesh.CreateSphere(0.3f, 32, 16);   // Smaller moon
            _podInteriorMesh = Mesh.CreateCube();           // Pod interior walls
            _starFieldMesh = Mesh.CreateSphere(50.0f, 32, 16); // Distant star field
            _telescopeMesh = Mesh.CreateCube();             // Telescope body

            // Load textures
            _planetTexture = new Texture("Assets/planet_texture.png");
            _moonTexture = new Texture("Assets/moon_texture.png");
            _metalTexture = new Texture("Assets/metal_texture.png");
            _starsTexture = new Texture("Assets/stars_texture.png");

            Console.WriteLine("=== Space Observation Pod ===");
            Console.WriteLine("Controls:");
            Console.WriteLine("  WASD - Move camera");
            Console.WriteLine("  Mouse - Look around");
            Console.WriteLine("  L - Toggle interior light");
            Console.WriteLine("  E - Use telescope (zoom view)");
            Console.WriteLine("  ESC - Exit");
            Console.WriteLine("============================");
        }

        /// <summary>
        /// Called when the window is resized
        /// </summary>
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        /// <summary>
        /// Update game logic
        /// </summary>
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (_camera == null) return;

            float deltaTime = (float)args.Time;

            // Handle keyboard input
            var keyboard = KeyboardState;

            // ESC to exit
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            // L to toggle light (interaction requirement)
            if (keyboard.IsKeyPressed(Keys.L))
            {
                _lightEnabled = !_lightEnabled;
                Console.WriteLine($"Interior light: {(_lightEnabled ? "ON" : "OFF")}");
            }

            // E to toggle telescope (second interaction)
            if (keyboard.IsKeyPressed(Keys.E))
            {
                _telescopeActive = !_telescopeActive;
                Console.WriteLine($"Telescope: {(_telescopeActive ? "ACTIVE - Zoomed View" : "INACTIVE - Normal View")}");
            }

            // Smoothly transition zoom level
            float targetZoom = _telescopeActive ? _telescopeZoom : _normalZoom;
            _currentZoom = MathHelper.Lerp(_currentZoom, targetZoom, 5.0f * deltaTime);
            if (_camera != null)
            {
                _camera.Zoom = _currentZoom;
            }

            // WASD movement
            _camera.ProcessKeyboard(keyboard, deltaTime);

            // Mouse look
            if (_firstMove)
            {
                _lastMousePos = new Vector2(MouseState.X, MouseState.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = MouseState.X - _lastMousePos.X;
                var deltaY = MouseState.Y - _lastMousePos.Y;
                _lastMousePos = new Vector2(MouseState.X, MouseState.Y);

                _camera.ProcessMouseMovement(deltaX, -deltaY);
            }

            // Animate planet rotation
            _planetRotation += 10.0f * deltaTime;

            // Animate moon orbit
            _moonOrbitAngle += 30.0f * deltaTime;
        }

        /// <summary>
        /// Render the scene
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // Clear color and depth buffers
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_shader == null || _camera == null) return;

            // Activate shader
            _shader.Use();

            // Set up view and projection matrices
            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 projection = _camera.GetProjectionMatrix((float)Size.X / Size.Y);

            _shader.SetMatrix4("view", view);
            _shader.SetMatrix4("projection", projection);

            // Set camera position for specular lighting
            _shader.SetVector3("viewPos", _camera.Position);

            // Set light properties (point light at the "sun" position)
            Vector3 lightPos = new Vector3(-8.0f, 5.0f, -5.0f);
            _shader.SetVector3("lightPos", lightPos);
            _shader.SetVector3("lightColor", new Vector3(1.0f, 0.95f, 0.8f)); // Warm sunlight
            _shader.SetBool("lightEnabled", _lightEnabled);

            // Render the star field (background sphere)
            RenderStarField();

            // Render the main planet
            RenderPlanet();

            // Render the orbiting moon
            RenderMoon();

            // Render pod interior elements
            RenderPodInterior();

            // Render the telescope
            RenderTelescope();

            // Swap buffers
            SwapBuffers();
        }

        /// <summary>
        /// Render the distant star field
        /// </summary>
        private void RenderStarField()
        {
            if (_shader == null || _starFieldMesh == null || _starsTexture == null) return;

            Matrix4 model = Matrix4.Identity;
            _shader.SetMatrix4("model", model);
            _shader.SetVector3("objectColor", Vector3.One);
            _shader.SetBool("useTexture", true);

            _starsTexture.Use();
            _starFieldMesh.Draw();
        }

        /// <summary>
        /// Render the main planet with rotation
        /// </summary>
        private void RenderPlanet()
        {
            if (_shader == null || _planetMesh == null || _planetTexture == null) return;

            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_planetRotation))
                          * Matrix4.CreateTranslation(-5.0f, 0.0f, -8.0f);

            _shader.SetMatrix4("model", model);
            _shader.SetVector3("objectColor", new Vector3(0.3f, 0.6f, 1.0f));
            _shader.SetBool("useTexture", true);

            _planetTexture.Use();
            _planetMesh.Draw();
        }

        /// <summary>
        /// Render the moon orbiting the planet
        /// </summary>
        private void RenderMoon()
        {
            if (_shader == null || _moonMesh == null || _moonTexture == null) return;

            // Moon orbits around the planet
            float orbitRadius = 3.0f;
            float moonX = -5.0f + orbitRadius * MathF.Cos(MathHelper.DegreesToRadians(_moonOrbitAngle));
            float moonZ = -8.0f + orbitRadius * MathF.Sin(MathHelper.DegreesToRadians(_moonOrbitAngle));

            Matrix4 model = Matrix4.CreateTranslation(moonX, 0.5f, moonZ);

            _shader.SetMatrix4("model", model);
            _shader.SetVector3("objectColor", new Vector3(1.0f, 1.0f, 1.0f)); // White to show texture properly
            _shader.SetBool("useTexture", true); // Now using texture!

            _moonTexture.Use();
            _moonMesh.Draw();
        }

        /// <summary>
        /// Render pod interior elements (walls/panels)
        /// </summary>
        private void RenderPodInterior()
        {
            if (_shader == null || _podInteriorMesh == null || _metalTexture == null) return;

            _shader.SetBool("useTexture", true);
            _metalTexture.Use();

            // Left panel
            Matrix4 modelLeft = Matrix4.CreateScale(0.1f, 2.0f, 3.0f)
                              * Matrix4.CreateTranslation(-2.0f, 0.0f, 1.0f);
            _shader.SetMatrix4("model", modelLeft);
            _shader.SetVector3("objectColor", new Vector3(0.6f, 0.6f, 0.65f));
            _podInteriorMesh.Draw();

            // Right panel
            Matrix4 modelRight = Matrix4.CreateScale(0.1f, 2.0f, 3.0f)
                               * Matrix4.CreateTranslation(2.0f, 0.0f, 1.0f);
            _shader.SetMatrix4("model", modelRight);
            _shader.SetVector3("objectColor", new Vector3(0.6f, 0.6f, 0.65f));
            _podInteriorMesh.Draw();

            // Bottom panel (floor)
            Matrix4 modelBottom = Matrix4.CreateScale(4.0f, 0.1f, 3.0f)
                                * Matrix4.CreateTranslation(0.0f, -2.0f, 1.0f);
            _shader.SetMatrix4("model", modelBottom);
            _shader.SetVector3("objectColor", new Vector3(0.5f, 0.5f, 0.55f));
            _podInteriorMesh.Draw();
        }

        /// <summary>
        /// Render the telescope (interactable object)
        /// </summary>
        private void RenderTelescope()
        {
            if (_shader == null || _telescopeMesh == null || _metalTexture == null) return;

            _shader.SetBool("useTexture", true);
            _metalTexture.Use();

            // Telescope body - positioned in front of player at an angle
            Matrix4 telescopeModel = Matrix4.CreateScale(0.15f, 0.15f, 0.5f) // Long and thin
                                   * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-20.0f)) // Angled upward
                                   * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-30.0f)) // Angled to side
                                   * Matrix4.CreateTranslation(1.2f, -0.3f, 1.5f); // Right side, slightly down

            // Change color based on active state (glows when active)
            Vector3 telescopeColor = _telescopeActive
                ? new Vector3(0.3f, 0.8f, 1.0f)  // Bright cyan when active
                : new Vector3(0.4f, 0.4f, 0.45f); // Gray when inactive

            _shader.SetMatrix4("model", telescopeModel);
            _shader.SetVector3("objectColor", telescopeColor);
            _telescopeMesh.Draw();

            // Telescope lens/eyepiece - small sphere at the end
            Matrix4 lensModel = Matrix4.CreateScale(0.1f, 0.1f, 0.1f)
                              * Matrix4.CreateTranslation(1.1f, -0.15f, 1.3f);

            Vector3 lensColor = _telescopeActive
                ? new Vector3(0.5f, 1.0f, 1.0f)  // Bright glowing cyan
                : new Vector3(0.3f, 0.3f, 0.35f); // Dark when inactive

            _shader.SetMatrix4("model", lensModel);
            _shader.SetVector3("objectColor", lensColor);
            _shader.SetBool("useTexture", false); // Use solid color for lens glow

            // Create a sphere mesh for the lens if we don't have one, or reuse moon mesh
            if (_moonMesh != null)
            {
                _moonMesh.Draw();
            }
        }

        /// <summary>
        /// Clean up resources when window closes
        /// </summary>
        protected override void OnUnload()
        {
            base.OnUnload();

            // Dispose of all resources
            _shader?.Dispose();
            _planetMesh?.Dispose();
            _moonMesh?.Dispose();
            _podInteriorMesh?.Dispose();
            _starFieldMesh?.Dispose();
            _telescopeMesh?.Dispose();
            _planetTexture?.Dispose();
            _moonTexture?.Dispose();
            _metalTexture?.Dispose();
            _starsTexture?.Dispose();
        }
    }
}