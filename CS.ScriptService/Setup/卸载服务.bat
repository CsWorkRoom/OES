@echo off

echo ע�⡮�������ơ���Ӧ�ó������� '*.exe'�Ĳ��
set SvcName=EasymanScriptService_csTest

echo ����ֹͣ����%SvcName%
net stop %SvcName%

set ExeName=CS.ScriptService.exe

echo ����ж��Ӧ�ó���%ExeName% �ķ���
set Path=%~dp0
set exePath=%Path:~0,-6%%ExeName%
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe %exePath% -u

echo.
pause 
