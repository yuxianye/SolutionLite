echo 正在卸载服务. . .
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /U %~dp0%..\Service.exe > InstallService.log
echo 操作结束，详细结果请在【InstallService.log】中查看。
pause