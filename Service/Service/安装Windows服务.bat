
echo ���ڰ�װ����. . .
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe %~dp0%..\Service.exe > InstallService.log
echo ����װ��ϣ���������. . .
net start ���ɷ��� >> InstallService.log
echo ������������ϸ������ڡ�InstallService.log���в鿴��
pause