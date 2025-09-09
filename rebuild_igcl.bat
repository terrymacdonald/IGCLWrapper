@echo off
echo Rebuilding IGCL C# Wrapper...

REM Set up Visual Studio environment
call "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat" >nul 2>&1
if errorlevel 1 (
    call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat" >nul 2>&1
)

REM Rebuild the native C++ project
echo Building native DLL...
msbuild IGCLWrapper\IGCLWrapper.vcxproj /p:Configuration=Debug /p:Platform=x64 /t:Rebuild /v:minimal
if errorlevel 1 (
    echo Native C++ build failed!
    pause
    exit /b 1
)
echo Native C++ build completed successfully!

REM Ensure the target directories exist for copying
if not exist "IGCLWrapper.Tests\bin\Debug\net8.0\" mkdir "IGCLWrapper.Tests\bin\Debug\net8.0\"

REM Copy generated C# binding files to IGCLWrapper.Tests project
echo Copying C# binding files...
xcopy /Y IGCLWrapper\cs_bindings\*.cs IGCLWrapper.Tests\cs_bindings\ >nul
if errorlevel 1 (
    echo Failed to copy C# binding files!
    pause
    exit /b 1
)

REM Copy compiled IGCLWrapper.dll to IGCLWrapper.Tests project
echo Copying IGCLWrapper.dll...
copy IGCLWrapper\x64\Debug\IGCLWrapper.dll IGCLWrapper.Tests\bin\Debug\net8.0\ >nul
if errorlevel 1 (
    echo Failed to copy IGCLWrapper.dll to Tests project!
    pause
    exit /b 1
)

REM Build and run unit tests
echo Building and running unit tests...
dotnet test IGCLWrapper.Tests\IGCLWrapper.Tests.csproj -c Debug -f net8.0
if errorlevel 1 (
    echo Unit tests failed!
    pause
    exit /b 1
)

echo All builds and tests completed successfully!
pause
