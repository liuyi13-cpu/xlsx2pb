@echo off  

set workPath=%~dp0

call .\protoc.exe  --proto_path ..\res\proto --proto_path .\ --cpp_out ..\proto\ tsv2pb.proto tools.proto

if not %errorlevel%==0 (
goto endFail 
) else (
goto endSucc
)    
:endFail
echo FAILED
pause
exit /b 1

:endSucc
exit /b 0