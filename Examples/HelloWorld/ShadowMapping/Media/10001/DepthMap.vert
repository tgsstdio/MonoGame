#version 330 core
 
layout(location = 0) in vec3 in_position;
layout(location = 1) in uint in_drawId;

struct ModelData 
{
	mat4 Transform;
};

layout(binding = 0, std430) buffer ssbo_0
{
	ModelData models[];
};

struct CameraData
{
	mat4 ViewMatrix;
	mat4 ProjectionMatrix;
};

layout(binding = 1, std430) buffer ssbo_1
{
	CameraData cameras[];
};

struct LightData
{
	vec4 Position;
	mat4 ViewMatrix;
	mat4 ProjectionMatrix;
};

layout(binding = 2, std430) buffer ssbo_2
{
	LightData lights[];
};
 
// Values that stay constant for the whole mesh.

uniform uint currentLight;
uniform uint currentCamera;

void main(){
	vec4 finalPosition = models[in_drawId].Transform * vec4(in_position, 1);
	gl_Position =  lights[currentLight].ProjectionMatrix * lights[currentLight].ViewMatrix * finalPosition;
}
