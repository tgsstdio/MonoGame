#version 150
#extension GL_ARB_explicit_uniform_location : require
#extension GL_ARB_bindless_texture : require

layout (location = 0) uniform sampler2D Texture;

out vec4 fragColor;

void main()
{
	fragColor = vec4(0, 0, 1, 1.0f);
}