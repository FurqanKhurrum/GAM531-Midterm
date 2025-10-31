# 📖 Code Reference Guide

Quick reference for understanding the project structure and key components.

## 🏗️ Architecture Overview

```
┌─────────────────────────────────────────────────┐
│              Program.cs (Entry Point)           │
│  • Creates GameWindow with 60 FPS settings      │
│  • Sets up 1280x720 window                      │
│  • Initializes OpenGL 3.3 Core                  │
└────────────────────┬────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────┐
│              Game.cs (Main Game Loop)           │
│  • OnLoad() - Initialize resources              │
│  • OnUpdateFrame() - Handle input & logic       │
│  • OnRenderFrame() - Draw the scene             │
│  • OnUnload() - Cleanup resources               │
└─────────┬───────────────────────────────┬───────┘
          │                               │
    ┌─────▼─────────┐           ┌────────▼────────┐
    │  GL Classes   │           │  GLSL Shaders   │
    │               │           │                 │
    │ • Shader.cs   │◄──────────┤ • vertex.glsl   │
    │ • Texture.cs  │           │ • fragment.glsl │
    │ • Mesh.cs     │           └─────────────────┘
    │ • Camera.cs   │
    └───────────────┘
```

## 📁 File Responsibilities

### Program.cs (Entry Point)
**Purpose**: Application startup and configuration
```csharp
// Creates the game window with specific settings
var gameWindowSettings = new GameWindowSettings() { UpdateFrequency = 60.0 };
var nativeWindowSettings = new NativeWindowSettings() { 
    ClientSize = new Vector2i(1280, 720),
    Title = "Space Observation Pod"
};
```
**Key Points**:
- Sets 60 FPS update frequency
- Configures window size and title
- Initializes OpenGL 3.3 Core context

---

### Game.cs (Main Game Logic - 270+ lines)
**Purpose**: Core game loop and scene management

#### Key Methods:

**OnLoad()** - Initialization
```csharp
// Called once when window is created
- Set up OpenGL state (depth testing, clear color)
- Load shaders
- Create camera
- Generate meshes (planet, moon, pod, stars)
- Load textures
- Print control instructions
```

**OnUpdateFrame()** - Game Logic
```csharp
// Called 60 times per second
- Handle keyboard input (WASD, ESC, L)
- Process mouse movement for camera
- Update animations (planet rotation, moon orbit)
- Toggle light state on L key press
```

**OnRenderFrame()** - Drawing
```csharp
// Called every frame
- Clear screen and depth buffer
- Set up view/projection matrices
- Set lighting uniforms
- Render star field
- Render planet
- Render moon
- Render pod interior
- Swap buffers
```

**Key Variables**:
```csharp
private Shader _shader;           // Manages GLSL shaders
private Camera _camera;           // FPS camera
private Mesh _planetMesh;         // Planet geometry
private Mesh _moonMesh;           // Moon geometry
private Mesh _podInteriorMesh;    // Pod walls
private Mesh _starFieldMesh;      // Background sphere
private Texture _planetTexture;   // Earth-like texture
private Texture _metalTexture;    // Pod panels
private Texture _starsTexture;    // Space background
private bool _lightEnabled;       // Light toggle state
private float _planetRotation;    // Animation angle
private float _moonOrbitAngle;    // Orbit angle
```

---

### Graphics/Shader.cs (Shader Management - 130+ lines)
**Purpose**: Load, compile, and use GLSL shaders

#### Key Methods:

**Constructor**
```csharp
public Shader(string vertexPath, string fragmentPath)
// 1. Read shader source files
// 2. Compile vertex shader
// 3. Compile fragment shader
// 4. Link into shader program
// 5. Check for errors
```

**Uniform Setters**
```csharp
SetMatrix4(string name, Matrix4 matrix)  // For transformations
SetVector3(string name, Vector3 vector)  // For colors/positions
SetInt(string name, int value)           // For texture units
SetBool(string name, bool value)         // For flags
```

**Usage Pattern**:
```csharp
shader.Use();
shader.SetMatrix4("model", modelMatrix);
shader.SetVector3("lightPos", lightPosition);
shader.SetBool("lightEnabled", true);
```

---

### Graphics/Texture.cs (Texture Loading - 60+ lines)
**Purpose**: Load and bind textures from image files

#### Key Features:
```csharp
public Texture(string path)
// 1. Generate OpenGL texture ID
// 2. Load image using StbImageSharp
// 3. Upload pixel data to GPU
// 4. Set texture parameters (wrapping, filtering)
// 5. Generate mipmaps
```

**Texture Parameters**:
- Min/Mag filtering: Linear (smooth)
- Wrap mode: Repeat
- Mipmaps: Auto-generated for LOD

**Usage**:
```csharp
var texture = new Texture("Assets/planet_texture.png");
texture.Use(TextureUnit.Texture0);
shader.SetInt("texture0", 0);
```

---

### Graphics/Mesh.cs (3D Geometry - 200+ lines)
**Purpose**: Create and render 3D geometry with VAO/VBO/EBO

#### Data Structure:
```csharp
// Each vertex has 8 floats:
// [px, py, pz, nx, ny, nz, u, v]
//  Position   Normal    TexCoord
```

#### Key Methods:

**Constructor**
```csharp
public Mesh(float[] vertices, uint[] indices)
// 1. Generate VAO, VBO, EBO
// 2. Upload vertex data to GPU
// 3. Upload index data to GPU
// 4. Configure vertex attributes:
//    - Location 0: Position (3 floats)
//    - Location 1: Normal (3 floats)
//    - Location 2: TexCoord (2 floats)
```

**Draw Method**
```csharp
public void Draw()
// 1. Bind VAO
// 2. Draw triangles using indices
// 3. Unbind VAO
```

**Static Factory Methods**:
```csharp
CreateCube()   // Box with 24 vertices (6 faces × 4 corners)
CreateSphere() // Sphere with parametric generation
CreatePlane()  // Flat ground plane
```

**Sphere Generation Algorithm**:
```csharp
// Uses spherical coordinates:
// x = radius * sin(phi) * cos(theta)
// y = radius * cos(phi)
// z = radius * sin(phi) * sin(theta)
// 
// Parameters:
// - segments: horizontal divisions
// - rings: vertical divisions
```

---

### Graphics/Camera.cs (FPS Camera - 130+ lines)
**Purpose**: First-person camera with WASD + mouse controls

#### Camera Properties:
```csharp
Vector3 Position  // Camera location
Vector3 Front     // Forward direction
Vector3 Up        // Up direction
Vector3 Right     // Right direction
float Yaw         // Horizontal rotation (degrees)
float Pitch       // Vertical rotation (degrees)
float Zoom        // Field of view
```

#### Key Methods:

**GetViewMatrix()**
```csharp
// Creates view matrix using LookAt
return Matrix4.LookAt(Position, Position + Front, Up);
```

**GetProjectionMatrix()**
```csharp
// Creates perspective projection
return Matrix4.CreatePerspectiveFieldOfView(
    MathHelper.DegreesToRadians(Zoom),
    aspectRatio,
    0.1f,  // Near plane
    100.0f // Far plane
);
```

**ProcessKeyboard()**
```csharp
// WASD movement
if (W) Position += Front * velocity;
if (S) Position -= Front * velocity;
if (A) Position -= Right * velocity;
if (D) Position += Right * velocity;
```

**ProcessMouseMovement()**
```csharp
// Update rotation based on mouse delta
Yaw += xOffset * MouseSensitivity;
Pitch += yOffset * MouseSensitivity;
// Clamp pitch to prevent flip
Pitch = clamp(Pitch, -89°, 89°);
UpdateCameraVectors();
```

**UpdateCameraVectors()**
```csharp
// Convert Euler angles to direction vectors
Front.x = cos(Yaw) * cos(Pitch);
Front.y = sin(Pitch);
Front.z = sin(Yaw) * cos(Pitch);
Front = normalize(Front);
Right = normalize(cross(Front, WorldUp));
Up = normalize(cross(Right, Front));
```

---

## 🎨 GLSL Shaders

### vertex.glsl (Vertex Shader)
**Purpose**: Transform vertices and prepare data for fragment shader

```glsl
// Input (from VBO)
layout (location = 0) in vec3 aPosition;  // Vertex position
layout (location = 1) in vec3 aNormal;    // Surface normal
layout (location = 2) in vec2 aTexCoord;  // Texture coordinate

// Output (to fragment shader)
out vec3 FragPos;   // World-space position
out vec3 Normal;    // World-space normal
out vec2 TexCoord;  // Texture coordinate

// Uniforms (from C# code)
uniform mat4 model;      // Model transformation
uniform mat4 view;       // Camera transformation
uniform mat4 projection; // Perspective projection

void main()
{
    // Transform position to world space
    FragPos = vec3(model * vec4(aPosition, 1.0));
    
    // Transform normal to world space (handle non-uniform scaling)
    Normal = mat3(transpose(inverse(model))) * aNormal;
    
    // Pass through texture coordinate
    TexCoord = aTexCoord;
    
    // Final position in clip space
    gl_Position = projection * view * vec4(FragPos, 1.0);
}
```

---

### fragment.glsl (Fragment Shader - Phong Lighting)
**Purpose**: Calculate pixel color using Phong lighting model

```glsl
// Input (from vertex shader)
in vec3 FragPos;   // Fragment position in world space
in vec3 Normal;    // Surface normal in world space
in vec2 TexCoord;  // Texture coordinate

// Output
out vec4 FragColor; // Final pixel color

// Uniforms
uniform vec3 viewPos;      // Camera position
uniform vec3 lightPos;     // Light position
uniform vec3 lightColor;   // Light color
uniform vec3 objectColor;  // Base object color
uniform sampler2D texture0; // Texture sampler
uniform bool useTexture;   // Use texture or color?
uniform bool lightEnabled; // Is light on?

void main()
{
    // Get base color
    vec3 baseColor = useTexture 
        ? texture(texture0, TexCoord).rgb 
        : objectColor;
    
    // If light is off, just use dim ambient
    if (!lightEnabled)
    {
        FragColor = vec4(baseColor * 0.3, 1.0);
        return;
    }
    
    // === PHONG LIGHTING MODEL ===
    
    // 1. AMBIENT (base illumination)
    float ambientStrength = 0.2;
    vec3 ambient = ambientStrength * lightColor;
    
    // 2. DIFFUSE (direction-dependent lighting)
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;
    
    // 3. SPECULAR (shiny highlights)
    float specularStrength = 0.8;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;
    
    // Combine all components
    vec3 result = (ambient + diffuse + specular) * baseColor;
    FragColor = vec4(result, 1.0);
}
```

#### Phong Lighting Breakdown:

**Ambient**:
- Simulates indirect lighting
- Constant for all surfaces
- Formula: `ambient = strength × lightColor`

**Diffuse**:
- Lambert's cosine law
- Brighter when surface faces light
- Formula: `diffuse = max(N · L, 0) × lightColor`
  - N = surface normal
  - L = direction to light

**Specular**:
- Shiny highlights
- Depends on view angle
- Formula: `specular = strength × (R · V)^shininess × lightColor`
  - R = reflection of light direction
  - V = direction to viewer
  - shininess = 32 (controls highlight size)

---

## 🔄 Rendering Pipeline Flow

```
1. APPLICATION (C# Code)
   ↓
   • Create vertices, normals, UVs
   • Upload to GPU (VAO/VBO/EBO)
   ↓
2. VERTEX SHADER (vertex.glsl)
   ↓
   • Transform vertices: model → world → view → clip space
   • Calculate world-space normals
   • Pass data to fragment shader
   ↓
3. RASTERIZATION (Hardware)
   ↓
   • Convert triangles to fragments (pixels)
   • Interpolate vertex data
   ↓
4. FRAGMENT SHADER (fragment.glsl)
   ↓
   • Sample textures
   • Calculate Phong lighting
   • Output final color
   ↓
5. FRAMEBUFFER
   ↓
   • Depth test
   • Color blending
   • Display on screen
```

---

## 🎯 Key Algorithms

### Matrix Transformations
```csharp
// Model matrix (object space → world space)
Matrix4 model = Matrix4.CreateRotationY(angle) 
              * Matrix4.CreateTranslation(position);

// View matrix (world space → camera space)
Matrix4 view = camera.GetViewMatrix();

// Projection matrix (camera space → clip space)
Matrix4 projection = camera.GetProjectionMatrix(aspectRatio);

// Final transformation
gl_Position = projection * view * model * position;
```

### Sphere Generation (Parametric)
```csharp
for (int ring = 0; ring <= rings; ring++)
{
    float phi = π * ring / rings;  // Vertical angle
    
    for (int seg = 0; seg <= segments; seg++)
    {
        float theta = 2π * seg / segments;  // Horizontal angle
        
        // Spherical to Cartesian conversion
        float x = radius * sin(phi) * cos(theta);
        float y = radius * cos(phi);
        float z = radius * sin(phi) * sin(theta);
        
        // Normal = normalized position (for unit sphere)
        Vector3 normal = normalize(new Vector3(x, y, z));
        
        // UV mapping
        float u = (float)seg / segments;
        float v = (float)ring / rings;
    }
}
```

### Camera Rotation (Euler Angles)
```csharp
// Update angles from mouse input
Yaw += deltaX * sensitivity;
Pitch += deltaY * sensitivity;

// Clamp pitch to prevent gimbal lock
Pitch = clamp(Pitch, -89°, 89°);

// Convert to direction vector
Front.x = cos(radians(Yaw)) * cos(radians(Pitch));
Front.y = sin(radians(Pitch));
Front.z = sin(radians(Yaw)) * cos(radians(Pitch));
Front = normalize(Front);

// Calculate right and up vectors
Right = normalize(cross(Front, WorldUp));
Up = normalize(cross(Right, Front));
```

---

## 📊 Performance Considerations

### Optimizations Used:

1. **Static Geometry**
   - Meshes created once in OnLoad()
   - Reused across frames

2. **Element Buffer Objects (EBO)**
   - Reduces vertex duplication
   - Planet: 1024 unique vertices, 3000+ triangles

3. **Texture Mipmaps**
   - Improves performance for distant objects
   - Reduces aliasing

4. **Depth Testing**
   - Hardware-accelerated occlusion
   - Only renders visible fragments

5. **Frame Rate Control**
   - Fixed 60 FPS update loop
   - Decoupled from render loop

---

## 🔧 Common Modifications

### Change Planet Speed
```csharp
// In Game.cs, OnUpdateFrame():
_planetRotation += 20.0f * deltaTime; // Was 10.0f
```

### Add Another Object
```csharp
// 1. Create mesh in OnLoad():
_asteroidMesh = Mesh.CreateSphere(0.5f, 16, 8);

// 2. Render in OnRenderFrame():
Matrix4 asteroidModel = Matrix4.CreateTranslation(x, y, z);
_shader.SetMatrix4("model", asteroidModel);
_asteroidMesh.Draw();
```

### Change Light Position
```csharp
// In Game.cs, OnRenderFrame():
Vector3 lightPos = new Vector3(-10.0f, 8.0f, -5.0f);
_shader.SetVector3("lightPos", lightPos);
```

### Adjust Camera Speed
```csharp
// In Graphics/Camera.cs constructor or OnLoad():
_camera.MovementSpeed = 8.0f; // Was 5.0f
```

---

## 📚 Learning Resources

### OpenGL Concepts:
- **VAO**: Stores vertex attribute configuration
- **VBO**: Stores actual vertex data
- **EBO**: Stores indices to reuse vertices
- **Shader**: Program that runs on GPU
- **Uniform**: Global variable in shader
- **Attribute**: Per-vertex data

### Matrix Math:
- **Model Matrix**: Positions object in world
- **View Matrix**: Positions world relative to camera
- **Projection Matrix**: Creates perspective
- **Normal Matrix**: `transpose(inverse(model))` for lighting

### Lighting Terms:
- **Ambient**: Base illumination
- **Diffuse**: Direction-dependent light
- **Specular**: Shiny highlights
- **Normal**: Surface orientation vector

---

This reference should help you understand and explain any part of the code!
