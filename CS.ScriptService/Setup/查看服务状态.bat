@echo off

echo ע�⡮�������ơ������ '*.exe'�Ĳ��

set SvcName=EasymanScriptServiceV4
echo Service state: %SvcName%
sc query %SvcName%

echo.
pause