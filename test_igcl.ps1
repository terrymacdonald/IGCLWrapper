#!/usr/bin/env pwsh
# test_igcl.ps1 - Runs IGCL C# Wrapper unit tests

Write-Host "Running IGCL C# Wrapper unit tests..." -ForegroundColor Cyan

# Check if the test project exists
$testProject = "IGCLWrapper.Tests\IGCLWrapper.Tests.csproj"
if (-not (Test-Path $testProject)) {
    Write-Host "Error: Test project not found at $testProject" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

# Check if the main IGCLWrapper project has been built
$wrapperProject = "IGCLWrapper\IGCLWrapper.csproj"
if (Test-Path $wrapperProject) {
    # Try to find the DLL in the wrapper project's output directory
    $dllPattern = "IGCLWrapper\bin\Debug\*\IGCLWrapper.dll"
    $wrapperDll = Get-ChildItem $dllPattern -ErrorAction SilentlyContinue | Select-Object -First 1
    
    if (-not $wrapperDll) {
        Write-Host "Warning: IGCLWrapper.dll not found in the wrapper project output directory." -ForegroundColor Yellow
        Write-Host "Please run rebuild_igcl.ps1 first to build the wrapper project." -ForegroundColor Yellow
    } else {
        Write-Host "Found IGCLWrapper.dll at: $($wrapperDll.FullName)" -ForegroundColor Gray
    }
} else {
    Write-Host "Warning: IGCLWrapper project not found at $wrapperProject" -ForegroundColor Yellow
}

# Run unit tests
Write-Host "`nExecuting tests..." -ForegroundColor Cyan
& dotnet test $testProject -c Debug -f net8.0

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nUnit tests failed!" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "`nAll tests passed successfully!" -ForegroundColor Green
Read-Host "Press Enter to exit"
