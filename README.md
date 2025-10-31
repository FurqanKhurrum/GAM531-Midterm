# Space Observation Pod - 3D Game Midterm Project

A fully interactive 3D space observation experience built with C# and OpenTK, where you sit inside a futuristic space pod and observe celestial bodies with dynamic lighting.

![Space Observation Pod](https://img.shields.io/badge/OpenTK-4.8.2-blue) ![.NET](https://img.shields.io/badge/.NET-8.0-purple) ![Status](https://img.shields.io/badge/Status-Complete-success)

## 📝 Description

You are positioned inside a space observation pod floating in deep space. Through the pod's viewport, you can observe a beautiful rotating planet with its orbiting moon against a stunning star field backdrop. The pod features an interior lighting system that you can toggle on and off, dramatically changing the atmosphere of your observation experience.

## 🎮 Gameplay Instructions

### Controls

- **W/A/S/D** - Move the camera around inside the pod
- **Mouse Movement** - Look around (360-degree view)
- **Space** - Move camera up
- **Left Shift** - Move camera down
- **L** - Toggle interior pod lighting (main interaction feature)
- **ESC** - Exit the game

### Objectives

- Explore the space pod interior
- Observe the rotating planet and orbiting moon
- Experiment with the lighting system to see how it affects the scene
- Practice your 3D navigation skills

## ✨ Features Implemented

### Required Features (15 pts)

✅ **Window Setup & Rendering Loop** (1 pt)
- 60 FPS rendering loop
- 1280x720 resolution window
- Proper OpenGL context initialization

✅ **Geometry - 3+ Objects with VAO/VBO/EBO** (2 pts)
- **Planet**: High-detail sphere (64 segments, 32 rings)
- **Moon**: Medium-detail sphere (32 segments, 16 rings)
- **Pod Interior**: Multiple cube-based panels (left wall, right wall, floor)
- **Star Field**: Large background sphere

✅ **Texturing** (2 pts)
- Planet texture (procedurally generated earth-like appearance)
- Metal panel texture for pod interior
- Star field texture for space background
- Proper UV mapping on all textured objects

✅ **Lighting - Phong Model** (3 pts)
- **Ambient lighting**: Base illumination for all objects
- **Diffuse lighting**: Direction-dependent surface lighting
- **Specular lighting**: Shiny highlights based on view angle
- Point light source positioned as a "sun"
- Toggle-able lighting system (main interaction)

✅ **Camera Control** (3 pts)
- WASD keyboard movement (forward/back/left/right)
- Mouse look with yaw and pitch control
- Space/Shift for vertical movement
- FPS-style camera with smooth controls
- Cursor locked during gameplay

✅ **Interaction - Event Response** (2 pts)
- Press **L** to toggle pod interior lighting
- Visual feedback when light state changes
- Console output confirms interaction

✅ **Code Structure & Documentation** (2 pts)
- Organized into proper classes: `Shader`, `Mesh`, `Texture`, `Camera`, `Game`
- Comprehensive code comments
- Clean, modular architecture
- Proper resource disposal (IDisposable pattern)

### Bonus Features (+3 pts potential)

✅ **Animation**
- Planet rotates continuously
- Moon orbits around the planet
- Dynamic scene with moving elements

## 🛠️ How to Build and Run

### Prerequisites

- **.NET SDK 8.0 or higher** - [Download here](https://dotnet.microsoft.com/download)
- **Visual Studio 2022** (optional but recommended) or any C# IDE
- **Windows/Linux/macOS** (OpenTK is cross-platform)

### Dependencies

The project automatically downloads these NuGet packages:
- `OpenTK 4.8.2` - OpenGL bindings for .NET
- `StbImageSharp 2.27.14` - Image loading library

### Building from Command Line

```bash
# Clone or download the repository
cd SpacePod_Midterm_Game

# Restore NuGet packages
dotnet restore

# Build the project
dotnet build

# Run the game
dotnet run --project Game/Game.csproj
```

### Building with Visual Studio

1. Open `Game.sln` in Visual Studio 2022
2. Right-click the solution → "Restore NuGet Packages"
3. Press **F5** to build and run in Debug mode
4. Or press **Ctrl+F5** to run without debugging

### Building with Visual Studio Code

1. Open the `SpacePod_Midterm_Game` folder in VS Code
2. Install the C# extension if not already installed
3. Open integrated terminal
4. Run: `dotnet run --project Game/Game.csproj`

## 📁 Project Structure

```
SpacePod_Midterm_Game/
│
├── Game.sln                      # Visual Studio solution file
├── .gitignore                    # Git ignore rules
├── README.md                     # This file
│
└── Game/                         # Main project folder
    ├── Game.csproj               # Project file with dependencies
    ├── Program.cs                # Entry point
    ├── Game.cs                   # Main game logic and rendering
    │
    ├── Shaders/                  # GLSL shaders
    │   ├── vertex.glsl           # Vertex shader with transformations
    │   └── fragment.glsl         # Fragment shader with Phong lighting
    │
    ├── Assets/                   # Game assets
    │   ├── planet_texture.png    # Procedurally generated planet texture
    │   ├── metal_texture.png     # Pod interior metal panels
    │   └── stars_texture.png     # Space background
    │
    └── Graphics/                 # OpenGL wrapper classes
        ├── Shader.cs             # Shader compilation and management
        ├── Texture.cs            # Texture loading and binding
        ├── Mesh.cs               # 3D mesh with VAO/VBO/EBO
        └── Camera.cs             # FPS camera with controls
```

## 🎨 Technical Details

### Graphics Pipeline

1. **Vertex Shader** (`vertex.glsl`)
   - Transforms vertices from model space to clip space
   - Calculates normal vectors for lighting
   - Passes texture coordinates to fragment shader

2. **Fragment Shader** (`fragment.glsl`)
   - Implements full Phong lighting model
   - Supports textured and non-textured rendering
   - Handles light enable/disable state

### Math & Transformations

- Model matrices for object positioning and rotation
- View matrix from FPS camera
- Perspective projection matrix (45° FOV)
- Normal matrix calculation for proper lighting

### Rendering Techniques

- Depth testing for correct 3D occlusion
- Texture sampling with mipmapping
- Efficient instanced rendering with reusable meshes
- Procedural sphere and cube generation

## 📸 Screenshots

The game features:
- A large, textured planet rotating in space
- A smaller moon orbiting the planet
- Metal-paneled pod interior walls
- A starfield background sphere
- Dynamic Phong lighting with toggleable interior lights

## 🎓 Learning Outcomes

This project demonstrates:
- 3D mathematics (vectors, matrices, quaternions)
- OpenGL rendering pipeline (shaders, VAO/VBO/EBO)
- Texture mapping and UV coordinates
- Phong lighting model implementation
- Camera systems and input handling
- Resource management and disposal
- Clean code architecture and organization

## 🙏 Credits

### Assets
- **Textures**: All textures are procedurally generated using Python PIL
  - Planet texture: Custom earth-like procedural generation
  - Metal texture: Procedural panel generation with rivets
  - Stars texture: Random star field generation
- **No external assets used** - All content is original

### Libraries
- **OpenTK** - OpenGL bindings (MIT License)
- **StbImageSharp** - Image loading (Public Domain)

### References
- OpenTK Documentation: https://opentk.net/
- LearnOpenGL tutorials: https://learnopengl.com/
- OpenGL specification: https://registry.khronos.org/OpenGraphics/

## 📄 License

This project is created for educational purposes as part of a midterm assignment.

## 👤 Author

Created for the Midterm Game Code Challenge (C# + OpenTK)
Date: October 30, 2025

---

**Note**: This project meets all technical requirements specified in the midterm rubric:
- ✅ C# + .NET 8.0
- ✅ OpenTK 4.x
- ✅ GLSL 330 shaders
- ✅ 3+ different meshes with VAO/VBO/EBO
- ✅ Phong lighting (ambient/diffuse/specular)
- ✅ Textured environment
- ✅ Camera with WASD + mouse control
- ✅ Interactive element (light toggle)
- ✅ Clean, documented code structure
- ✅ 60+ FPS performance
