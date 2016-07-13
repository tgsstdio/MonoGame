# BirdNest.MonoGame

All in one compilation using Portable Core Libraries for cross-platform (and no preprocessors)
Magnesium graphical interface (i.e. Vulkan) overhaul to two implementation
 - OpenGL backend
 - (TODO) Vulkan 

Using PCL profile 259
 - .NET Framework 4.5 or later
 - Windows Phone Silverlight 8 or later 
 - ASP .NET Core 5.0
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
 - compute shaders 
 - shader storage buffer objects 

Licensed under MIT license (my contributions) and Microsoft Public License.