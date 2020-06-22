::@echo off
chcp 65001 2>nul >nul

pushd "%~dp0"

echo (will show help entries, output to stdout, then exit, exit-code 0).
"%~dp0set_time\bin\Release\set_time.exe" --help
echo Exit-code: %ErrorLevel%.
echo.

echo (error, missing --target, verbose mode, exit-code 111).
"%~dp0set_time\bin\Release\set_time.exe" --verbose
echo Exit-code: %ErrorLevel%.
echo.

echo (error, missing at least one --set-time-*, verbose mode, exit-code 222).
"%~dp0set_time\bin\Release\set_time.exe" --verbose --target "%~dp0dummy folder"
echo Exit-code: %ErrorLevel%.
echo.

echo (will set all timedates of folder, verbose mode, exit-code 0).
"%~dp0set_time\bin\Release\set_time.exe" --target "%~dp0dummy folder" --set-time-creation 123456789 --set-time-last-access 123456789 --set-time-last-modified 123456789 --verbose
echo Exit-code: %ErrorLevel%.
echo.

echo (will set all timedates of file, verbose mode, exit code 0).
"%~dp0set_time\bin\Release\set_time.exe" --target "%~dp0dummy file.txt" --set-time-creation 123456789 --set-time-last-access 123456789 --set-time-last-modified 123456789 --verbose
echo Exit-code: %ErrorLevel%.
echo.

popd

pause

exit /b 0