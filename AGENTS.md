# AGENTS Guide for IGCLWrapper

This file captures the essential rules and context for agents working on this IGCLWrapper repository. 

## Project Scope
- Purpose: Safe C# wrapper over Intel IGCL for Windows x64 targeting .NET 10.0 and higher.
- Structure: 
    - Root `IGCLWrapper/` project (helpers + `cs_generated/` bindings),
    - `IGCLWrapper.NativeTests/` xUnit native test suite,
    - `IGCLWrapper.FacadeTests/` xUnit facade test suite,
    - `Samples/`, scripts for prepare/build/test.
- Two API Levels: There are two levels of IGCLWrapper API available for users to use. 
    - Native: This level is low level and use the C# equivalent functions in IGCLWrapper that were created by ClangSharpPInvokeGenerator, and are found in `../IGCLWrapper/cs_generated`. These are the functions that are the C# equvalent of the C++ Intel IGCL SDK described in the ../drivers.gpu.control-library/include/igcl_api.h, so please look there if you need to know what they are.
    - Facade: This level uses helper functions to abstract away any memory management, and to make it very easy and simple to access the information provided by the IGCL SDK.

## Core Development Rules
- ALWAYS MAKE SURE THAT YOU TELL THE USER YOUR PLAN BEFORE YOU MAKE ANY CHANGES TO FILES AND GIVE THE USER A CHANCE TO REVIEW. ONLY MAKE CHANGS ONCE THE USER HAS GIVEN THEIR APPROVAL. THe user can tell you to perform multiple steps of a plan if you want to.
- When PLANNING, if you think you will get confused and lose track of where you are in your plan, then please write it down into a PLAN.md document. Keep the PLAN.md updated as you go, and make sure that the information you store in the PLAN.md is very descriptive and detailed, so that if you lose track in the future you can review the PLAN.md and you will know what to do and will do it well. Do not be overly concise as you lose a lot of nuance that will be important.
- DO NOT MAKE THINGS UP. Always check the Intel IGCL SDK header files in `../drivers.gpu.control-library/include/igcl_api.h`, or the Intel IGCL Samples in `../drivers.gpu.control-library/Samples`, or the IGCLWrapper code in `IGCLWrapper` if you need more information. If you are unsure then tell the user. The user wants you to only use facts - not conjecture. Tune your temperature to the lowest you can. 
- Write code that tries to be robust and cope with problems getting the information requested, but without causing an exception or a crash. 
- Naming/patterns: Preserve established helper naming (`IGCL<Feature>Helper`). Replicate existing helper/test patterns for new features. Create function names without the 'ctl' at the start of them e.g. 
   CTL_APIEXPORT ctl_result_t CTL_APICALL ctlCheckDriverVersion(ctl_device_adapter_handle_t hDeviceAdapter, ctl_version_info_t version_info)
becomes 
   public ICGL_VERSION_INFO CheckDriverVersion(ctl_device_adapter_handle_t hDeviceAdapter) 
when this appears in the IGCLDisplayHelper

 Consistently of API is key. The user has spent a long time trying to keep everything standard and consistent, so make sure new creations align with existing patterns. Ask for permission for anything that does not align.
- The following Features are present in the IGCL right now - please use these with the naming structure listed above:
    - 3d
	- Display
	- Ecc
	- Engine
	- Fan
	- Firmware
	- Frequency
	- Led
	- Media
	- Memory
	- Overclock
	- Pci
	- Power
	- Temperature
- Platform: Windows-only, x64; relies on Intel GPU drivers. Lightweight check is `IsIGCLDllAvailable`.
- Any initialisation code generated needs to avoid it or handle it when getting an CTL_RESULT_ERROR_ALREADY_INITIALIZED exception when trying to initialise IGCL a second time, and avoid or handle CTL_RESULT_ERROR_UNSUPPORTED_VERSION or CTL_RESULT_ERROR_UNSUPPORTED_FEATURE exceptions on optional functions.

## Core Native-specific Development Rules
- Follow the usage patterns shown in the Intel IGCL SDK Samples as closely as possible to ensure that the C# Native functions will work. The IGCL SDK Samples can be found in IGCL/Samples. Please also look at the IGCL/Include folder and the IGCL/SDKDoc folder for more information about how the IGCL SDK works. 
- Generated code: Do not hand-edit `cs_generated/`. Changes come from headers/config used by `ClangSharpPInvokeGenerator` (`GenerateBindings` target in `IGCLWrapper.csproj`).
- The Native level functions should always be developed and tested first. Those low level functions will be used by Facade level functions, so its important that we make sure that they Native functinos work before moving up to the higher-level Facade functions.
- Initialization (Native): Use `using var IGCL = IGCLApi.Initialize();` by default.
- Disposal (Native): Dispose any child pointers before disposing `IGCL`. Any call after disposal should throw `ObjectDisposedException`.
- Native struct factory pattern: Every native struct that requires `Size` and `Version` fields has a static `Create*()` factory method on the helper class (e.g. `CreateDisplayProperties()`, `CreateSharpnessCaps()`). Always use these instead of inline struct initialization. If a factory method doesn't exist for a struct you need, add one. All `Create*()` methods are `unsafe` because they use `sizeof()`. Example: `private static unsafe ctl_foo_t CreateFoo() => new ctl_foo_t { Size = (uint)sizeof(ctl_foo_t), Version = 0 };`
- `unsafe` pointer rule: In an `unsafe` method, use `&local` directly to pass a pointer to a local stack variable to IGCL functions. Do NOT use `fixed (T* p = &local)` — this causes CS0213 ("cannot use the fixed statement to take the address of an already fixed expression") because local stack variables are already fixed. Example: `var result = IGCL.ctlFoo(handle, &myLocalStruct);`


## Core Facade-Specific Development Rules
- Facade level objects should handle all underlying Native level memory management themselves. The user should not need to worry about it. This includes memory creation, disposal when objects are deleted, and handling functions being called multiple times in threads. Our aim is to never have memory leaks when using Facades.
- The Facade functions should (in general) return Helper objects that represent the relevant objects within the underlying IGCL SDK, for example IGCLDesktop. Each returned Facade object should have properties that store the information contained within the underlying Native objects e.g. NativeResolutionWidth, and Access to any underlying functions that are offered by the Native objects e.g. IGCLAdapterHelper providing access to EnumerateDisplayOutputs() function that returns a list of Displays outputs currently known to Windows.
- Facade helper methods should return DTOs with `bool` properties where native structs use `byte` for `bool`. Do NOT add public `*Native()` methods to helper classes — the public API exposes only DTO-returning methods.
- **Nullable return convention**: All public Facade getter methods return `T?` (nullable struct) rather than throwing when the underlying IGCL call returns an unsupported-feature result code. Setter methods return `bool` — `true` on success, `false` when unsupported. The four result codes treated as "unsupported" (mapped to `null`/`false`) are: `CTL_RESULT_ERROR_UNSUPPORTED_FEATURE`, `CTL_RESULT_ERROR_UNSUPPORTED_VERSION`, `CTL_RESULT_ERROR_INVALID_OPERATION_TYPE`, and `CTL_RESULT_ERROR_INVALID_ARGUMENT`. Any other non-success result still throws `IGCLException`. Each helper class contains a `private static bool IsUnsupportedResult(ctl_result_t result)` helper that performs this check. When writing tests, use `Skip.If(!result.HasValue, ...)` or `result?.Value.X` rather than assuming the result is non-null.
- DTOs provide `static FromNative(nativeStruct)` and `ToNative()` instance methods for round-tripping with native structs when needed internally.
- Exception: `IGCLDisplayHelper` exposes three public `*Native()` methods (`PixelTransformationGetConfigNative` and `PixelTransformationSetConfigNative`) because these require setting raw pointer fields (`pBlockConfigs`) that cannot be expressed through DTOs. No other helper class should have public `*Native()` methods.
- For native bitmask/flags fields stored in DTOs (e.g. `*Flags`, `Supported*`, `Enabled*`), keep the raw numeric field for roundtrip fidelity but also expose per-flag boolean convenience properties on the DTO so callers can use the data ergonomically without manual bitmask operations. Verify flag mappings against `drivers.gpu.control-library/include/igcl_api.h` and generated enums.
- Split combined operations into `Get*()` and `Set*()` helpers. `GetSet*Native()` methods exist only as **private** implementation helpers inside helper classes; they are not part of the public API.
- Initialization (Facade): Use `using var IGCL = IGCLApiHelper.Initialize();` as the standard entry point.
- Disposal (Facade): Dispose facade system services and returned facade objects before disposing `IGCL`. `IGCLApiHelper` disposal should result in `ObjectDisposedException` on use-after-dispose.
- Each helper class has a private `ThrowIfDisposed()` method that must be called at the top of every public method.

## Build and Scripts
- Prepare: `./prepare_igcl.ps1` (downloads/validates IGCL SDK headers into `IGCL/`).
- Build: `./build_igcl.ps1` (restores, cleans, builds solution; version from `VERSION` + git commit count). Direct build: `dotnet build IGCLWrapper/IGCLWrapper.csproj`.
- Test: `./test_igcl.ps1` (runs both native and facade test suites).
- Release ZIP: `./create_igcl_release_zip.ps1` (produces artifacts/IGCLWrapper-<version>-Release.zip).

## Testing Expectations
- Suites: xUnit in `IGCLWrapper.NativeTests` (Native) and `IGCLWrapper.FacadeTests` (Facade) targeting `net10.0`; hardware-aware and read-only (no tuning changes). Global xUnit parallelization is disabled.
- Test one feature per individual test case, as we want to keep good visibility for the user as to which test fails.
- Run (Native first): `dotnet test IGCLWrapper.NativeTests/IGCLWrapper.NativeTests.csproj --verbosity normal` (or from tests folder), or `./test_IGCL.ps1`. Then run facades with `dotnet test IGCLWrapper.FacadeTests/IGCLWrapper.FacadeTests.csproj --verbosity normal`.
- Both Native and Facade tests should test the full range of the Intel IGCL API. 
- Test Filenames: Native tests are currently organized in consolidated domain files: `DisplayServicesTests.cs`, `GpuServicesTests.cs`, `SystemServicesTests.cs`, `CoreApiTests.cs`, `BasicApiTests.cs`. New native tests for a feature should go in the most relevant existing file. Facade tests use `IGCL<Feature>HelperTests.cs` (e.g. `IGCLDisplayHelperTests.cs`, `IGCLFrequencyHelperTests.cs`). This keeps files small and feature-aligned.
- Native vs Facade tests:
  - Native (`DisplayServicesTests.cs` etc.): Use only ClangSharp-generated APIs in `IGCLWrapper/cs_generated`; never call facades as they will be tested in the Facade tests. The Native tests should be able to run and pass successfully even if all the IGCLWrapper Facade functions were removed.
  - Facade (`IGCL<Feature>HelperTests.cs`): Exercise helper/facade ergonomics built on native APIs. Use `FacadeTestUtils.RequireAdapter()` to bootstrap and `FacadeTestUtils.InvokeOrSkip()` to wrap optional-feature calls — it automatically converts `CTL_RESULT_ERROR_UNSUPPORTED_FEATURE`, `CTL_RESULT_ERROR_UNSUPPORTED_VERSION`, and `EntryPointNotFoundException` into `SkipException`.
  - Active tests (`IGCL<Feature>HelperActiveTests.cs`): Tests that write/modify hardware state (apply-and-revert) live in a separate `*ActiveTests.cs` file. Tag each test `[Trait("Category", "Active")]` and place it in `[Collection("ActiveDisplay")]` or `[Collection("ActiveCombined")]` (defined in `TestCollectionOrdering.cs`). Use the `RunActiveDisplayTest` / `ApplyAndRevert` helpers to ensure settings are always reverted even on failure.
- Test creation: Write Native tests first to validate low-level APIs, then Facade tests. If IGCL marks features optional or provides `IsSupported`, gate tests accordingly; skip only when unsupported. Fix underlying wrapper bugs rather than skipping failing coverage. Shared fixtures for bootstrapping IGCL are acceptable; keep initialization/disposal safe across tests.
- Hardware skip: Tests that need Intel GPU/driver or IGCL DLL gracefully skip when missing. Use `IGCLApiHelper.IsIGCLDllAvailable()` and `IGCLHardwareDetection.HasIntelGPU()` for detection (already encapsulated in `FacadeTestUtils.RequireAdapter()`).

## Usage Notes
- DLL loading: `IGCLApi` dynamically loads `ControlLib.dll` (x64) or `ControlLib32.dll` (x86) via `LoadLibraryEx`; keep `IGCLNative.GetDllName()` and `IGCLApi.LoadIGCLDll()` in sync if names/paths change; surface errors via `IGCLException`.
- Data shapes: Helpers expose DTO classes (e.g. `DisplayPropertiesDto`, `FrequencyPropertiesDto`) with `bool` convenience properties, per-flag booleans for bitmask fields, and `FromNative()`/`ToNative()` round-trip methods.
- Samples: See `IGCLWrapper/README.md` and `Samples/` for usage patterns (enumeration, capability checks, event listeners).

## Versioning
- Version scheme: `VERSION` provides MAJOR.MINOR; PATCH computed from git commit count via `SetVersionFromGit` and `build_IGCL.ps1`. Update `VERSION` when bumping MAJOR/MINOR.

## Expectations for Agents
- Keep APIs and helpers consistent with existing conventions; avoid breaking established patterns. Consistentcy is key across the whole codebase. Do not deviate from this consistentcy without first requesting permission from the user. 
- Respect disposal and pointer ownership rules; ensure safe lifetime handling.
- Prefer generated enums over custom ones to align with IGCL updates.
- Maintain optional-feature gating and hardware skip behavior in tests and helpers.
