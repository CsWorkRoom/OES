@echo off

echo 注意‘服务名称’与应用程序名称 '*.exe'的差别
set SvcName=EasymanScriptService_csTest

echo 即将停止服务%SvcName%
net stop %SvcName%

set ExeName=CS.ScriptService.exe

echo 即将卸载应用程序%ExeName% 的服务
set Path=%~dp0
set exePath=%Path:~0,-6%%ExeName%
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe %exePath% -u

echo.
pause 
