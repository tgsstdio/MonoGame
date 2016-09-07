# BirdNest.MonoGame

All in one compilation using Portable Core Libraries for cross-platform (and no preprocessors)
Magnesium graphical interface (i.e. Vulkan) overhaul to two implementation
 - Magnesium.OpenGL
	- Magnesium.OpenGL.DesktopGL 		
 - Magnesium.Vulkan
	- Vulkan backend for Magnesium

Using PCL profile 111
 - .NET Framework 4.5 or later
 - ASP .NET Core 1.0
 - Windows 8
 - Windows Phone 8.1
 - Xamarin.Android
 - Xamarin.iOS
 - Xamarin.iOS (Classic) 
 
Formerly strictly AZDO OpenGL fork of MonoGame

Implemented around modern OpenGL design features such as
 - bindless textures 
 - upfront/once-off OpenGL calls
 - GLSL shaders entirely for rendering
 - multi indirect draw calls
 - vertex buffer objects
 - compute shaders (TODO)
 - shader storage buffer objects 

Licensed under MIT license (my contributions) and Microsoft Public License.

## Shader Guide
SEE https://matthewwellings.com/blog/the-new-vulkan-coordinate-system/

# GLSL to Vulkan
Vulkan uses a right handed coordinate-system with Y axis going down the screen and Z axis positive going into the screen
OpenGL uses a left handed coordinate-system with Y axis going up the screen and Z axis positive going into the screen.

Therefore GLSL vertex shader source files used by Magnesium.OpenGL should be in the ammended with the following line.

gl_Position.y = -gl_Position.y;

Transcoded GLSL to SPIRV shader files SHOULD NOT be ammended from Magnesium.Vulkan. PLS Note: the original fork for MonoGame had previously modified GLSL source code at runtime to simulate XNA coordinate-system.

Asset Workflow
 - Create GLSL shader files to applications
 - Take a copy of original GLSL shader files and add "gl_Position.y = -gl_Position.y;" to all GLSL vertex shader files used by Magnesium.OpenGL
 - Take a copy of original GLSL shader files and transcompile original GLSL to SPIRV files.
  
