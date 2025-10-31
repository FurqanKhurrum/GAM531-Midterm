using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace SpacePodGame
{
    /// <summary>
    /// Entry point for the Space Observation Pod game
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Configure game window settings for 60 FPS
            var gameWindowSettings = new GameWindowSettings()
            {
                UpdateFrequency = 60.0
            };

            // Configure native window settings
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(1280, 720),
                Title = "Space Observation Pod - Midterm Project",
                Flags = ContextFlags.ForwardCompatible,
                APIVersion = new Version(3, 3),
                Profile = ContextProfile.Core
            };

            // Create and run the game
            using (var game = new Game(gameWindowSettings, nativeWindowSettings))
            {
                game.Run();
            }
        }
    }
}
