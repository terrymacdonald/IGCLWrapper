#!/usr/bin/env pwsh
# rebuild_igcl.ps1 - Rebuilds IGCL C# Wrapper and runs tests

Write-Host "Rebuilding IGCL C# Wrapper..." -ForegroundColor Cyan

# Check if .NET 8.0 SDK is available
Write-Host "`nChecking .NET SDK..." -ForegroundColor Gray
$dotnetVersion = & dotnet --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error: .NET SDK not found. Please install .NET 8.0 SDK or later." -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}
Write-Host "Found .NET SDK version: $dotnetVersion" -ForegroundColor Gray

# Check if IGCL SDK headers are present
$igclHeaderPath = "drivers.gpu.control-library\include\igcl_api.h"
if (-not (Test-Path $igclHeaderPath)) {
    Write-Host "Warning: IGCL SDK headers not found at $igclHeaderPath" -ForegroundColor Yellow
    Write-Host "Please run prepare_igcl.ps1 first to download the IGCL SDK." -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}
Write-Host "IGCL SDK headers found" -ForegroundColor Gray

# Build the C# wrapper project (ClangSharp will generate P/Invoke bindings during build)
Write-Host "`nBuilding IGCLWrapper C# project..." -ForegroundColor Cyan
& dotnet build IGCLWrapper\IGCLWrapper.csproj -c Debug

if ($LASTEXITCODE -ne 0) {
    Write-Host "IGCLWrapper build failed!" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "IGCLWrapper build completed successfully!" -ForegroundColor Green

# Build and run unit tests
Write-Host "`nBuilding and running unit tests..." -ForegroundColor Cyan
& dotnet test IGCLWrapper.Tests\IGCLWrapper.Tests.csproj -c Debug -f net8.0

if ($LASTEXITCODE -ne 0) {
    Write-Host "Unit tests failed!" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "`nAll builds and tests completed successfully!" -ForegroundColor Green
Write-Host "Generated DLL location: IGCLWrapper\bin\Debug\net8.0\IGCLWrapper.dll" -ForegroundColor Gray
Read-Host "Press Enter to exit"
