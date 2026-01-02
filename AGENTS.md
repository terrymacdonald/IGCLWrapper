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


## Core Facade-Specific Development Rules
- Facade level objects should handle all underlying Native level memory management themselves. The user should not need to worry about it. This includes memory creation, disposal when objects are deleted, and handling functions being called multiple times in threads. Our aim is to never have memory leaks when using Facades.
- The Facade functions should (in general) return Helper objects that represent the relevant objects within the underlying IGCL SDK, for example IGCLDesktop. Each returned Facade object should have properties that store the information contained within the underlying Native objects e.g. NativeResolutionWidth, and Access to any underlying functions that are offered by the Native objects e.g. IGCLDisplayHelper providing access to EnumerateDisplayOutputs() function that returns a list of Displays outputs currently known to Windows.
- Initialization (Facade): Use `using var IGCL = IGCLApiHelper.Initialize();` as the standard entry point.
- Disposal (Facade): Dispose facade system services and returned facade objects before disposing `IGCL`. `IGCLApiHelper` disposal should result in `ObjectDisposedException` on use-after-dispose.

## Build and Scripts
- Prepare: `./prepare_IGCL.ps1` (downloads/validates IGCL SDK headers into `IGCL/`).
- Build: `./build_IGCL.ps1` (restores, cleans, builds solution; version from `VERSION` + git commit count). Direct build: `dotnet build IGCLWrapper/IGCLWrapper.csproj`.
- Release ZIP: `./create_IGCL_release_zip.ps1` (produces artifacts/IGCLWrapper-<version>-Release.zip).

## Testing Expectations
- Suites: xUnit in `IGCLWrapper.NativeTests` (Native) and `IGCLWrapper.FacadeTests` (Facade) targeting `net10.0`; hardware-aware and read-only (no tuning changes). Global xUnit parallelization is disabled.
- Test one feature per individual test case, as we want to keep good visibility for the user as to which test fails.
- Run (Native first): `dotnet test IGCLWrapper.NativeTests/IGCLWrapper.NativeTests.csproj --verbosity normal` (or from tests folder), or `./test_IGCL.ps1`. Then run facades with `dotnet test IGCLWrapper.FacadeTests/IGCLWrapper.FacadeTests.csproj --verbosity normal`.
- Both Native and Facade tests should test the full range of the Intel IGCL API. 
- Test Filenames should align with each of the feature areas being tested. e.g. 
  - IGCLDisplayNativeTests.cs should be where the Native tests that exercise the Display objects live, and 
  - IGCLDisplayFacadeTests.cs should be where the Facade tests that exercise the Display objects live.
  - This structure will keep the files small to make it easier for LLMs to edit them, and make it easier for humans to find the functions when they need fixing.
- Native vs Facade tests:
  - Native (`*NativeTests.cs`): Use only ClangSharp-generated APIs in `IGCLWrapper/cs_generated`; never call facades as they will be tested in the Facade tests. THe Native tests should be able to run and pass successfully even if all the IGCLWrapper Facade functions were removed.
  - Facade (`*FacadeTests.cs`): Exercise helper/facade ergonomics built on native APIs.
- Test creation: Write Native tests first to validate low-level APIs, then Facade tests. If IGCL marks features optional or provides `IsSupported`, gate tests accordingly; skip only when unsupported. Fix underlying wrapper bugs rather than skipping failing coverage. Shared fixtures for bootstrapping IGCL are acceptable; keep initialization/disposal safe across tests.
- Hardware skip: Tests that need Intel GPU/driver or IGCL DLL gracefully skip when missing.

## Usage Notes
- DLL loading: `IGCLApi` dynamically loads `amdIGCL64.dll` via `LoadLibraryEx`; keep `IGCLNative.GetDllName()` and `IGCLApi.LoadIGCLDll()` in sync if names/paths change; surface errors via `IGCLException`.
- Data shapes: Helpers expose serializable `Info` structs (e.g., `GpuInfo`, `DisplayInfo`) and support apply/restore flows.
- Samples: See `IGCLWrapper/README.md` and `Samples/` for usage patterns (enumeration, capability checks, event listeners).

## Versioning
- Version scheme: `VERSION` provides MAJOR.MINOR; PATCH computed from git commit count via `SetVersionFromGit` and `build_IGCL.ps1`. Update `VERSION` when bumping MAJOR/MINOR.

## Expectations for Agents
- Keep APIs and helpers consistent with existing conventions; avoid breaking established patterns. Consistentcy is key across the whole codebase. Do not deviate from this consistentcy without first requesting permission from the user. 
- Respect disposal and pointer ownership rules; ensure safe lifetime handling.
- Prefer generated enums over custom ones to align with IGCL updates.
- Maintain optional-feature gating and hardware skip behavior in tests and helpers.
