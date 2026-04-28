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
