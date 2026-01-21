# Enhanced IGCL Preparation Script with Improved Error Handling


# Get the directory where this script is located (works from any location)
$scriptRoot = $PSScriptRoot
if (-not $scriptRoot) {
    $scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
}

# Change to script directory to ensure relative paths work correctly
Set-Location $scriptRoot
Write-Host "Working directory: $scriptRoot" -ForegroundColor Cyan
Write-Host ""

# Define IGCL-related variables
$zipUrl = "https://github.com/intel/drivers.gpu.control-library/archive/refs/heads/master.zip"
$zipFilePath = Join-Path $scriptRoot "master.zip"
$destinationFolder = Join-Path $scriptRoot "drivers.gpu.control-library"
$tempExtractFolder = Join-Path $scriptRoot "drivers.gpu.control-library-master"
$outFolder = Join-Path $scriptRoot "out"

# Function to validate IGCL SDK completeness
function Test-IGCLSDKCompleteness {
    param([string]$IGCLPath)
    
    $requiredPaths = @(
        "$IGCLPath\Source\cApiWrapper.cpp",
        "$IGCLPath\include\igcl_api.h"
    )
    
    $missingFiles = @()
    foreach ($path in $requiredPaths) {
        if (-not (Test-Path -Path $path)) {
            $missingFiles += $path
        }
    }
    
    if ($missingFiles.Count -gt 0) {
        Write-Host "ERROR: IGCL SDK is incomplete. Missing files:" -ForegroundColor Red
        foreach ($file in $missingFiles) {
            Write-Host "  - $file" -ForegroundColor Red
        }
        return $false
    }
    
    Write-Host "IGCL SDK validation passed - all required files present." -ForegroundColor Green
    return $true
}

# Function to check internet connectivity
function Test-InternetConnection {
    try {
        $response = Invoke-WebRequest -Uri "https://www.google.com" -Method Head -TimeoutSec 10 -ErrorAction Stop
        return $true
    } catch {
        return $false
    }
}

# ============================================================================
# Main Script Execution
# ============================================================================

Write-Host "=== Enhanced IGCL Preparation Script ===" -ForegroundColor Cyan
Write-Host "Preparing SWIG and IGCL SDK ..." -ForegroundColor Cyan

# ============================================================================
# Check .NET 10.0 SDK (informational only)
# ============================================================================
Write-Host "Checking for .NET 10.0 SDK..." -ForegroundColor Yellow

$dotnetInstalled = $false
$net10Installed = $false

try {
    $dotnetPath = Get-Command dotnet -ErrorAction SilentlyContinue
    if ($dotnetPath) {
        $dotnetInstalled = $true
        $sdks = & dotnet --list-sdks 2>&1
        $net10Sdk = $sdks | Where-Object { $_ -match "10\.0\." }
        
        if ($net10Sdk) {
            $net10Installed = $true
            Write-Host ".NET 10.0 SDK found:" -ForegroundColor Green
            $net10Sdk | ForEach-Object { Write-Host "  $_" -ForegroundColor Green }
        } else {
            Write-Host ".NET CLI found, but .NET 10.0 SDK not installed" -ForegroundColor Yellow
            Write-Host "Available SDKs:" -ForegroundColor Gray
            $sdks | ForEach-Object { Write-Host "  $_" -ForegroundColor Gray }
        }
    } else {
        Write-Host ".NET CLI not found in PATH" -ForegroundColor Yellow
    }
} catch {
    Write-Host "Could not check .NET installation: $_" -ForegroundColor Yellow
}

if (-not $net10Installed) {
    Write-Host ""
    Write-Host "??  WARNING: .NET 10.0 SDK not detected" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "To build this project, you need:" -ForegroundColor Yellow
    Write-Host "  - .NET 10.0 SDK" -ForegroundColor Cyan
    Write-Host "  - Download from: https://dotnet.microsoft.com/download/dotnet/10.0" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Or install via Visual Studio:" -ForegroundColor Yellow
    Write-Host "  - Open Visual Studio Installer" -ForegroundColor Cyan
    Write-Host "  - Modify your installation" -ForegroundColor Cyan
    Write-Host "  - Under 'Individual components', select '.NET 10.0 Runtime'" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Continuing with ADLX SDK download..." -ForegroundColor Gray
    Write-Host ""
}

Write-Host ""

# Check internet connection
Write-Host "Checking internet connectivity..."
if (-not (Test-InternetConnection)) {
    Write-Host "ERROR: No internet connection detected." -ForegroundColor Red
    Write-Host "This script requires internet access to download the ADLX SDK." -ForegroundColor Red
    exit 1
}
Write-Host "Internet connection verified." -ForegroundColor Green
Write-Host ""

# ============================================================================
# ClangSharpPInvokeGenerator Nuget Install
# ============================================================================
Write-Host "=== Installing ClangSharpPInvokeGenerator Nuget Package ===" -ForegroundColor Cyan

if ($net10Installed) {
    Write-Host "Running dotnet tool installer" -ForegroundColor Green
    dotnet tool install --global ClangSharpPInvokeGenerator
} else {
    Write-Host "Skipping ClangSharpPInvokeGenerator installation (requires .NET 10.0 SDK)" -ForegroundColor Yellow
}
Write-Host ""

# ============================================================================
# Docfx Nuget Install
# ============================================================================
Write-Host "=== Installing Docfx Nuget Package ===" -ForegroundColor Cyan

if ($net10Installed) {
    Write-Host "Running dotnet tool installer" -ForegroundColor Green
    dotnet tool install -g docfx
} else {
    Write-Host "Skipping Docfx installation (requires .NET 10.0 SDK)" -ForegroundColor Yellow
}
Write-Host ""

# ============================================================================
# IGCL SDK Download
# ============================================================================
Write-Host ""
Write-Host "=== IGCL SDK Dependency Check ===" -ForegroundColor Cyan

# Check if IGCL folder already exists and is complete
if (Test-Path -Path $destinationFolder) {
    Write-Host "Existing drivers.gpu.control-library folder found. Validating completeness..."
    if (Test-IGCLSDKCompleteness -IGCLPath $destinationFolder) {
        Write-Host "Existing IGCL SDK is complete. Skipping download." -ForegroundColor Green
        
        Write-Host "Project pre-build tasks completed successfully." -ForegroundColor Green
        exit 0
    } else {
        Write-Host "Existing IGCL SDK is incomplete. Re-downloading..." -ForegroundColor Yellow
        try {
            Remove-Item -Path $destinationFolder -Recurse -Force -ErrorAction Stop
            Write-Host "Removed incomplete IGCL folder." -ForegroundColor Green
        } catch {
            Write-Host "ERROR: Failed to remove existing IGCL folder: $_" -ForegroundColor Red
            exit 1
        }
    }
}

# Download the zip file
Write-Host "Downloading the latest version of IGCL... (may take a while)"
try {
    # Add progress tracking for large downloads
    $ProgressPreference = 'Continue'
    Invoke-WebRequest -Uri $zipUrl -OutFile $zipFilePath -ErrorAction Stop
    Write-Host "Download succeeded." -ForegroundColor Green
    
    # Validate downloaded file
    if (-not (Test-Path -Path $zipFilePath)) {
        throw "Downloaded file not found"
    }
    
    $fileSize = (Get-Item $zipFilePath).Length
    if ($fileSize -lt 100KB) {
        throw "Downloaded file appears to be too small ($fileSize bytes)"
    }
    
    Write-Host "Downloaded file validated ($([math]::Round($fileSize/100KB, 2)) KB)." -ForegroundColor Green
    
} catch {
    Write-Host "ERROR: Failed to download IGCL SDK: $_" -ForegroundColor Red
    # Clean up partial download
    if (Test-Path -Path $zipFilePath) {
        Remove-Item -Path $zipFilePath -Force
    }
    exit 1
}

# Unzip the downloaded file into a temporary folder
Write-Host "Extracting the contents of the zip file... (may take a while)"
try {
    Expand-Archive -Path $zipFilePath -DestinationPath . -Force -ErrorAction Stop
    Write-Host "Extraction completed successfully." -ForegroundColor Green
} catch {
    Write-Host "ERROR: Failed to extract IGCL SDK: $_" -ForegroundColor Red
    # Clean up
    if (Test-Path -Path $zipFilePath) {
        Remove-Item -Path $zipFilePath -Force
    }
    exit 1
}

# Validate extracted folder exists
if (-not (Test-Path -Path $tempExtractFolder)) {
    Write-Host "ERROR: Extracted folder '$tempExtractFolder' not found." -ForegroundColor Red
    # Clean up
    if (Test-Path -Path $zipFilePath) {
        Remove-Item -Path $zipFilePath -Force
    }
    exit 1
}

# Rename the drivers.gpu.control-library-main folder to drivers.gpu.control-library
Write-Host "Renaming drivers.gpu.control-library-main to drivers.gpu.control-library..."
try {
    Rename-Item -Path $tempExtractFolder -NewName $destinationFolder -ErrorAction Stop
    Write-Host "Folder renamed successfully." -ForegroundColor Green
} catch {
    Write-Host "ERROR: Failed to rename IGCL folder: $_" -ForegroundColor Red
    # Clean up
    if (Test-Path -Path $zipFilePath) {
        Remove-Item -Path $zipFilePath -Force
    }
    if (Test-Path -Path $tempExtractFolder) {
        Remove-Item -Path $tempExtractFolder -Recurse -Force
    }
    exit 1
}

# Validate IGCL SDK completeness
Write-Host "Validating IGCL SDK completeness..."
if (-not (Test-IGCLSDKCompleteness -IGCLPath $destinationFolder)) {
    Write-Host "ERROR: Downloaded IGCL SDK is incomplete." -ForegroundColor Red
    # Clean up
    if (Test-Path -Path $zipFilePath) {
        Remove-Item -Path $zipFilePath -Force
    }
    if (Test-Path -Path $destinationFolder) {
        Remove-Item -Path $destinationFolder -Recurse -Force
    }
    exit 1
}

# Remove the zip file after successful extraction and validation
Write-Host "Cleaning up download files..."
try {
    Remove-Item -Path $zipFilePath -Force -ErrorAction Stop
    Write-Host "Cleanup completed." -ForegroundColor Green
} catch {
    Write-Host "WARNING: Failed to remove zip file: $_" -ForegroundColor Yellow
    # This is not critical, continue
}

Write-Host ""
Write-Host "=== IGCL SDK Setup Complete ===" -ForegroundColor Green
Write-Host ""
Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  - IGCL SDK location: $destinationFolder" -ForegroundColor Cyan
Write-Host ""


if ($net10Installed) {
    Write-Host "Next steps:" -ForegroundColor Green
    Write-Host "  - Build the wrapper: .\build_adlx.ps1" -ForegroundColor Gray
    Write-Host "  - Or open IGCLWrapper.sln in Visual Studio" -ForegroundColor Gray
    Write-Host "  - Or use: dotnet build" -ForegroundColor Gray
} else {
    Write-Host "Before building:" -ForegroundColor Yellow
    Write-Host "  1. Install .NET 10.0 SDK from: https://dotnet.microsoft.com/download/dotnet/10.0" -ForegroundColor Cyan
    Write-Host "     Or via Visual Studio Installer (see warning above)" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Then build:" -ForegroundColor Green
    Write-Host "  - .\build_igcl.ps1" -ForegroundColor Gray
    Write-Host "  - Or open IGCLWrapper.sln in Visual Studio" -ForegroundColor Gray
}
Write-Host ""

