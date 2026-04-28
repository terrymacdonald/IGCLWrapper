# DTO Conversion Execution Plan (Managed-Only DTOs)

## Objective
Convert DTOs in IGCLWrapper/IGCLDisplayHelper.cs so DTO object graphs contain only managed primitives/enums/managed collections, with no nested native structs, while preserving native interop through FromNative/ToNative.

## Rules Being Enforced
- Keep enums as native generated enums when they are enums.
- Convert nested native structs into new `*Dto` types.
- Convert native `byte` bools using `IGCLDisplayDtoBool`.
- Convert fixed buffers to managed arrays/lists in DTOs.
- Convert pointer-backed DTO fields to managed collections/values.
- Exclude `Size`/`Version` from `Equals`/`GetHashCode` for converted DTOs.

## Phase Status
- [x] Phase 1: Convert PowerOptimization DTO graph
- [x] Phase 2: Convert DisplayProperties + AdapterDisplayEncoder DTO dependencies
- [x] Phase 3: Convert WireFormat + Lace + DCE DTO dependencies
- [x] Phase 4: Build validation and error cleanup

## Detailed Tasks
### Phase 1
- Add:
  - `PowerOptimizationDpstDto`
  - `PowerOptimizationPsrDto`
  - `PowerOptimizationLrrDto`
  - `PowerOptimizationFeatureSpecificInfoDto`
- Update `PowerOptimizationSettingsDto` to use managed nested DTO.
- Update equality/hash behavior to exclude `Size`/`Version`.

### Phase 2
- Add:
  - `GenericVoidDatatypeDto`
  - `OsDisplayEncoderIdentifierDto`
  - `RevisionDatatypeDto`
  - `DisplayTimingDto`
- Update:
  - `DisplayPropertiesDto`
  - `AdapterDisplayEncoderPropertiesDto`
- Convert reserved native fixed buffers to managed array fields where still native.

### Phase 3
- Add:
  - `WireFormatDto`
  - `LaceLuxAggrMapEntryDto`
  - `LaceLuxAggrMapDto`
  - `LaceAggrConfigDto`
- Update:
  - `WireFormatConfigDto`
  - `LaceConfigDto`
  - `DceArgsDto` (`Histogram` to managed list representation)

### Phase 4
- Run error check for IGCLDisplayHelper.cs.
- Fix compile issues caused by DTO type changes.
- Provide summary of converted DTOs and any follow-up needed.

## Progress Log
- 2026-04-28: Plan initialized and approved by user.
- 2026-04-28: Phase 1 completed in IGCLDisplayHelper.cs (PowerOptimization* DTO graph converted; PowerOptimizationSettingsDto now uses managed nested DTO and excludes Size/Version from equality/hash).
- 2026-04-28: Phase 2 completed in IGCLDisplayHelper.cs (added GenericVoidDatatypeDto/OsDisplayEncoderIdentifierDto/RevisionDatatypeDto/DisplayTimingDto and converted DisplayPropertiesDto + AdapterDisplayEncoderPropertiesDto with managed reserved fields).
- 2026-04-28: Phase 3 completed in IGCLDisplayHelper.cs (added WireFormatDto and Lace* DTO graph; converted WireFormatConfigDto, LaceConfigDto, and DceArgsDto histogram pointer to managed list representation).
- 2026-04-28: Phase 4 completed (IGCLWrapper.csproj builds successfully after DTO conversion changes).

## Facade Helper DTO Conversion (Wave 2)

### Phase Status
- [x] Phase A: Convert DTOs in IGCLApiHelper.cs
- [x] Phase B: Convert pointer/native DTOs in IGCL3DHelper.cs and IGCLMediaHelper.cs
- [x] Phase C: Convert struct-heavy DTOs in IGCLPciHelper.cs, IGCLOverclockHelper.cs, and IGCLPowerHelper.cs
- [x] Phase D: Build validation and cleanup

### Scope Notes
- Keep enums as native enums.
- Replace nested native structs with managed `*Dto` wrappers.
- Convert pointer-backed DTO fields to managed values/collections where feasible.
- Exclude `Size`/`Version` from `Equals`/`GetHashCode` for converted DTOs.

### Progress Log (Wave 2)
- 2026-04-28: Wave 2 plan started.
- 2026-04-28: Wave 2 Phase A completed in IGCLApiHelper.cs (added managed FirmwareVersion/AdapterBdf/Rect/ChildDisplayTargetMode/GenlockTopology DTOs and updated DeviceAdapterPropertiesDto, CombinedDisplayChildInfoDto, and GenlockArgsDto).
- 2026-04-28: Wave 2 Phase B completed in IGCL3DHelper.cs and IGCLMediaHelper.cs (added managed Property* DTO graph and replaced native ctl_property_t fields in 3D/media DTOs).
- 2026-04-28: Wave 2 Phase C completed in IGCLPciHelper.cs, IGCLOverclockHelper.cs, and IGCLPowerHelper.cs (added managed PciAddress/PciSpeed/DataValue DTOs and removed remaining nested native structs in these files).
- 2026-04-28: Wave 2 Phase D completed (IGCLWrapper.csproj builds successfully after all wave 2 DTO conversions).

## Facade Helper DTO Conversion (Wave 3 - List<T> Preference)

### Phase Status
- [x] Phase E: Convert remaining DTO managed arrays to `List<T>` across facade helpers
- [x] Phase F: Convert residual native DTO fields (`DisplaySettingsDto.Reserved`, `LedStateDto.Color`)
- [x] Phase G: Build validation and cleanup

### Scope Notes
- Apply user preference for `List<T>` over managed arrays in DTO fields.
- Keep native interop behavior intact through `FromNative`/`ToNative` marshaling helpers.
- Preserve existing helper naming and facade patterns.

### Progress Log (Wave 3)
- 2026-04-28: Wave 3 Phase E completed in IGCLApiHelper.cs, IGCLDisplayHelper.cs, IGCLMediaHelper.cs, IGCLOverclockHelper.cs, and IGCLFirmwareHelper.cs (converted DTO collection fields from managed arrays to `List<T>` and updated conversion/equality/hash helpers accordingly).
- 2026-04-28: Wave 3 Phase F completed in IGCLDisplayHelper.cs and IGCLLedHelper.cs (`DisplaySettingsDto.Reserved` moved to managed `List<uint>` and `LedStateDto.Color` moved to managed `LedColorDto`).
- 2026-04-28: Wave 3 Phase G completed (`dotnet build IGCLWrapper/IGCLWrapper.csproj -v minimal` succeeded).

## Facade Helper DTO Conversion (Wave 4 - ChildInfo List-Only Marshalling)

### Phase Status
- [x] Phase H: Remove `CombinedDisplayArgsDto.ChildInfo` pointer field and use `ChildInfos` as the managed source of truth
- [x] Phase I: Replace managed-array combined-display marshalling paths with `List<T>` + `stackalloc`
- [x] Phase J: Build validation

### Progress Log (Wave 4)
- 2026-04-28: Wave 4 Phase H completed in IGCLApiHelper.cs (`CombinedDisplayArgsDto.ChildInfo` removed; `ToNative()` now derives `NumOutputs` from `ChildInfos` when needed and leaves native `pChildInfo` null for call-site marshalling).
- 2026-04-28: Wave 4 Phase I completed in IGCLApiHelper.cs (combined display get/set marshalling paths changed from managed arrays to `List<CombinedDisplayChildInfoDto>` and `stackalloc ctl_combined_display_child_info_t[...]`).
- 2026-04-28: Wave 4 Phase J completed (`dotnet build IGCLWrapper/IGCLWrapper.csproj -v minimal` succeeded).
