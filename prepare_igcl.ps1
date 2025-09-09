# Enhanced IGCL Preparation Script with Improved Error Handling and SWIG Integration

# Define SWIG-related variables
$swigZipUrl = "https://phoenixnap.dl.sourceforge.net/project/swig/swigwin/swigwin-4.3.1/swigwin-4.3.1.zip?viasf=1"
$swigZipFilePath = ".\swigwin.zip"
$swigDestinationFolder = ".\swigwin"
$swigTempExtractFolder = ".\swigwin-4.3.1"
$swigExecutablePath = ".\swigwin\swig.exe"

# Define IGCL-related variables
$zipUrl = "https://github.com/intel/drivers.gpu.control-library/archive/refs/heads/master.zip"
$zipFilePath = ".\master.zip"
$destinationFolder = ".\drivers.gpu.control-library"
$tempExtractFolder = ".\drivers.gpu.control-library-master"
$outFolder = ".\IGCLWrapper\cs_bindings"

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

# Function to validate SWIG installation
function Test-SwigInstallation {
    param([string]$swigPath)
    
    # Check if SWIG executable exists
    if (-not (Test-Path -Path $swigPath)) {
        Write-Host "SWIG executable not found at: $swigPath" -ForegroundColor Yellow
        return $false
    }
    
    # Test SWIG functionality by checking version
    try {
        $swigVersion = & $swigPath -version 2>&1
        if ($LASTEXITCODE -eq 0 -and $swigVersion -match "SWIG Version") {
            Write-Host "SWIG validation passed - executable is functional." -ForegroundColor Green
            return $true
        } else {
            Write-Host "SWIG executable exists but is not functional." -ForegroundColor Yellow
            return $false
        }
    } catch {
        Write-Host "Failed to execute SWIG: $_" -ForegroundColor Yellow
        return $false
    }
}

# Function to install SWIG for Windows
function Install-SwigWindows {
    Write-Host "Downloading SWIG v4.3.1 for Windows... (may take a while)"
    
    try {
        # Add progress tracking for downloads and handle redirects
        $ProgressPreference = 'Continue'
        Invoke-WebRequest -Uri $swigZipUrl -OutFile $swigZipFilePath -MaximumRedirection 10 -ErrorAction Stop
        Write-Host "SWIG download succeeded." -ForegroundColor Green
        
        # Validate downloaded file
        if (-not (Test-Path -Path $swigZipFilePath)) {
            throw "Downloaded SWIG file not found"
        }
        
        $fileSize = (Get-Item $swigZipFilePath).Length
        # SWIG 4.3.1 zip file should be around 10-15MB, so anything less than 5MB is likely an error page
        if ($fileSize -lt 5MB) {
            throw "Downloaded SWIG file appears to be too small ($fileSize bytes) - likely a redirect page or error"
        }
        
        Write-Host "Downloaded SWIG file validated ($([math]::Round($fileSize/1MB, 2)) MB)." -ForegroundColor Green
        
    } catch {
        Write-Host "ERROR: Failed to download SWIG: $_" -ForegroundColor Red
        # Clean up partial download
        if (Test-Path -Path $swigZipFilePath) {
            Remove-Item -Path $swigZipFilePath -Force
        }
        return $false
    }
    
    # Remove existing SWIG folder if it exists
    if (Test-Path -Path $swigDestinationFolder) {
        Write-Host "Removing existing SWIG folder..."
        try {
            Remove-Item -Path $swigDestinationFolder -Recurse -Force -ErrorAction Stop
            Write-Host "Removed existing SWIG folder." -ForegroundColor Green
        } catch {
            Write-Host "ERROR: Failed to remove existing SWIG folder: $_" -ForegroundColor Red
            # Clean up
            if (Test-Path -Path $swigZipFilePath) {
                Remove-Item -Path $swigZipFilePath -Force
            }
            return $false
        }
    }
    
    # Extract SWIG
    Write-Host "Extracting SWIG contents... (may take a while)"
    try {
        Expand-Archive -Path $swigZipFilePath -DestinationPath . -Force -ErrorAction Stop
        Write-Host "SWIG extraction completed successfully." -ForegroundColor Green
    } catch {
        Write-Host "ERROR: Failed to extract SWIG: $_" -ForegroundColor Red
        # Clean up
        if (Test-Path -Path $swigZipFilePath) {
            Remove-Item -Path $swigZipFilePath -Force
        }
        return $false
    }
    
    # Validate extracted folder exists
    if (-not (Test-Path -Path $swigTempExtractFolder)) {
        Write-Host "ERROR: Extracted SWIG folder '$swigTempExtractFolder' not found." -ForegroundColor Red
        # Clean up
        if (Test-Path -Path $swigZipFilePath) {
            Remove-Item -Path $swigZipFilePath -Force
        }
        return $false
    }
    
    # Rename the extracted folder
    Write-Host "Renaming SWIG folder..."
    try {
        Rename-Item -Path $swigTempExtractFolder -NewName $swigDestinationFolder -ErrorAction Stop
        Write-Host "SWIG folder renamed successfully." -ForegroundColor Green
    } catch {
        Write-Host "ERROR: Failed to rename SWIG folder: $_" -ForegroundColor Red
        # Clean up
        if (Test-Path -Path $swigZipFilePath) {
            Remove-Item -Path $swigZipFilePath -Force
        }
        if (Test-Path -Path $swigTempExtractFolder) {
            Remove-Item -Path $swigTempExtractFolder -Recurse -Force
        }
        return $false
    }
    
    # Clean up zip file
    Write-Host "Cleaning up SWIG download files..."
    try {
        Remove-Item -Path $swigZipFilePath -Force -ErrorAction Stop
        Write-Host "SWIG cleanup completed." -ForegroundColor Green
    } catch {
        Write-Host "WARNING: Failed to remove SWIG zip file: $_" -ForegroundColor Yellow
        # This is not critical, continue
    }
    
    # Validate SWIG installation
    if (Test-SwigInstallation -swigPath $swigExecutablePath) {
        Write-Host "SWIG installation completed successfully." -ForegroundColor Green
        return $true
    } else {
        Write-Host "ERROR: SWIG installation validation failed." -ForegroundColor Red
        return $false
    }
}

Write-Host "=== Enhanced IGCL Preparation Script with SWIG Integration ===" -ForegroundColor Cyan
Write-Host "Preparing SWIG and IGCL SDK ..." -ForegroundColor Cyan

# Check internet connectivity
Write-Host "Checking internet connectivity..."
if (-not (Test-InternetConnection)) {
    Write-Host "ERROR: No internet connection available. Cannot download dependencies." -ForegroundColor Red
    exit 1
}
Write-Host "Internet connectivity confirmed." -ForegroundColor Green

# === SWIG PROCESSING ===
Write-Host ""
Write-Host "=== SWIG Dependency Check ===" -ForegroundColor Cyan

# Check if SWIG is already installed and functional
if (Test-SwigInstallation -swigPath $swigExecutablePath) {
    Write-Host "Existing SWIG installation is valid. Skipping download." -ForegroundColor Green
} else {
    Write-Host "SWIG not found or not functional. Installing SWIG..." -ForegroundColor Yellow
    
    if (-not (Install-SwigWindows)) {
        Write-Host "ERROR: Failed to install SWIG. Cannot proceed with build." -ForegroundColor Red
        exit 1
    }
}

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

# Create the out folder if it doesn't exist
if (-not (Test-Path -Path $outFolder)) {
    Write-Host "Creating the output folder (cs_bindings)..."
    try {
        New-Item -ItemType Directory -Path $outFolder -ErrorAction Stop | Out-Null
        Write-Host "Output folder cs_bindings created successfully." -ForegroundColor Green
    } catch {
        Write-Host "ERROR: Failed to create output folder: $_" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "Output folder already exists." -ForegroundColor Green
}

Write-Host "=== Project pre-build tasks completed successfully ===" -ForegroundColor Green
Write-Host "SWIG and IGCL SDK are ready for compilation." -ForegroundColor Green
