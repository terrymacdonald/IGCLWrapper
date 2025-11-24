# Enhanced IGCL Preparation Script with Improved Error Handling

# Define IGCL-related variables
$zipUrl = "https://github.com/intel/drivers.gpu.control-library/archive/refs/heads/master.zip"
$zipFilePath = ".\master.zip"
$destinationFolder = ".\drivers.gpu.control-library"
$tempExtractFolder = ".\drivers.gpu.control-library-master"

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

Write-Host "=== Enhanced IGCL Preparation Script ===" -ForegroundColor Cyan
Write-Host "Preparing SWIG and IGCL SDK ..." -ForegroundColor Cyan

# Check internet connectivity
Write-Host "Checking internet connectivity..."
if (-not (Test-InternetConnection)) {
    Write-Host "ERROR: No internet connection available. Cannot download dependencies." -ForegroundColor Red
    exit 1
}
Write-Host "Internet connectivity confirmed." -ForegroundColor Green

# === IGCL PROCESSING ===
Write-Host ""
Write-Host "=== IGCL SDK Dependency Check ===" -ForegroundColor Cyan

# Check if IGCL folder already exists and is complete
if (Test-Path -Path $destinationFolder) {
    Write-Host "Existing drivers.gpu.control-library folder found. Validating completeness..."
    if (Test-IGCLSDKCompleteness -IGCLPath $destinationFolder) {
        Write-Host "Existing IGCL SDK is complete. Skipping download." -ForegroundColor Green
        
        # Still create the out folder if it doesn't exist
        if (-not (Test-Path -Path $outFolder)) {
            Write-Host "Creating the out folder..."
            New-Item -ItemType Directory -Path $outFolder | Out-Null
        }
        
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

Write-Host "=== Project pre-build tasks completed successfully ===" -ForegroundColor Green
Write-Host "The IGCL SDK is ready for compilation." -ForegroundColor Green
