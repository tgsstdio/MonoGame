GLSL to SPIRV transcompiler
WIN32 executable
SEE https://github.com/KhronosGroup/glslang.git

EXAMPLE: 

glslangValidator.exe -V -o 80000002.spirv 80000002.vert

glslangValidator.exe -V -o 80000003.spirv 80000003.frag

glslangValidator.exe -V -o 80000002.spv 80000002.vert

glslangValidator.exe -V -o 80000003.spv 80000003.frag

SPIRV REMAP for better compression
EXAMPLE
spirv-remap -v --map all --input 80000002.spv 80000003.spv --output out\
