
@echo off

echo ע�⡮�������ơ���Ӧ�ó������� '*.exe'�Ĳ��
set ExeName=CS.ScriptService.exe

echo ������װӦ�ó���%ExeName% Ϊ����
set Path=%~dp0
set exePath=%Path:~0,-6%%ExeName%
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe %exePath%

echo.
pause
