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


## ✨ Features Implemented

### Required Features (15 pts)

✅ **Window Setup & Rendering Loop** (1 pt)
- 60 FPS rendering loop
- 1280x720 resolution window
- Proper OpenGL context initialization

✅ **Geometry - 3+ Objects with VAO/VBO/EBO** (2 pts)
- **Planet**: High-detail sphere (64 segments, 32 rings)
- **Moon**: Medium-detail sphere (32 segments, 16 rings)
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

### Building with Visual Studio

1. Open `Game.sln` in Visual Studio 2022
2. Right-click the solution → "Restore NuGet Packages"
3. Press **F5** to build and run in Debug mode
4. Or press **Ctrl+F5** to run without debugging

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

## 🎓 Learning Outcomes

This project demonstrates:
- 3D mathematics (vectors, matrices, quaternions)
- OpenGL rendering pipeline (shaders, VAO/VBO/EBO)
- Texture mapping and UV coordinates
- Phong lighting model implementation
- Camera systems and input handling
- Resource management and disposal
- Clean code architecture and organization


### Libraries
- **OpenTK** - OpenGL bindings (MIT License)
- **StbImageSharp** - Image loading (Public Domain)

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
