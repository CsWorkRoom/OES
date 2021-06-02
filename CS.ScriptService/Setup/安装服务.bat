
@echo off

echo 注意‘服务名称’与应用程序名称 '*.exe'的差别
set ExeName=CS.ScriptService.exe

echo 即将安装应用程序%ExeName% 为服务
set Path=%~dp0
set exePath=%Path:~0,-6%%ExeName%
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe %exePath%

echo.
pause
