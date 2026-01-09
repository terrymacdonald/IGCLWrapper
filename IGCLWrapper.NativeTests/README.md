# IGCLWrapper.NativeTests

Integration tests for IGCLWrapper. Tests require:
- Intel GPU with IGCL support
- IGCL DLLs available (run `./prepare_igcl.ps1` first)
- Windows 10/11 x64 and .NET 10.0 SDK

## Running tests
```powershell
./test_igcl.ps1
# or
cd IGCLWrapper.NativeTests
dotnet test IGCLWrapper.NativeTests.csproj
```

Tests skip gracefully when hardware or DLLs are missing; skipped tests are reported in output.

## Notes
- Tests instantiate `IGCLApi` via `IGCLApi.Initialize()` and dispose via `IDisposable` to ensure native handles are closed.
- Some tests enumerate hardware features; availability depends on your GPU/driver.
