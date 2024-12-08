@echo off
set appname=KeepAwake
set artifact="%~dp0Release\%appname%.exe"
cls
echo ****************************************
echo Starting publishing...
echo ****************************************
echo.
echo ========================================
echo dotnet restore
echo ========================================
dotnet restore
echo.
echo ========================================
echo Starting Cake build...
echo ========================================
Powershell.exe -executionpolicy remotesigned -File build.ps1 --configuration=Release --framework=net9.0-windows --runtime=win-x64 --selfcontained=false --publishdir=../Release --publishexename="%appname%"
echo.
echo ========================================
echo Artifact
echo ========================================
if not exist %artifact% (
	echo Publishing with error finished. Binary can not be found in subfolder %artifact%
) else (
	echo Publishing finished. Binary can be found in subfolder %artifact%
)
echo.
pause
