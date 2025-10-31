using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SpacePodGame.Graphics
{
    /// <summary>
    /// First-person camera with WASD movement and mouse look
    /// </summary>
    public class Camera
    {
        // Camera position and orientation
        public Vector3 Position { get; set; }
        public Vector3 Front { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Right { get; private set; }
        public Vector3 WorldUp { get; private set; }

        // Euler angles for rotation
        public float Yaw { get; set; } = -90.0f; // Looking towards -Z initially
        public float Pitch { get; set; } = 0.0f;

        // Camera settings
        public float MovementSpeed { get; set; } = 5.0f;
        public float MouseSensitivity { get; set; } = 0.1f;
        public float Zoom { get; set; } = 45.0f;

        // Constructor
        public Camera(Vector3 position, Vector3 up)
        {
            Position = position;
            WorldUp = up;
            UpdateCameraVectors();
        }

        /// <summary>
        /// Get the view matrix for rendering
        /// </summary>
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        /// <summary>
        /// Get the projection matrix
        /// </summary>
        public Matrix4 GetProjectionMatrix(float aspectRatio)
        {
            return Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(Zoom), 
                aspectRatio, 
                0.1f, 
                100.0f
            );
        }

        /// <summary>
        /// Process keyboard input for movement (WASD)
        /// </summary>
        public void ProcessKeyboard(KeyboardState keyboard, float deltaTime)
        {
            float velocity = MovementSpeed * deltaTime;

            if (keyboard.IsKeyDown(Keys.W))
                Position += Front * velocity;
            if (keyboard.IsKeyDown(Keys.S))
                Position -= Front * velocity;
            if (keyboard.IsKeyDown(Keys.A))
                Position -= Right * velocity;
            if (keyboard.IsKeyDown(Keys.D))
                Position += Right * velocity;
            if (keyboard.IsKeyDown(Keys.Space))
                Position += Up * velocity;
            if (keyboard.IsKeyDown(Keys.LeftShift))
                Position -= Up * velocity;
        }

        /// <summary>
        /// Process mouse movement for looking around
        /// </summary>
        public void ProcessMouseMovement(float xOffset, float yOffset, bool constrainPitch = true)
        {
            xOffset *= MouseSensitivity;
            yOffset *= MouseSensitivity;

            Yaw += xOffset;
            Pitch += yOffset;

            // Constrain pitch to prevent screen flip
            if (constrainPitch)
            {
                if (Pitch > 89.0f)
                    Pitch = 89.0f;
                if (Pitch < -89.0f)
                    Pitch = -89.0f;
            }

            UpdateCameraVectors();
        }

        /// <summary>
        /// Process mouse scroll for zoom
        /// </summary>
        public void ProcessMouseScroll(float yOffset)
        {
            Zoom -= yOffset;
            if (Zoom < 1.0f)
                Zoom = 1.0f;
            if (Zoom > 45.0f)
                Zoom = 45.0f;
        }

        /// <summary>
        /// Update camera vectors based on yaw and pitch
        /// </summary>
        private void UpdateCameraVectors()
        {
            // Calculate new Front vector
            Vector3 front;
            front.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
            front.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            Front = Vector3.Normalize(front);

            // Recalculate Right and Up vectors
            Right = Vector3.Normalize(Vector3.Cross(Front, WorldUp));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }
    }
}
