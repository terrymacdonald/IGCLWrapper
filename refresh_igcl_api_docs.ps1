#
# IGCLWrapper DocFX Refresh Script (PowerShell)
# Regenerates API documentation and launches the DocFX dev server on port 8000
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
Write-Host "IGCLWrapper API Docs Refresh (DocFX)" -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

# Paths
$docfxConfig = Join-Path $scriptRoot "APIDocs\\docfx.json"
$docfxSite   = Join-Path $scriptRoot "APIDocs\\_site"
$docfxPort   = 8080

# Validate DocFX config
if (-not (Test-Path $docfxConfig)) {
    Write-Host "ERROR: DocFX config not found at $docfxConfig" -ForegroundColor Red
    Write-Host "Please ensure APIDocs/docfx.json exists." -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}

# Locate or install DocFX global tool
Write-Host "Checking for DocFX CLI..." -ForegroundColor Yellow

$docfxEntry = dotnet tool list --global 2>&1 | Where-Object { $_ -match '^docfx\s' }
if (-not $docfxEntry) {
    Write-Host "DocFX not installed. Installing globally..." -ForegroundColor Yellow
    dotnet tool install --global docfx
    if ($LASTEXITCODE -ne 0) {
        Write-Host ""
        Write-Host "ERROR: Failed to install DocFX. Check your internet connection and .NET SDK version." -ForegroundColor Red
        Write-Host ""
        Read-Host "Press Enter to exit"
        exit 1
    }
    Write-Host "DocFX installed successfully." -ForegroundColor Green
    Write-Host ""
    $docfxEntry = dotnet tool list --global 2>&1 | Where-Object { $_ -match '^docfx\s' }
}

# Parse installed version and locate the DLL (invoking via 'dotnet exec' bypasses AppLocker)
$docfxVersion = ($docfxEntry -split '\s+')[1]
$docfxDll = (Get-ChildItem "$env:USERPROFILE\.dotnet\tools\.store\docfx\$docfxVersion\docfx\$docfxVersion\tools" -Recurse -Filter "docfx.dll" -ErrorAction SilentlyContinue | Select-Object -First 1).FullName
if (-not $docfxDll) {
    Write-Host ""
    Write-Host "ERROR: Could not locate docfx.dll under the global tool store." -ForegroundColor Red
    Write-Host "Expected: $env:USERPROFILE\.dotnet\tools\.store\docfx\$docfxVersion" -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "DocFX $docfxVersion found at: $docfxDll" -ForegroundColor Green
Write-Host ""

# Run DocFX metadata + build
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "Generating metadata..." -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

Push-Location (Split-Path $docfxConfig -Parent)
try {
    dotnet exec $docfxDll metadata $docfxConfig
    if ($LASTEXITCODE -ne 0) { throw "DocFX metadata failed with exit code $LASTEXITCODE" }

    Write-Host ""
    Write-Host "Metadata generated successfully." -ForegroundColor Green
    Write-Host ""

    Write-Host "============================================================================" -ForegroundColor Cyan
    Write-Host "Building site..." -ForegroundColor Cyan
    Write-Host "============================================================================" -ForegroundColor Cyan
    Write-Host ""

    dotnet exec $docfxDll build $docfxConfig
    if ($LASTEXITCODE -ne 0) { throw "DocFX build failed with exit code $LASTEXITCODE" }

    Write-Host ""
    Write-Host "DocFX site built successfully at: $docfxSite" -ForegroundColor Green
    Write-Host ""
} catch {
    Pop-Location
    Write-Host ""
    Write-Host "ERROR: DocFX generation failed." -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}
Pop-Location

# Serve the site
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "Starting DocFX server on http://localhost:$docfxPort/" -ForegroundColor Cyan
Write-Host "Press Ctrl+C to stop the server." -ForegroundColor Yellow
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

dotnet exec $docfxDll serve $docfxSite -p $docfxPort --hostname localhost
