glslangValidator.exe -V -o 80000002.a 80000002.vert
spirv-dis -o 80000002.b 80000002.a

glslangValidator.exe -V -o 80000003.a 80000003.frag
spirv-dis -o 80000003.b 80000003.a

REM Requires search-and-replace "main" within glsl disassemblies with custom entrypoint
REM POWERSHELL
REM (Get-Content 80000002.b).replace('"main"', '"vmain"') | Set-Content 80000002.c
REM (Get-Content 80000003.b).replace('"main"', '"fmain"') | Set-Content 80000003.c

REM Reassembly new SPIRV file from update .dst files
REM spirv-as -o 80000002.spv 80000002.c
REM spirv-as -o 80000003.spv 80000003.c