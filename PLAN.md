## Goal and Constraints (read this first)
You must add a lightweight facade layer for IGCL (one helper per listed feature) and split the tests into Native vs Facade suites. Key rules:
- Native tests: MUST use only the ClangSharp-generated APIs in `cs_generated` (no facade types anywhere).
- Facade tests: MUST use only the new facade helpers (no direct `cs_generated` or raw P/Invoke).
- Existing `IGCLWrapper.Tests` project will be RENAMED to `IGCLWrapper.NativeTests`. A new `IGCLWrapper.FacadeTests` project will be CREATED.
- Do NOT edit anything in `cs_generated/`.
- Handle missing IGCL DLL or Intel GPU by skipping tests gracefully, not by failing.
- Keep lifetimes safe: `IGCLApiHelper` owns the API handle; helpers do not free handles, they only mark disposed.

## Current repo snapshot (what exists today)
- `IGCLWrapper/IGCLApi.cs`: Main API wrapper. Uses SafeHandle (`IGCLApiHandle`), exposes `IsIGCLDllAvailable`, version macros, struct init helpers. Enumerates adapters/displays with raw IntPtr handles.
- `IGCLWrapper/IGCLExtensions.cs` (aka `IGCLHelpers`): Utility methods on raw handles (adapter/display properties, timing, etc.).
- `IGCLWrapper.Tests/IGCLWrapper.Tests.csproj`: Single test project (will become NativeTests).
- `IGCLWrapper.sln`: References `IGCLWrapper` and `IGCLWrapper.Tests`.
- `IGCLWrapper/cs_generated/`: ClangSharp bindings (hands off).
- `IGCLWrapper/IGCLHardwareDetection.cs`: WMI-based Intel GPU detection (useful for skip conditions).

## Desired facade architecture (lightweight, no ref-counting)
General principles:
- Move/expose common static helpers in `IGCLApiHelper` so consumers don’t touch raw `IGCLApi`: `IsIGCLDllAvailable`, version helpers, struct initialization helpers.
- `IGCLApiHelper` owns the underlying `IGCLApi` (SafeHandle). It enumerates adapters and produces helpers for features; no separate `IGCLServicesHelper`.
- Adapter/display helpers hold a back-reference to `IGCLApiHelper` and the native handle. They never free the native handle; they only guard use-after-dispose.
- Feature helpers are adapter-scoped unless IGCL requires a display scope (LED could be either; clarify based on IGCL API). Keep names consistent with the provided list.

Concrete facade classes to add (all in `IGCLWrapper/`):
- `IGCLApiHelper`:
  - Static: `IsIGCLDllAvailable(out string error)`, version macros, struct init helpers for any IGCL struct used by facades (adapter props, display props, telemetry, 3D caps, etc.).
  - Instance: `Initialize()` -> returns `IGCLApiHelper`; `EnumerateAdapters()` -> `IReadOnlyList<IGCLAdapterHelper>`.
  - Factory/accessors for feature helpers, e.g., `GetPowerHelper(IGCLAdapterHelper adapter)` (optionally overloads by handle).
  - Dispose pattern: owns `IGCLApi` SafeHandle; guards disposed; no AddRef/Release.
- `IGCLAdapterHelper`:
  - Holds adapter handle (`IntPtr`) + back-reference to `IGCLApiHelper`.
  - Cached properties from `ctl_device_adapter_properties_t` (name, PCI IDs, device ID, flags).
  - Methods: `EnumerateDisplayOutputs()` -> `IReadOnlyList<IGCLDisplayHelper>`, and factory methods to get adapter-scoped feature helpers (could call back into `IGCLApiHelper`).
  - Dispose flag; guard all public methods.
- `IGCLDisplayHelper`:
  - Holds display handle + back-reference.
  - Cached properties from `ctl_display_properties_t` (name/EDID if exposed, active flag, timing info).
  - Convenience methods: `IsActive()`, `GetResolution()`, `GetRefreshRateHz()`, `GetTiming()`.
  - Dispose flag; guard all public methods.
- Feature helpers (one per requested feature; adapter-scoped unless IGCL dictates display scope):
  - `IGCL3DHelper`
  - `IGCLEccHelper`
  - `IGCLEngineHelper`
  - `IGCLFanHelper`
  - `IGCLFirmwareHelper`
  - `IGCLFrequencyHelper`
  - `IGCLLedHelper`
  - `IGCLMediaHelper`
  - `IGCLMemoryHelper`
  - `IGCLOverclockHelper`
  - `IGCLPciHelper`
  - `IGCLPowerHelper`
  - `IGCLTemperatureHelper`
Each helper:
  - Uses IGCL APIs to fetch caps/state/telemetry for the adapter (or display if appropriate).
  - Uses struct-init helpers from `IGCLApiHelper` to set `Size`/`Version` correctly.
  - Throws `IGCLException` on non-success; guards disposed.
  - No manual AddRef/Release; assumes handles remain valid while `IGCLApiHelper` is alive.

Thread-safety stance:
- Use `lock` around lazy property initialization if needed; otherwise keep synchronization minimal.

Error handling stance:
- Convert IGCL failures to `IGCLException`.
- Return empty lists for zero counts.
- Use `ObjectDisposedException` for use-after-dispose.
- Leverage `IGCLException.IsNoDisplayError` where relevant.

## Test project restructuring (hard requirements)
- Rename `IGCLWrapper.Tests` -> `IGCLWrapper.NativeTests`:
  - Move/rename folder and `.csproj`.
  - Update `AssemblyName` / `RootNamespace` in the csproj.
  - Update `IGCLWrapper.sln` project entry and configuration mappings to the new name/path.
  - Ensure all NativeTests use ONLY `cs_generated` APIs. No facade helper references.
- Add new `IGCLWrapper.FacadeTests`:
  - New xUnit project targeting `net10.0`, referencing `IGCLWrapper`.
  - Tests must use ONLY the facade helpers (no direct `cs_generated`).
  - Add skip guards based on `IsIGCLDllAvailable` and/or `IGCLHardwareDetection`.

Test coverage expectations:
- NativeTests:
  - Adapter enumeration via raw IGCL P/Invoke wrappers (`cs_generated` types); display enumeration; property/struct reads; disposal safety.
  - Skip gracefully if DLL or Intel GPU not present.
- FacadeTests:
  - Adapter/display enumeration via `IGCLApiHelper`.
  - Property/timing/refresh access via `IGCLDisplayHelper`.
  - Feature helper smoke tests for each helper class (power, temperature, 3D, etc.) gated on feature availability or skip if unsupported.
  - Disposal safety (use-after-dispose throws).

## Solution and docs updates (must-do)
- Update `IGCLWrapper.sln`:
  - Change the existing test project entry to `IGCLWrapper.NativeTests\IGCLWrapper.NativeTests.csproj` with new project name/GUID.
  - Add a new entry for `IGCLWrapper.FacadeTests\IGCLWrapper.FacadeTests.csproj`.
- Update any docs/README references:
  - Mention the test split (Native vs Facade).
  - Add a short example of facade usage (`IGCLApiHelper`, adapter/display enumeration, feature helper call).
- If any scripts exist that invoke `IGCLWrapper.Tests`, adjust to `IGCLWrapper.NativeTests` and add `IGCLWrapper.FacadeTests`.

## Detailed execution order (follow sequentially)
1) **Create IGCLApiHelper.cs**
   - Implement static helpers (DLL availability, version macros, struct init for all needed structs).
   - Implement `Initialize()` returning `IGCLApiHelper` that owns an `IGCLApi` instance.
   - Add `EnumerateAdapters()` returning `List<IGCLAdapterHelper>`.
   - Add factory methods for feature helpers (accept `IGCLAdapterHelper` or handle).
   - Implement dispose pattern; guard disposed in all public methods.
2) **Add adapter/display helpers**
   - `IGCLAdapterHelper.cs`: store adapter handle + back-reference; cache properties via struct init helpers; provide `EnumerateDisplayOutputs()` and feature helper accessors.
   - `IGCLDisplayHelper.cs`: store display handle + back-reference; cache display properties; convenience methods for active/resolution/refresh/timing.
3) **Add feature helpers (one class per feature)**
   - For each of: 3D, Ecc, Engine, Fan, Firmware, Frequency, Led, Media, Memory, Overclock, Pci, Power, Temperature.
   - Scope: adapter-level unless IGCL API specifies otherwise (LED might be display-level; adjust based on IGCL APIs).
   - Use struct init helpers from `IGCLApiHelper`; throw `IGCLException` on failure; guard disposed.
   - Keep each helper minimal: fetch caps/state/telemetry; no ref-counting.
4) **Restructure tests**
   - Rename folder `IGCLWrapper.Tests` -> `IGCLWrapper.NativeTests`.
   - Rename csproj to `IGCLWrapper.NativeTests.csproj`; update assembly/root namespace.
   - Fix any namespaces/usings in test code to match rename.
   - Ensure NativeTests reference only `cs_generated` (no facade types).
   - Add new `IGCLWrapper.FacadeTests` project (xUnit, net10.0); reference `IGCLWrapper`.
   - Implement FacadeTests using only the facade helpers. Add skip logic for missing DLL/hardware.
5) **Update solution and references**
   - Modify `IGCLWrapper.sln` to point to the renamed NativeTests project and add FacadeTests.
   - Adjust any scripts/docs that reference the old test project name.
6) **Docs update**
   - Add README section showing facade usage pattern (no Native code allowed):
     ```
     using var api = IGCLApiHelper.Initialize();
     var adapters = api.EnumerateAdapters();
     var displays = adapters[0].EnumerateDisplayOutputs();
     var power = api.GetPowerHelper(adapters[0]);
     ```
   - Note DLL/hardware prerequisites and test split.
   - Add README section showing Native usage pattern:
     ```
     using var api = IGCLApi.Initialize();
     ```
7) **Testing guidance**
   - Native: `dotnet test IGCLWrapper.NativeTests/IGCLWrapper.NativeTests.csproj` (will skip if no DLL/GPU).
   - Facade: `dotnet test IGCLWrapper.FacadeTests/IGCLWrapper.FacadeTests.csproj` (will skip if no DLL/GPU).

## Guardrails and reminders (do not skip)
- Do NOT edit files under `cs_generated/`.
- Native tests: only `cs_generated`. Facade tests: only facade helpers.
- Always guard for missing IGCL DLL or Intel GPU; prefer skipping tests to failing when unsupported.
- Always add XML comments for any functions so that docfx will generate accurate API documentation.
- Respect SafeHandle ownership: only `IGCLApiHelper`/`IGCLApi` should close the API handle; helper classes never free adapter/display handles.
