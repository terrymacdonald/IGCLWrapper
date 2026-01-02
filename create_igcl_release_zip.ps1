# Create a release zip for IGCLWrapper
param(
    [string]$Configuration = "Release",
    [switch]$IncludeSources
)

$scriptRoot = $PSScriptRoot
if (-not $scriptRoot) {
    $scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
}

Set-Location $scriptRoot

$IGCLHeader = Join-Path $scriptRoot "drivers.gpu.control-library/include/igcl_api.h"
if (-not (Test-Path $IGCLHeader)) {
    Write-Host "IGCL SDK headers not found. Run ./prepare_IGCL.ps1 first." -ForegroundColor Red
    exit 1
}

# ---------------------------------------------------------------------------
# Versioning (match build_IGCL.ps1: MAJOR/MINOR from VERSION, PATCH = git rev-list --count HEAD)
# ---------------------------------------------------------------------------
$versionFile = Join-Path $scriptRoot "VERSION"
$major = 1
$minor = 0
if (Test-Path $versionFile) {
    foreach ($line in Get-Content $versionFile) {
        if ($line -match "^MAJOR=(\d+)") { $major = [int]$matches[1] }
        elseif ($line -match "^MINOR=(\d+)") { $minor = [int]$matches[1] }
    }
}
$patch = 0
try {
    $gitPath = Get-Command git -ErrorAction SilentlyContinue
    if ($gitPath) {
        $commitCount = & git rev-list --count HEAD 2>$null
        if ($LASTEXITCODE -eq 0 -and $commitCount -match "^\d+$") {
            $patch = [int]$commitCount
        }
    }
} catch {}
$version = "$major.$minor.$patch"

$projectPath = Join-Path $scriptRoot "IGCLWrapper/IGCLWrapper.csproj"
Write-Host "Building IGCLWrapper ($Configuration) version $version..." -ForegroundColor Cyan
dotnet build $projectPath -c $Configuration /p:Version=$version /p:AssemblyVersion=$version /p:FileVersion=$version
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed; aborting packaging." -ForegroundColor Red
    exit $LASTEXITCODE
}

$buildOutput = Join-Path $scriptRoot "IGCLWrapper/bin/$Configuration/net10.0"
$assemblyPath = Join-Path $buildOutput "IGCLWrapper.dll"
if (-not (Test-Path $assemblyPath)) {
    Write-Host "Build output not found at $assemblyPath" -ForegroundColor Red
    exit 1
}

$artifactsDir = Join-Path $scriptRoot "release-zip"
New-Item -ItemType Directory -Force -Path $artifactsDir | Out-Null
$zipPath = Join-Path $artifactsDir "IGCLwrapper-$version-$Configuration.zip"
if (Test-Path $zipPath) { Remove-Item $zipPath }

$pathsToPack = @()
$pathsToPack += $assemblyPath
$pathsToPack += Join-Path $buildOutput "IGCLWrapper.pdb"
$pathsToPack += Join-Path $buildOutput "IGCLWrapper.deps.json"
$pathsToPack += Join-Path $buildOutput "IGCLWrapper.xml"
$pathsToPack += Join-Path $scriptRoot "LICENSE"
$pathsToPack += Join-Path $scriptRoot "README.md"
$pathsToPack += Join-Path $scriptRoot "IGCLWrapper/README.md"
$pathsToPack = $pathsToPack | Where-Object { Test-Path $_ }

if ($IncludeSources) {
    $pathsToPack += (Join-Path $scriptRoot "IGCLWrapper/cs_generated")
    $sourceFiles = Get-ChildItem -Path (Join-Path $scriptRoot "IGCLWrapper") -Filter *.cs -File
    $pathsToPack += $sourceFiles.FullName
    $pathsToPack += (Join-Path $scriptRoot "IGCLWrapper/IGCLWrapper.csproj")
}

$pathsToPack = $pathsToPack | Select-Object -Unique

Write-Host "Creating $zipPath..." -ForegroundColor Cyan
Compress-Archive -Path $pathsToPack -DestinationPath $zipPath -Force
Write-Host "Release zip created at $zipPath" -ForegroundColor Green
