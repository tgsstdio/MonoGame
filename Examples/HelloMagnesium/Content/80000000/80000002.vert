#version 420
#extension GL_ARB_explicit_uniform_location : require

layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_uv;

void main()
{
	gl_Position = vec4(in_position.x, -in_position.y, 0, 1);
}