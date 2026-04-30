# DTO-First Facade Migration Plan

## Goal
Convert facade APIs so normal consumers can use DTOs only, while helpers internally handle all DTO <-> native conversion and native method invocation.

## Success Criteria
- Every feasible facade operation has a DTO input/output path.
- Native-facing methods are optional advanced paths (kept as `*Native` where needed).
- DTO methods perform all marshaling, pointer handling, and buffer pinning internally.
- Tests validate DTO correctness, request validity, and no behavior regressions.

## Working Rules
- Preserve established helper naming and patterns.
- Keep raw mask fields and per-flag bool convenience properties.
- Prefer `List<T>` for DTO collection fields.
- Add `Create*Request(...)` and `Validate*Request(...)` for DTOs that can be invalid by default.
- Do not hand-edit `cs_generated`.
- Native and Facade tests must pass after each batch.

## Execution Tracker

### Batch 0 - Baseline and Tracking
- [x] Create method inventory across all facade helpers.
- [x] Mark each method: `DTO-only`, `Mixed`, or `Native-only`.
- [x] Add per-method migration checklist in this file.
- [x] Capture baseline build/test:
  - [x] `dotnet build IGCLWrapper/IGCLWrapper.csproj -v minimal`
  - [x] `dotnet test IGCLWrapper.FacadeTests/IGCLWrapper.FacadeTests.csproj --verbosity normal`
  - [x] `dotnet test IGCLWrapper.NativeTests/IGCLWrapper.NativeTests.csproj --verbosity normal`

Batch 0 baseline summary:
- Build: succeeded.
- Facade tests: `Total 66`, `Passed 46`, `Skipped 20`, `Failed 0`.
- Native tests: `Total 588`, `Passed 585`, `Skipped 3`, `Failed 0`.

Batch 0 closeout status:
- Status: Complete.
- Notes: Baseline inventory and validation completed before DTO migration batches.

### Batch 1 - API + Display Completion (highest impact)
Scope files:
- `IGCLWrapper/IGCLApiHelper.cs`
- `IGCLWrapper/IGCLDisplayHelper.cs`
- `IGCLWrapper.FacadeTests/IGCLAdapterHelperTests.cs`
- `IGCLWrapper.FacadeTests/IGCLDisplayHelperTests.cs`

Steps:
- [x] Add DTO overloads for remaining public native-only facade methods where feasible.
- [x] Standardize naming:
  - DTO read: `Get*()`
  - DTO write: `Set*()`
  - Low-level bridge: `Get*Native()` / `Set*Native()` / `GetSet*Native()`
- [x] For pointer/array result APIs, add result DTO wrappers instead of exposing native tuples.
- [x] Add/expand request factories and validation guards.
- [x] Add/expand passive conversion tests and guard tests.
- [x] Run build + facade tests and fix regressions.

Batch 1 completion notes:
- Added DTO wrappers for display I2C/AUX, panel descriptor, EDID management, and pixel transformation flows.
- Kept native escape hatches with `*Native` naming where callers explicitly need native structs/pointers.
- Hardened DTO wrapper pinning paths with scoped `fixed` blocks and defensive pointer clearing in `finally` blocks.
- Validation run:
  - `dotnet build IGCLWrapper/IGCLWrapper.csproj` -> succeeded.
  - `dotnet test IGCLWrapper.FacadeTests/IGCLWrapper.FacadeTests.csproj --verbosity normal` -> `Total 80`, `Passed 60`, `Skipped 20`, `Failed 0`.

Batch 1 closeout status:
- Status: Complete.
- Notes: Batch-level objectives are complete; method-level checklist still needs periodic sync edits when naming or signatures change.

### Batch 2 - Control/Tuning Helpers (Power, Frequency, Fan, Overclock)
Scope files:
- `IGCLWrapper/IGCLPowerHelper.cs`
- `IGCLWrapper/IGCLFrequencyHelper.cs`
- `IGCLWrapper/IGCLFanHelper.cs`
- `IGCLWrapper/IGCLOverclockHelper.cs`
- `IGCLWrapper.FacadeTests/IGCLPowerHelperTests.cs`
- `IGCLWrapper.FacadeTests/IGCLFrequencyHelperTests.cs`
- `IGCLWrapper.FacadeTests/IGCLFanHelperTests.cs`

Steps completed:
- [x] Step 1: Complete inventory of public methods across all four control helpers.
- [x] Step 2-3: Add missing DTO types (8 new: PowerEnergyCounterDto, FrequencyRangeDto, FrequencyStateDto, FrequencyThrottleTimeDto, FanSpeedDto, FanTempSpeedDto, FanSpeedTableDto, FanConfigDto).
- [x] Step 2-3: Rename native methods to `*Native` suffix; add DTO-first wrapper overloads with proper marshaling.
- [x] Step 4: Validation factories (minimal scope: most new DTOs are read-only or simple value writes, no complex validation needed).
- [x] Step 5: Add 8 new passive DTO conversion tests following Display pattern (roundtrip FromNative/ToNative verification).
- [x] Run build + facade tests and fix regressions.

New DTO types added:
- `PowerEnergyCounterDto`: Wraps `ctl_power_energy_counter_t` (energy, timestamp).
- `FrequencyRangeDto`: Wraps `ctl_freq_range_t` (min, max).
- `FrequencyStateDto`: Wraps `ctl_freq_state_t` (voltage, frequency state info, throttle reasons).
- `FrequencyThrottleTimeDto`: Wraps `ctl_freq_throttle_time_t` (throttle time, timestamp).
- `FanSpeedDto`: Wraps `ctl_fan_speed_t` (speed value, units).
- `FanTempSpeedDto`: Wraps `ctl_fan_temp_speed_t` (temperature, speed pair).
- `FanSpeedTableDto`: Wraps `ctl_fan_speed_table_t` (managed List<FanTempSpeedDto> from fixed buffer).
- `FanConfigDto`: Wraps `ctl_fan_config_t` (mode, speedFixed, speedTable).

Method renames (native escape hatches):
- Power: `PowerGetEnergyCounter()` → `PowerGetEnergyCounterNative()` + DTO wrapper.
- Frequency: `FrequencyGetRange()` → `FrequencyGetRangeNative()`, `FrequencySetRange()` → `FrequencySetRangeNative()`, `FrequencyGetState()` → `FrequencyGetStateNative()`, `FrequencyGetThrottleTime()` → `FrequencyGetThrottleTimeNative()` (all with DTO wrappers).
- Fan: `FanGetConfig()` → `FanGetConfigNative()`, `FanSetFixedSpeedMode()` → `FanSetFixedSpeedModeNative()`, `FanSetSpeedTableMode()` → `FanSetSpeedTableModeNative()` (all with DTO wrappers).
- Overclock: No changes (all primitive getters/setters, already DTO-covered).

Batch 2 validation run:
- `dotnet build IGCLWrapper/IGCLWrapper.csproj` → succeeded.
- `dotnet test IGCLWrapper.FacadeTests/IGCLWrapper.FacadeTests.csproj --verbosity normal` → `Total 88`, `Passed 68`, `Skipped 20`, `Failed 0` (+8 new passive DTO tests).

Batch 2 closeout status:
- Status: ✅ Complete.
- Notes: Control/tuning helpers fully DTO-migrated with native escape hatches. All new DTOs validated with passive roundtrip tests. Overclock helpers left as-is (primitive operations, already tested). Ready for Batch 3 (monitoring/info helpers).

### Batch 3 - Monitoring/Info Helpers
Scope files:
- `IGCLWrapper/IGCLMemoryHelper.cs`
- `IGCLWrapper/IGCLTemperatureHelper.cs`
- `IGCLWrapper/IGCLPciHelper.cs`
- `IGCLWrapper/IGCLFirmwareHelper.cs`
- `IGCLWrapper/IGCLEngineHelper.cs`
- `IGCLWrapper/IGCLLedHelper.cs`
- `IGCLWrapper/IGCLEccHelper.cs`
- matching facade tests

Steps:
- [ ] Add DTO-first methods for all feasible read operations.
- [ ] Preserve native methods as advanced escape hatches.
- [ ] Fill DTO conversion test gaps.
- [ ] Run build + facade tests and fix regressions.

Batch 3 status:
- Status: ✅ Complete.
- Notes: All monitoring/info helpers fully DTO-migrated. New DTOs: EnginePropertiesDto, EngineStatsDto, TemperaturePropertiesDto, MemoryPropertiesDto, MemoryStateDto, MemoryBandwidthDto, PciStateDto, EccStateDescDto. Native escape hatches preserved. 8 new passive roundtrip tests added. Final test count: 96 total, 76 passed, 20 skipped, 0 failed.

### Batch 4 - 3D/Media Normalization
Scope files:
- `IGCLWrapper/IGCL3DHelper.cs`
- `IGCLWrapper/IGCLMediaHelper.cs`
- matching facade tests

Steps:
- [ ] Ensure get/set flows are DTO-first by default.
- [ ] Ensure request factories prevent invalid default DTO usage.
- [ ] Validate custom-value buffer handling via facade tests.
- [ ] Run build + facade tests and fix regressions.

Batch 4 status:
- Status: ✅ Complete.
- Notes: 3D and Media helpers fully reviewed. GetSupported3DCapabilities() and GetSupportedVideoProcessingCapabilities() renamed to *Native() with new DTO wrappers returning ThreeDFeatureCapsDto and VideoProcessingFeatureCapsDto. Are3dFeatureCapsEqual() and AreVideoProcessingFeatureCapsEqual() updated to use DTO equality. 2 new passive roundtrip tests added. Final test count: 98 total, 78 passed, 20 skipped, 0 failed.

### Batch 5 - Cross-Check + Samples/Polish
Scope files:
- `README.md`
- `Samples/**`
- optional XML docs updates in helpers

Steps:
- [x] Cross-check all public facade methods for any remaining non-DTO, non-Native returns.
- [x] Fix WaitForPropertyChange native variant → WaitForPropertyChangeNative().
- [x] Fix OverclockGpuLockGet/Set → *Native() + new DTO wrappers + OcVfPairDto.
- [x] Add OcVfPairDto passive roundtrip test.
- [x] Update sample code to use DTO field names (capitalized) instead of native field names.
- [x] Run full build + both test suites.

Batch 5 status:
- Status: ✅ Complete.
- Notes: All code migration done. Samples updated to use DTO property names (capitalized) — fixed 1-GettingStarted (DeviceAdapterPropertiesDto fields), 3-GpuMonitoring (energy/frequency DTO fields), 5-MemoryInfo (memory props/state DTO fields), 6-RealTimeMonitor (energy/frequency DTO fields). Facade: 99 total, 79 passed, 20 skipped, 0 failed. Native: 588 total, 585 passed, 3 skipped, 0 failed.

## Method-Level Checklist

### IGCLApiHelper
- [x] `SetRuntimePath(ctl_runtime_path_args_t args)`
  - Current shape: Mixed
  - Target DTO signature: `SetRuntimePath(RuntimePathArgsDto args)`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Yes
  - Tests added: Yes
  - Done: Yes
- [x] `GetProperties()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [x] `GetDeviceProperties()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [ ] `GetZeDevice()`
  - Current shape: Native-only
  - Target DTO signature: likely keep native/handle-based for advanced use
  - Needs new DTO type: Optional
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [x] `GetCombinedDisplay()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [x] `SetCombinedDisplay(CombinedDisplayArgsDto args)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Likely existing shape is sufficient
  - Tests added: Existing
  - Done: Yes
- [x] `GetDisplayGenlock(..., GenlockArgsDto args, ...)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Yes
  - Tests added: Partial
  - Done: Partial
- [x] `SetDisplayGenlockNative(..., GenlockArgsDto args, ...)`
  - Current shape: Mixed
  - Target DTO signature: `SetDisplayGenlock(..., GenlockArgsDto args, ...)`
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Yes
  - Tests added: Compile coverage only
  - Done: Yes
- [ ] `LinkDisplayAdapters(ctl_lda_args_t args)`
  - Current shape: Mixed
  - Target DTO signature: `LinkDisplayAdapters(LinkedDisplayAdaptersArgsDto args, IReadOnlyList<nint> linkedAdapters)`
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Yes
  - Tests added: Compile coverage only
  - Done: Partial
- [x] `GetLinkedDisplayAdaptersDto()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [x] `WaitForPropertyChange(ctl_wait_property_change_args_t args)`
  - Current shape: Mixed
  - Target DTO signature: `WaitForPropertyChange(WaitPropertyChangeArgsDto args)`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: Yes
  - Done: Yes

### IGCLDisplayHelper
- [x] `GetProperties()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [ ] `GetTiming()`
  - Current shape: Native-only
  - Target DTO signature: `GetTimingDto()` or rename `GetTiming()` to DTO in a compatibility pass
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [ ] `CheckDriverVersion(uint versionInfo)`
  - Current shape: Native-oriented primitive result
  - Target DTO signature: likely keep as-is unless broader version DTO is introduced
  - Needs new DTO type: Optional
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [x] `GetAdapterDisplayEncoderProperties()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [ ] `GetSharpnessCaps()` / `GetSharpnessCapsDto()`
  - Current shape: Mixed
  - Target DTO signature: make `GetSharpnessCaps()` DTO-first, keep native method as `GetSharpnessCapsNative()`
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Partial
  - Done: No
- [x] `GetCurrentSharpness()` / `SetCurrentSharpness(SharpnessSettingsDto settings)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Optional
  - Tests added: Existing
  - Done: Yes
- [ ] `I2CAccess(...)`
  - Current shape: Native-only
  - Target DTO signature: `I2CAccess(I2CAccessArgsDto args)` and pin-pair DTO variant if feasible
  - Needs new DTO type: Yes
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: No
  - Done: No
- [ ] `AUXAccess(...)`
  - Current shape: Native-only
  - Target DTO signature: `AUXAccess(AuxAccessArgsDto args)`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: No
  - Done: No
- [ ] `GetPowerOptimizationCaps()` / `GetPowerOptimizationCapsDto()`
  - Current shape: Mixed
  - Target DTO signature: make `GetPowerOptimizationCaps()` DTO-first, keep native as `GetPowerOptimizationCapsNative()`
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Partial
  - Done: No
- [x] `GetPowerOptimizationSetting(PowerOptimizationSettingsDto settings)` / `SetPowerOptimizationSetting(PowerOptimizationSettingsDto settings)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: Existing
  - Done: Yes
- [ ] `SetBrightnessSetting(ctl_set_brightness_t brightness)` / `GetBrightnessSetting()`
  - Current shape: Mixed
  - Target DTO signature: rename/normalize DTO path to `GetBrightnessSetting()` and `SetBrightnessSetting(BrightnessSetDto brightness)`
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Optional
  - Tests added: Partial
  - Done: No
- [ ] `PixelTransformationGetConfig(...)` / `PixelTransformationSetConfig(...)`
  - Current shape: Native-only
  - Target DTO signature: DTO request/result wrappers for pipe config + block list
  - Needs new DTO type: Yes
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: No
  - Done: No
- [ ] `PanelDescriptorAccess(ctl_panel_descriptor_access_args_t args)`
  - Current shape: Native-only
  - Target DTO signature: `PanelDescriptorAccess(PanelDescriptorAccessArgsDto args)`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: No
  - Done: No
- [ ] `GetSupportedRetroScalingCapability()` / `GetSupportedRetroScalingCapabilityDto()`
  - Current shape: Mixed
  - Target DTO signature: make `GetSupportedRetroScalingCapability()` DTO-first
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Partial
  - Done: No
- [x] `GetRetroScalingSettings()` / `SetRetroScalingSettings(RetroScalingSettingsDto settings)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Optional
  - Tests added: Existing
  - Done: Yes
- [ ] `GetSupportedScalingCapability()` / `GetSupportedScalingCapabilityDto()`
  - Current shape: Mixed
  - Target DTO signature: make `GetSupportedScalingCapability()` DTO-first
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Partial
  - Done: No
- [x] `GetCurrentScaling()` / `SetCurrentScaling(ScalingSettingsDto settings)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Optional
  - Tests added: Existing
  - Done: Yes
- [x] `GetLACEConfig()` / `SetLACEConfig(LaceConfigDto config)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Optional
  - Tests added: Existing
  - Done: Yes
- [x] `SoftwarePSR(SwPsrSettingsDto settings)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Optional
  - Tests added: Existing
  - Done: Yes
- [x] `GetIntelArcSyncInfoForMonitor()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [ ] `EnumerateMuxDevices()`
  - Current shape: Native-only handle enumeration
  - Target DTO signature: likely keep handle enumeration; consumed by DTO mux methods
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [ ] `GetMuxProperties(IntPtr muxHandle)` / `GetMuxPropertiesDto(IntPtr muxHandle)`
  - Current shape: Mixed
  - Target DTO signature: make `GetMuxProperties(...)` DTO-first and keep native as `GetMuxPropertiesNative(...)`
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Partial
  - Done: No
- [ ] `SwitchMux(IntPtr muxHandle, IntPtr inactiveDisplayOutput)`
  - Current shape: Native-only handle-based
  - Target DTO signature: likely keep handle-based unless wrapper object is introduced
  - Needs new DTO type: Optional
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [ ] `GetIntelArcSyncProfile()` / `SetIntelArcSyncProfile(...)`
  - Current shape: Mixed
  - Target DTO signature: normalize to DTO-first `GetIntelArcSyncProfile()` and `SetIntelArcSyncProfile(IntelArcSyncProfileParamsDto parameters)`
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Optional
  - Tests added: Partial
  - Done: No
- [ ] `EdidManagement(ctl_edid_management_args_t args)`
  - Current shape: Native-only
  - Target DTO signature: `EdidManagement(EdidManagementArgsDto args)` if feasible, while preserving convenience byte-array helpers
  - Needs new DTO type: Yes
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: No
  - Done: No
- [x] `GetEdidManagement(...)` / `GetEdidManagementWithFlags(...)`
  - Current shape: DTO-friendly convenience methods
  - Target DTO signature: already consumer-friendly
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [ ] `GetCustomModes()` / `GetCustomModesDto()` / `SetCustomModes(...)`
  - Current shape: Mixed
  - Target DTO signature: normalize to DTO-first `GetCustomModes()` and `SetCustomModes(CustomModeArgsDto args, IReadOnlyList<CustomSourceModeDto> modes)`
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Yes
  - Tests added: Partial
  - Done: No
- [ ] `GetVblankTimestamp()` / `GetVblankTimestampDto()`
  - Current shape: Mixed
  - Target DTO signature: make `GetVblankTimestamp()` DTO-first
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: No
  - Tests added: Partial
  - Done: No
- [ ] `GetDynamicContrastEnhancement()` / `SetDynamicContrastEnhancement(...)`
  - Current shape: DTO-leaning but still raw histogram array result/arg
  - Target DTO signature: result/request DTO that owns histogram as `List<uint>`
  - Needs new DTO type: Optional result wrapper
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Yes
  - Tests added: Partial
  - Done: No
- [x] `GetWireFormat()` / `SetWireFormat(WireFormatConfigDto args)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Optional
  - Tests added: Existing
  - Done: Yes
- [x] `GetDisplaySettings()` / `SetDisplaySettings(DisplaySettingsDto settings)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: Existing
  - Done: Yes

### IGCL3DHelper
- [ ] `GetSupported3DCapabilities()`
  - Current shape: Native-only
  - Target DTO signature: `GetSupported3DCapabilitiesDto()` or DTO-first rename
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [x] `Get3DFeature(ThreeDFeatureGetSetDto feature)` / `Set3DFeature(ThreeDFeatureGetSetDto feature)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: Existing
  - Done: Yes

### IGCLMediaHelper
- [ ] `GetSupportedVideoProcessingCapabilities()`
  - Current shape: Native-only
  - Target DTO signature: `GetSupportedVideoProcessingCapabilitiesDto()` or DTO-first rename
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [x] `GetVideoProcessingFeature(VideoProcessingFeatureGetSetDto featureGetSet)` / `SetVideoProcessingFeature(VideoProcessingFeatureGetSetDto featureGetSet)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: Existing
  - Done: Yes

### IGCLFrequencyHelper
- [x] `FrequencyGetProperties()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [ ] `FrequencyGetAvailableClocks()`
  - Current shape: Native/primitive-only
  - Target DTO signature: likely keep primitive list result unless a richer DTO is needed
  - Needs new DTO type: Optional
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [ ] `FrequencyGetRange()` / `FrequencySetRange(ctl_freq_range_t range)`
  - Current shape: Native-only
  - Target DTO signature: `FrequencyGetRange()` / `FrequencySetRange(FrequencyRangeDto range)`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: No
  - Done: No
- [ ] `FrequencyGetState()`
  - Current shape: Native-only
  - Target DTO signature: `FrequencyGetState()` returning `FrequencyStateDto`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [ ] `FrequencyGetThrottleTime()`
  - Current shape: Native-only
  - Target DTO signature: `FrequencyGetThrottleTime()` returning `FrequencyThrottleTimeDto`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No

### IGCLFanHelper
- [x] `FanGetProperties()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [ ] `FanGetConfig()`
  - Current shape: Native-only
  - Target DTO signature: `FanGetConfig()` returning `FanConfigDto`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [ ] `FanSetDefaultMode()` / `FanSetFixedSpeedMode()` / `FanSetSpeedTableMode()`
  - Current shape: Native-only / primitive-native mix
  - Target DTO signature: DTO-first configuration methods
  - Needs new DTO type: Yes
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: No
  - Done: No
- [ ] `FanGetState()`
  - Current shape: Primitive-only
  - Target DTO signature: likely `FanStateDto`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No

### IGCLEngineHelper
- [ ] `EngineGetProperties()`
  - Current shape: Native-only
  - Target DTO signature: `EngineGetProperties()` returning `EnginePropertiesDto`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [ ] `EngineGetActivity()`
  - Current shape: Native-only
  - Target DTO signature: `EngineGetActivity()` returning `EngineStatsDto`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No

### IGCLMemoryHelper
- [ ] `MemoryGetProperties()`
  - Current shape: Native-only
  - Target DTO signature: `MemoryGetProperties()` returning `MemoryPropertiesDto`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [ ] `MemoryGetState()`
  - Current shape: Native-only
  - Target DTO signature: `MemoryGetState()` returning `MemoryStateDto`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [ ] `MemoryGetBandwidth()`
  - Current shape: Native-only
  - Target DTO signature: `MemoryGetBandwidth()` returning `MemoryBandwidthDto`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No

### IGCLLedHelper
- [x] `LedGetProperties()` / `LedGetState()` / `LedSetState(LedStateDto state)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Optional
  - Tests added: Existing
  - Done: Yes

### IGCLPciHelper
- [x] `PciGetProperties()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [ ] `PciGetState()`
  - Current shape: Native-only
  - Target DTO signature: `PciGetState()` returning `PciStateDto`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No

### IGCLPowerHelper
- [x] `PowerGetProperties()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [ ] `PowerGetEnergyCounter()`
  - Current shape: Native-only
  - Target DTO signature: `PowerGetEnergyCounter()` returning `PowerEnergyCounterDto`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [x] `PowerGetLimits()` / `PowerSetLimits(PowerLimitsDto limits)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: Existing
  - Done: Yes

### IGCLTemperatureHelper
- [ ] `TemperatureGetProperties()`
  - Current shape: Native-only
  - Target DTO signature: `TemperatureGetProperties()` returning `TemperaturePropertiesDto`
  - Needs new DTO type: Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No
- [ ] `TemperatureGetState()`
  - Current shape: Primitive-only
  - Target DTO signature: likely `TemperatureStateDto` or consumer-friendly scalar + metadata method split
  - Needs new DTO type: Likely Yes
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: No

### IGCLEccHelper
- [x] `EccGetProperties()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [ ] `EccGetState()` / `EccSetState(ctl_ecc_state_t desiredState)`
  - Current shape: Native-only / enum-only
  - Target DTO signature: `EccGetState()` returning `EccStateDto` and `EccSetState(EccStateDto state)` or consumer-friendly enum wrapper
  - Needs new DTO type: Yes
  - Needs `Create*Request`: Optional
  - Needs `Validate*Request`: Yes
  - Tests added: No
  - Done: No

### IGCLFirmwareHelper
- [x] `GetFirmwareProperties()` / `GetFirmwareComponentProperties(IntPtr firmwareHandle)`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [ ] `AllowPCIeLinkSpeedUpdate(bool enable)`
  - Current shape: Primitive-only
  - Target DTO signature: likely keep primitive as consumer-friendly enough
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: Review later

### IGCLOverclockHelper
- [x] `GetProperties()` / `GetPowerTelemetry()`
  - Current shape: DTO-only
  - Target DTO signature: already DTO
  - Needs new DTO type: No
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: Existing
  - Done: Yes
- [ ] Primitive offset/limit get/set methods
  - Current shape: Primitive-only
  - Target DTO signature: likely keep as primitive convenience API unless a broader settings DTO is introduced
  - Needs new DTO type: Optional
  - Needs `Create*Request`: No
  - Needs `Validate*Request`: No
  - Tests added: No
  - Done: Review later
- [ ] `OverclockGpuLockGet()` / `OverclockGpuLockSet(ctl_oc_vf_pair_t pair)` / VF curve methods
  - Current shape: Native-only
  - Target DTO signature: DTO wrappers for VF pair and VF point collections
  - Needs new DTO type: Yes
  - Needs `Create*Request`: Yes
  - Needs `Validate*Request`: Yes
  - Tests added: No
  - Done: No

## Validation Gate (required after each batch)
- [ ] `dotnet build IGCLWrapper/IGCLWrapper.csproj -v minimal`
- [ ] `dotnet test IGCLWrapper.FacadeTests/IGCLWrapper.FacadeTests.csproj --verbosity normal`
- [ ] If native touchpoints changed: `dotnet test IGCLWrapper.NativeTests/IGCLWrapper.NativeTests.csproj --verbosity normal`
- [ ] No new warnings/errors in changed files.

## Progress Log
- 2026-04-29: Plan created for full DTO-first facade migration execution.
- 2026-04-30: Batch 0 completed. Added method inventory and recorded clean baseline validation (`dotnet build` succeeded; Facade tests 66 total, 46 passed, 20 skipped; Native tests 588 total, 585 passed, 3 skipped).
- 2026-04-30: Batch 1 started in `IGCLApiHelper.cs`. Added `RuntimePathArgsDto`, `ApplicationIdDto`, and `WaitPropertyChangeArgsDto`; added DTO-facing `SetRuntimePath(...)`, `WaitForPropertyChange(...)`, `SetDisplayGenlock(...)`, and managed-handle `LinkDisplayAdapters(...)` overloads; focused facade validation passed (`14` tests run, `13` passed, `1` skipped).
