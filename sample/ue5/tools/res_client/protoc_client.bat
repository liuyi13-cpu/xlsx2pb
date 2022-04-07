@echo off  
setlocal EnableDelayedExpansion
chcp 65001

set workPath=%~dp0

cd ..
set protoPath=res\proto\client

set protoList=
for /r %protoPath% %%i in (*.proto) do (
	set protoList=!protoList! %%~nxi
)

del /F/S/Q .\..\project\Source\Proto\Res\*.h
del /F/S/Q .\..\project\Source\Proto\Res\*.cc
call .\res_client\protoc.exe -I %protoPath% --cpp_out=.\..\project\Source\Proto\Res -o .\..\project\Content\res\data\proto_res.bytes !protoList!

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