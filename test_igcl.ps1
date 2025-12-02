#!/usr/bin/env pwsh
# test_igcl.ps1 - Runs IGCL C# Wrapper unit tests


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
Write-Host "IGCLWrapper Test Suite (ClangSharp Implementation)" -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

# ============================================================================
# Read version from VERSION file and calculate build number from git commits
# ============================================================================
Write-Host "Determining version number..." -ForegroundColor Yellow

$versionFile = Join-Path $scriptRoot "VERSION"
if (-not (Test-Path $versionFile)) {
    Write-Host "Warning: VERSION file not found, using default version" -ForegroundColor Yellow
    $version = "1.0.0"
} else {
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
        }
    }
    catch {
        # Silently fall back to 0
    }

    $version = "$major.$minor.$patch"
}

Write-Host "Testing version: $version" -ForegroundColor Green
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
    $net9Sdk = $sdks | Where-Object { $_ -match "9\.0\." }
    
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

# ============================================================================
# Verify test project exists
# ============================================================================
$testProjectPath = Join-Path $scriptRoot "IGCLWrapper.Tests\IGCLWrapper.Tests.csproj"

if (-not (Test-Path $testProjectPath)) {
    Write-Host "ERROR: Test project not found at: $testProjectPath" -ForegroundColor Red
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "Test project found: $testProjectPath" -ForegroundColor Green
Write-Host ""

# ============================================================================
# Build and run unit tests
# ============================================================================
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "Building and running ClangSharp-based unit tests..." -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

try {
    # Run tests with detailed console output
    & dotnet test $testProjectPath --configuration Debug --verbosity normal
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "============================================================================" -ForegroundColor Green
        Write-Host "*** ALL TESTS PASSED! ***" -ForegroundColor Green
        Write-Host "============================================================================" -ForegroundColor Green
        Write-Host "All unit tests completed successfully using .NET 10.0" -ForegroundColor Green
        Write-Host ""
    } else {
        Write-Host ""
        Write-Host "============================================================================" -ForegroundColor Yellow
        Write-Host "*** SOME TESTS FAILED OR WERE SKIPPED ***" -ForegroundColor Yellow
        Write-Host "============================================================================" -ForegroundColor Yellow
        Write-Host "Exit code: $LASTEXITCODE" -ForegroundColor Yellow
        Write-Host ""
        Write-Host "Troubleshooting tips:" -ForegroundColor Yellow
        Write-Host "  - Tests gracefully skip if Intel hardware is not available" -ForegroundColor Gray
        Write-Host "  - Ensure Intel GPU with IGCL support is present" -ForegroundColor Gray
        Write-Host "  - Verify latest Intel Video drivers are installed" -ForegroundColor Gray
        Write-Host "  - Review test output above for specific failure details" -ForegroundColor Gray
        Write-Host ""
        Write-Host "Note: Tests automatically skip if hardware/drivers don't support them." -ForegroundColor Cyan
        Write-Host ""
        
        Read-Host "Press Enter to exit..."
        exit 1
    }
} catch {
    Write-Host ""
    Write-Host "ERROR: Failed to run unit tests!" -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Press Enter to exit..."
    exit 1
}
Read-Host "Press Enter to exit..."
