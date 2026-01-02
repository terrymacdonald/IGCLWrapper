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

# Locate DocFX CLI
Write-Host "Checking for DocFX CLI..." -ForegroundColor Yellow
$docfxCmd = Get-Command docfx -ErrorAction SilentlyContinue
if (-not $docfxCmd) {
    Write-Host ""
    Write-Host "ERROR: DocFX CLI not found in PATH." -ForegroundColor Red
    Write-Host "Install DocFX (e.g., 'choco install docfx' or 'dotnet tool install -g docfx')." -ForegroundColor Yellow
    Write-Host "After installation, ensure 'docfx' is available in your PATH." -ForegroundColor Yellow
    Write-Host ""
    Read-Host "Press Enter to exit"
    exit 1
}

try {
    $docfxVersion = & $docfxCmd.Source --version 2>&1
    Write-Host "DocFX found: $($docfxCmd.Source)" -ForegroundColor Green
    if ($docfxVersion) {
        Write-Host "DocFX version: $docfxVersion" -ForegroundColor Green
    }
    Write-Host ""
} catch {
    Write-Host "Warning: Unable to read DocFX version: $_" -ForegroundColor Yellow
    Write-Host ""
}

# Run DocFX metadata + build
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "Generating metadata..." -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

Push-Location (Split-Path $docfxConfig -Parent)
try {
    & $docfxCmd.Source metadata $docfxConfig
    if ($LASTEXITCODE -ne 0) { throw "DocFX metadata failed with exit code $LASTEXITCODE" }

    Write-Host ""
    Write-Host "Metadata generated successfully." -ForegroundColor Green
    Write-Host ""

    Write-Host "============================================================================" -ForegroundColor Cyan
    Write-Host "Building site..." -ForegroundColor Cyan
    Write-Host "============================================================================" -ForegroundColor Cyan
    Write-Host ""

    & $docfxCmd.Source build $docfxConfig
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

& $docfxCmd.Source serve $docfxSite -p $docfxPort --hostname localhost
