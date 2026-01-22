#!/usr/bin/env pwsh
# rebuild_igcl.ps1 - Rebuilds IGCL C# Wrapper and runs tests

#
# IGCLWrapper Build Script (PowerShell)
# Builds the ClangSharp-based C# wrapper for IGCL
#

# Get the directory where this script is located
$scriptRoot = $PSScriptRoot
if (-not $scriptRoot) {
    $scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
}

# Change to script directory
Set-Location $scriptRoot
Write-Host "Working directory: $scriptRoot" -ForegroundColor Cyan
Write-Host ""

Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "IGCLWrapper Rebuild Script (ClangSharp Implementation)" -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

# ============================================================================
# Read version from VERSION file and calculate build number from git commits
# ============================================================================
Write-Host "Determining version number..." -ForegroundColor Yellow

$versionFile = Join-Path $scriptRoot "VERSION"
if (-not (Test-Path $versionFile)) {
    Write-Host "ERROR: VERSION file not found: $versionFile" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

# Read MAJOR and MINOR from VERSION file
$versionContent = Get-Content $versionFile
$major = "1"
$minor = "0"

foreach ($line in $versionContent) {
    if ($line -match "^MAJOR=(\d+)") {
        $major = $matches[1]
    }
    elseif ($line -match "^MINOR=(\d+)") {
        $minor = $matches[1]
    }
}

# Get git commit count for PATCH/build number
$patch = "0"
try {
    # Check if git is available
    $gitPath = Get-Command git -ErrorAction SilentlyContinue
    if ($gitPath) {
        # Get the commit count
        $commitCount = & git rev-list --count HEAD 2>&1
        if ($LASTEXITCODE -eq 0 -and $commitCount -match "^\d+$") {
            $patch = $commitCount
        }
        else {
            Write-Host "Warning: Could not get git commit count, using 0" -ForegroundColor Yellow
        }
    }
    else {
        Write-Host "Warning: git not found in PATH, using PATCH=0" -ForegroundColor Yellow
    }
}
catch {
    Write-Host "Warning: Error reading git commit count: $_" -ForegroundColor Yellow
    Write-Host "Using PATCH=0" -ForegroundColor Yellow
}

$version = "$major.$minor.$patch"
Write-Host "Version: $version" -ForegroundColor Green
Write-Host "  MAJOR: $major (from VERSION file)" -ForegroundColor Gray
Write-Host "  MINOR: $minor (from VERSION file)" -ForegroundColor Gray
Write-Host "  PATCH: $patch (git commit count)" -ForegroundColor Gray
Write-Host ""

# ============================================================================
# Verify dotnet CLI is available
# ============================================================================
Write-Host "Checking for .NET CLI..." -ForegroundColor Yellow

try {
    $dotnetPath = Get-Command dotnet -ErrorAction Stop
    $dotnetVersion = & dotnet --version 2>&1
    Write-Host ".NET CLI found: $($dotnetPath.Source)" -ForegroundColor Green
    Write-Host ".NET version: $dotnetVersion" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host ""
    Write-Host "ERROR: dotnet CLI not found in PATH" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please ensure .NET 10.0 SDK is installed" -ForegroundColor Yellow
    Write-Host "Download from: https://dotnet.microsoft.com/download/dotnet/10.0" -ForegroundColor Cyan
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}

# ============================================================================
# Check .NET 10.0 SDK availability
# ============================================================================
Write-Host "Checking for .NET 10.0 SDK..." -ForegroundColor Yellow

try {
    $sdks = & dotnet --list-sdks 2>&1
    $net9Sdk = $sdks | Where-Object { $_ -match "10\.0\." }
    
    if ($net9Sdk) {
        Write-Host ".NET 10.0 SDK found:" -ForegroundColor Green
        $net9Sdk | ForEach-Object { Write-Host "  $_" -ForegroundColor Green }
        Write-Host ""
    } else {
        Write-Host ""
        Write-Host "ERROR: .NET 10.0 SDK not found" -ForegroundColor Red
        Write-Host ""
        Write-Host "Available SDKs:" -ForegroundColor Yellow
        $sdks | ForEach-Object { Write-Host "  $_" -ForegroundColor Gray }
        Write-Host ""
        Write-Host "Please install .NET 10.0 SDK from: https://dotnet.microsoft.com/download/dotnet/10.0" -ForegroundColor Cyan
        Write-Host ""
        Read-Host "Press Enter to exit"
        exit 1
    }
} catch {
    Write-Host ""
    Write-Host "ERROR: Failed to check .NET SDK version: $_" -ForegroundColor Red
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "Checking for Intel IGCL SDK..." -ForegroundColor Yellow
# Check if IGCL SDK headers are present
$igclHeaderPath = "drivers.gpu.control-library\include\igcl_api.h"
if (-not (Test-Path $igclHeaderPath)) {
    Write-Host "Warning: IGCL SDK headers not found at $igclHeaderPath" -ForegroundColor Yellow
    Write-Host "Please run prepare_igcl.ps1 first to download the IGCL SDK." -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}
Write-Host "Intel IGCL SDK found" -ForegroundColor Gray


# ============================================================================
# Restore NuGet packages
# ============================================================================
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "Restoring NuGet packages..." -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

$solutionPath = Join-Path $scriptRoot "IGCLWrapper.sln"

if (-not (Test-Path $solutionPath)) {
    Write-Host "ERROR: Solution file not found: $solutionPath" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

try {
    & dotnet restore $solutionPath
    
    if ($LASTEXITCODE -ne 0) {
        throw "Restore failed with exit code $LASTEXITCODE"
    }
    
    Write-Host ""
    Write-Host "NuGet packages restored successfully!" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host ""
    Write-Host "ERROR: Failed to restore NuGet packages!" -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}

# ============================================================================
# Clean the solution
# ============================================================================
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "Cleaning IGCLWrapper solution..." -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

try {
    Write-Host "dotnet clean $solutionPath"
    dotnet clean $solutionPath 
    
    if ($LASTEXITCODE -ne 0) {
        throw "Clean failed with exit code $LASTEXITCODE"
    }
    
    Write-Host ""
    Write-Host "Clean completed successfully!" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host ""
    Write-Host "ERROR: Clean failed!" -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Troubleshooting:" -ForegroundColor Yellow
    Write-Host "  - Ensure all NuGet packages are restored" -ForegroundColor Gray
    Write-Host "  - Verify .NET 10.0 SDK is installed" -ForegroundColor Gray
    Write-Host "  - Check project files for errors" -ForegroundColor Gray
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}

# ============================================================================
# Rebuild the solution
# ============================================================================
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "Rebuilding IGCLWrapper solution..." -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

# Build the C# wrapper project (ClangSharp will generate P/Invoke bindings during build)
Write-Host "`nBuilding IGCLWrapper C# project..." -ForegroundColor Cyan

try 
{
    Write-Host "dotnet build $solutionPath --configuration Debug --no-restore /p:Version=$version /p:AssemblyVersion=$version /p:FileVersion=$version"
    dotnet build $solutionPath --configuration Debug --no-restore /p:Version=$version /p:AssemblyVersion=$version /p:FileVersion=$version
    #& dotnet build IGCLWrapper\IGCLWrapper.csproj -c Debug

    if ($LASTEXITCODE -ne 0) {
        throw "Rebuild failed with exit code $LASTEXITCODE"
    }
    
    Write-Host ""
    Write-Host "Rebuild completed successfully!" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host ""
    Write-Host "ERROR: Rebuild failed!" -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Troubleshooting:" -ForegroundColor Yellow
    Write-Host "  - Ensure all NuGet packages are restored" -ForegroundColor Gray
    Write-Host "  - Verify .NET 10.0 SDK is installed" -ForegroundColor Gray
    Write-Host "  - Check project files for errors" -ForegroundColor Gray
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}

# ============================================================================
# Success summary
# ============================================================================
Write-Host "============================================================================" -ForegroundColor Green
Write-Host "*** REBUILD SUCCESSFUL! ***" -ForegroundColor Green
Write-Host "============================================================================" -ForegroundColor Green
Write-Host ""
Write-Host "Projects built:" -ForegroundColor Cyan
Write-Host "  - IGCLWrapper (ClangSharp-based wrapper)" -ForegroundColor Green
Write-Host "  - IGCLWrapper.NativeTests (Test suite)" -ForegroundColor Green
Write-Host "  - IGCLWrapper.FacadeTests (Test suite)" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "  - Run tests: .\test_igcl.ps1" -ForegroundColor Gray
Write-Host "  - Use in your project: Add reference to ADLXWrapper\ADLXWrapper.csproj" -ForegroundColor Gray
Write-Host ""
Write-Host "Press Enter to exit..."
Read-Host

