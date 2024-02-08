
echo 正在安装服务. . .
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe %~dp0%..\Service.exe > InstallService.log
echo 服务安装完毕，启动服务. . .
net start 数采服务 >> InstallService.log
echo 操作结束，详细结果请在【InstallService.log】中查看。
pause