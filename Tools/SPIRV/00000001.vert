#version 430 core

layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_uv;

void vertMain()
{
	gl_Position = vec4(in_position, 0, 1);
}