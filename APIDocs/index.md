---
_layout: landing
---

# ADLXWrapper API Docs

Welcome to the generated API reference for **ADLXWrapper**, a C# façade over the AMD ADLX SDK. Use this site to explore the public classes, helpers, and DTOs that make up the wrapper. 

The goal of ADLXWrapper project is to provide a lightweight, simpler way to access the AMD ADLX API, to read settings and make changes to the AMD GPU settings. The ADLXWrapper project provides Helper objects that provide a pointer-free, simple ergonomic API surface to make it easy to use, and it still exposies the native handles if you need to do something advanced.

## How to use

- Start at the API landing page: [ADLXWrapper API Reference](/api/ADLXWrapper.html).
- Navigate by feature: helpers like `ADLXDisplayServicesHelper`, `ADLXDesktopServicesHelper`, and `ADLXSystemServicesHelper` list the available operations and event hooks.
- Façade types (`ADLXDisplay`, `ADLXDesktop`, `ADLXGPU`) show the pointer-free workflows; use the `...Native()`/`...Handle()` methods only when you need raw ADLX handles.


## Where to Learn More

- **Project overview & usage**: See `README.md` at the repository root or the repo at https://github.com/terrymacdonald/ADLXWrapper for quick-start examples and patterns.
- **ADLX SDK reference**: The upstream ADLX SDK official docs are at https://gpuopen.com/manuals/adlx/.
- **ADLX SDK repository**: The upstream ADLX SDK repository is at https://github.com/GPUOpen-LibrariesAndSDKs/ADLX
- **Samples**: Runnable samples demonstrating display, desktop, GPU, and event-listener flows are under `Samples/`.
- **Wrapper internals**: Core helpers and façades are in `ADLXWrapper/` (e.g., `ADLXDisplayServicesHelper`, `ADLXDesktopServicesHelper`, `ADLXSystemServicesHelper`).

## Regenerating Docs

Use `.\refresh_adlx_api_docs.ps1` from the repo root to rebuild the DocFX site and serve it locally on port 8000.
