@echo off
curl http://localhost:8622/stop -s
echo.>.\unblock.ps1:Zone.Identifier
powershell -NoProfile -ExecutionPolicy Unrestricted .\unblock.ps1
set DST=%USERPROFILE%\lsFeed
if exist "%DST%" (
  rmdir "%DST%\Content" /S /Q
  del "%DST%\lsFeed.exe" /Q
  del "%DST%\readme.txt" /Q
) else (
  mkdir "%DST%"
)
xcopy Build "%DST%" /S /E /F /G /H /R /K /Y /Q
pause
