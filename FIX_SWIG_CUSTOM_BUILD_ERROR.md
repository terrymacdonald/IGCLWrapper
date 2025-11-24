# Script to Remove SWIG Build Configuration

## What This Does
This removes any remaining SWIG custom build configuration that might be causing build errors.

## Steps to Fix

### 1. Clean Visual Studio Cache
Close Visual Studio, then delete these folders:
```
IGCLWrapper\.vs\
IGCLWrapper\IGCLWrapper\.vs\
```

### 2. Remove Any Remaining SWIG Project Files
```powershell
# Run from solution root (C:\vs-code\IGCLWrapper\)
Remove-Item "IGCLWrapper\*.vcxproj*" -Force -ErrorAction SilentlyContinue
Remove-Item "IGCLWrapper\*.user" -Force -ErrorAction SilentlyContinue
Remove-Item "IGCLWrapper\obj" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item "IGCLWrapper\bin" -Recurse -Force -ErrorAction SilentlyContinue
```

### 3. Update Solution File
The `IGCLWrapper.sln` file might still reference the old C++ project. 

**Option A: Let Visual Studio fix it automatically**
1. Open `IGCLWrapper.sln` in Visual Studio
2. You'll see a prompt that the project couldn't be loaded
3. Right-click the unloaded project ? Remove
4. Right-click Solution ? Add ? Existing Project
5. Add `IGCLWrapper\IGCLWrapper.csproj` (the new C# project)
6. Save the solution

**Option B: Manual edit**
Open `IGCLWrapper.sln` in a text editor and:

**Remove these lines** (if they exist):
```
Project("{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}") = "IGCLWrapper", "IGCLWrapper\IGCLWrapper.vcxproj", "{...GUID...}"
EndProject
```

**Add this line** (if not present):
```
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "IGCLWrapper", "IGCLWrapper\IGCLWrapper.csproj", "{...NEW-GUID...}"
EndProject
```

### 4. PowerShell Command to Clean Everything
```powershell
# Navigate to solution directory
cd C:\vs-code\IGCLWrapper

# Close Visual Studio first!

# Remove VS cache
Remove-Item ".vs" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item "IGCLWrapper\.vs" -Recurse -Force -ErrorAction SilentlyContinue

# Remove old C++ project remnants
Remove-Item "IGCLWrapper\*.vcxproj" -Force -ErrorAction SilentlyContinue
Remove-Item "IGCLWrapper\*.vcxproj.filters" -Force -ErrorAction SilentlyContinue
Remove-Item "IGCLWrapper\*.vcxproj.user" -Force -ErrorAction SilentlyContinue

# Remove build artifacts
Remove-Item "IGCLWrapper\obj" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item "IGCLWrapper\bin" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item "IGCLWrapper\x64" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item "IGCLWrapper\Debug" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item "IGCLWrapper\Release" -Recurse -Force -ErrorAction SilentlyContinue

Write-Host "Cleanup complete! Now open Visual Studio and rebuild."
```

### 5. Rebuild
```powershell
# After cleanup, rebuild the solution
dotnet clean
dotnet build
```

## If Error Persists

If you're still seeing the error about `IGCLWrapper.i`, check:

1. **Global.json or Directory.Build.props**
   - Make sure there's no global MSBuild configuration importing SWIG targets

2. **Check for .targets files**
   ```powershell
   Get-ChildItem -Recurse -Filter "*.targets" | Select-Object FullName
   ```
   - Look for any custom `.targets` files that might reference SWIG

3. **Nuclear Option - Fresh Clone**
   If nothing works:
   ```powershell
   # Commit your changes
   git add .
   git commit -m "WIP: ClangSharp migration"
   git push
   
   # Clone fresh
   cd C:\temp
   git clone https://github.com/terrymacdonald/IGCLWrapper IGCLWrapper-fresh
   cd IGCLWrapper-fresh
   git checkout feature-converting-to-interop
   
   # Build fresh
   dotnet build
   ```

## What's Expected After Cleanup

Your solution should only have:
- ? `IGCLWrapper.sln` - Solution file (updated to reference .csproj)
- ? `IGCLWrapper\IGCLWrapper.csproj` - C# project
- ? `IGCLWrapper.Tests\IGCLWrapper.Tests.csproj` - Test project
- ? NO `IGCLWrapper.vcxproj` (archived in SWIG_Archive)
- ? NO `IGCLWrapper.i` custom build step

## Verify It's Fixed

```powershell
# This should succeed
dotnet build IGCLWrapper.sln

# This should show only C# projects
dotnet sln list
```

Expected output:
```
Project(s)
----------
IGCLWrapper\IGCLWrapper.csproj
IGCLWrapper.Tests\IGCLWrapper.Tests.csproj
```
