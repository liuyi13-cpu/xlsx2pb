@echo off  

set workPath=%~dp0

call .\xls2pb_cpp.exe --xlsx_input=./excel --tsv_path=./tsv/client --proto_path=./proto/client --store_path=./store/client --store_suffix=bytes

pause

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